using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace MQTT_Processing_Config
{
    class SqlDatabase : IDatabase
    {
        bool hasPrimaryKey = false;
        SqlCommand insertCmd;
        string connectionString;
        string database;
        string table;
        string lastCommand;

        public SqlDatabase()
        {
        }

        public void Initialise(XmlDocument xml)
        {
        }

        public void CreateDBObjects(string connectionString, string database, string table, List<DatabaseData> fields)
        {
            this.connectionString = connectionString;
            this.database = database;
            this.table = table;

            string insertQuery = $"INSERT INTO [dbo].{table}\n";
            insertQuery += "(";
            bool first = true;
            hasPrimaryKey = false;
            foreach (var col in fields)
            {
                if (col.primaryKey)
                    hasPrimaryKey = true;
                insertQuery += (first ? "" : ",") + $"[{col.dbColumn}]";
                first = false;
            }
            insertQuery += ")\n";
            insertQuery += "select ";
            first = true;
            foreach (var col in fields)
            {
                insertQuery += (first ? "" : ",") + $"@{col.dbColumn}";
                first = false;
            }
            insertQuery += "\n";
            if (hasPrimaryKey)
            {
                insertQuery += $"where not exists (select 1 from [dbo].[{table}] where \n";
                first = true;
                foreach (var col in fields)
                    if (col.primaryKey)
                    {
                        insertQuery += (first ? "" : " and ") + $"[{col.dbColumn}] = @{col.dbColumn}";
                        first = false;
                    }
                insertQuery += ")";
            }

            insertCmd = new SqlCommand(insertQuery);
            foreach (var col in fields)
                insertCmd.Parameters.Add("@" + col.dbColumn, col.dbType, col.dbSize);
        }

        public void SetParameter(string column, object data)
        {
            insertCmd.Parameters["@" + column].Value = data;
        }

        public void WriteRecord()
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand($"use {database}", con))
                {
                    cmd.ExecuteNonQuery();
                }

                insertCmd.Connection = con;
                insertCmd.ExecuteNonQuery();
                con.Close();

                lastCommand = insertCmd.CommandText + MakeString(insertCmd.Parameters);
            }
        }

        private string MakeString(SqlParameterCollection parameters)
        {
            string s="";
            foreach (SqlParameter p in parameters)
            {
                s += $"{p.ParameterName} = '{p.Value.ToString()}'";
            }
            return s;
        }

        public CheckDBResults CheckDB(string connectionString, string databaseName, string tableName, List<JsonNode> jsonNodes, List<JsonNode> allJsonNodes, List<TopicNode> topicNodes )
        {
            CheckDBResults results = new CheckDBResults();
            results.success = false;

            // Check the connection
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                results.messages = "Failed to connect to database server: " + ex.Message;
                return results;
            }

            // Check the database
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (var cmd = new SqlCommand($"use {databaseName}", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                results.messages = "Failed to access database: " + ex.Message;
                return results;
            }

            // Check the table - we need to check if it exists, if the columns are there (and correct) and if the primary key is set.
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (var cmd = new SqlCommand($"use {databaseName}", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Does table exist
                    using (var cmd = new SqlCommand($"select OBJECT_ID(N'{tableName}',N'U')", con))
                    {
                        var ret = cmd.ExecuteScalar();

                        if (DBNull.Value.Equals(ret))
                        {
                            // No table
                            results.hasTable = false;
                            results.messages = $"No table '{tableName}'";
                            results.success = true;
                            return results;
                        }
                        else
                            results.hasTable = true;
                    }

                    // Is there data
                    using (var cmd = new SqlCommand($"select top 1 1 from {tableName}", con))
                    {
                        var ret = cmd.ExecuteScalar();

                        if (DBNull.Value.Equals(ret))
                        {
                            // No data
                            System.Diagnostics.Debug.WriteLine($"No data in table '{tableName}'");
                            results.hasData = false;
                        }
                        else
                        {
                            // data
                            System.Diagnostics.Debug.WriteLine($"Table has data '{tableName}'");
                            results.hasData = true;
                        }
                    }

                    // Check columns
                    Dictionary<string, DatabaseData> columns = (from node in allJsonNodes where node.children.Count == 0 && node.inUse select node).ToDictionary(p => p.dbColumn, p => (DatabaseData)p);
                    columns.AddRange((from node in topicNodes select node).ToDictionary(p => p.dbColumn, p => (DatabaseData)p));
                    Dictionary<string, DatabaseData> primaryKeys = (from node in allJsonNodes where node.primaryKey select node).ToDictionary(p => p.dbColumn, p => (DatabaseData)p);
                    primaryKeys.AddRange((from node in topicNodes where node.primaryKey select node).ToDictionary(p => p.dbColumn, p => (DatabaseData)p));
                    using (var cmd = new SqlCommand($"sp_help {tableName}", con))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            do
                            {
                                while (rdr.Read())
                                {
                                    bool foundColumns = false;
                                    bool foundConstraints = false;
                                    for (int i = 0; i < rdr.FieldCount; i++)
                                        if (rdr.GetName(i) == "Column_name")
                                        {
                                            foundColumns = true;
                                            break;
                                        }
                                        else if (rdr.GetName(i) == "constraint_type")
                                        {
                                            foundConstraints = true;
                                            break;
                                        }

                                    if (foundColumns)
                                    {
                                        // We have the table result set.
                                        do
                                        {
                                            string columnName = rdr.GetString(rdr.GetOrdinal("Column_name"));
                                            string type = rdr.GetString(rdr.GetOrdinal("type"));
                                            // check column
                                            DatabaseData col = null;
                                            if (!columns.TryGetValue(columnName, out col))
                                            {
                                                results.tooManyColumns.Add(columnName);
                                                results.messages += $"Column in database is not in our list '{columnName}'\n";
                                            }
                                            else
                                            {
                                                if (col.dbType.ToString().ToLower() != type.ToLower())
                                                {
                                                    results.wrongTypeColumns.Add(columnName);
                                                    results.messages += $"Column '{columnName}' has wrong type DB='{col.dbType}' JSON='{type}'\n";
                                                }
                                                columns.Remove(columnName);
                                            }
                                        }
                                        while (rdr.Read());
                                        // list missing columns
                                        foreach (var kvp in columns)
                                        {
                                            results.missingColumns.Add(kvp.Value.dbColumn);
                                            results.messages += $"Missing column '{kvp.Value.dbColumn}'\n";
                                        }
                                    }
                                    if (foundConstraints)
                                    {
                                        // We have the constraints - Primary key
                                        do
                                        {
                                            string constraintType = rdr.GetString(rdr.GetOrdinal("constraint_type"));
                                            string constraintKeys = rdr.GetString(rdr.GetOrdinal("constraint_keys"));
                                            if (constraintType == "PRIMARY KEY (clustered)")
                                            {
                                                // check keys
                                                string[] keys = constraintKeys.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (var key in keys)
                                                {
                                                    DatabaseData col = null;
                                                    if (!primaryKeys.TryGetValue(key, out col))
                                                    {
                                                        results.messages += $"Column '{key}' is not a primary key\n";
                                                        results.wrongPrimaryKey.Add(key);
                                                    }
                                                    else
                                                    {
                                                        primaryKeys.Remove(key);
                                                    }
                                                }

                                            }
                                        }
                                        while (rdr.Read());
                                        // list missing columns
                                        foreach (var kvp in primaryKeys)
                                        {
                                            results.messages += $"Primary key not in definition '{kvp.Value.dbColumn}'\n";
                                            results.missingPrimaryKey.Add(kvp.Value.dbColumn);
                                        }
                                    }
                                }
                            }
                            while (rdr.NextResult());
                            rdr.Close();
                        }
                    }

                    con.Close();
                }
                results.success = true;
            }
            catch (Exception ex)
            {
                results.messages += "Failed to access database: " + ex.Message;
            }

            return results;
        }

        public void CreateTable(string connectionString, string databaseName, string tableName, List<JsonNode> jsonNodes, List<JsonNode> allJsonNodes, List<TopicNode> topicNodes)
        {
            CheckDBResults results = CheckDB(connectionString, databaseName, tableName, jsonNodes, allJsonNodes, topicNodes);

            if (!results.success)
            {
                throw new Exception("Errors verifying database:\n" + results.messages);
            }

            if (!results.hasTable)
            {
                // Not table, so we just create the lot.
                List<DatabaseData> columns = (from node in allJsonNodes where node.children.Count == 0 && node.inUse select (DatabaseData)node).ToList();
                columns.AddRange(from node in topicNodes select (DatabaseData)node);
                List<DatabaseData> primaryKeys = (from node in allJsonNodes where node.primaryKey select (DatabaseData)node).ToList();
                primaryKeys.AddRange(from node in topicNodes where node.primaryKey select (DatabaseData)node);

                string query = "";
                query += $"CREATE TABLE [dbo].[{tableName}](\n";
                bool first = true;
                foreach (var col in columns)
                {
                    if (!first)
                        query += ",\n";
                    string isNullable = col.primaryKey ? "NOT NULL" : "NULL";
                    string len = col.dbType == SqlDbType.VarChar ? $"({col.dbSize})" : "";
                    query += $"[{col.dbColumn}] [{col.dbType.ToString()}] {len} {isNullable}";
                    first = false;
                }
                if (primaryKeys.Count() > 0)
                {
                    query += $",\nCONSTRAINT [PK_{tableName}] PRIMARY KEY CLUSTERED (\n";
                    first = true;
                    foreach (var col in primaryKeys)
                    {
                        if (!first)
                            query += ",\n";
                        query += $"[{col.dbColumn}] ASC";
                        first = false;
                    }
                    query += $"\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
                }
                query += $"\n) ON [PRIMARY]\n";
                System.Diagnostics.Debug.WriteLine(query);

                lastCommand = query;

                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (var cmd = new SqlCommand($"use {databaseName}", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
        }
        public string LastCommand { get { return lastCommand; } }
    }
}
