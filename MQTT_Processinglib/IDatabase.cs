using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace MQTT_Processinglib
{
    public class CheckDBResults
    {
        public string messages = "";
        public bool success = false;
        public bool hasTable = false;
        public bool hasData = false;
        public List<string> tooManyColumns = new List<string>();
        public List<string> wrongTypeColumns = new List<string>();
        public List<string> missingColumns = new List<string>();
        public List<string> wrongPrimaryKey = new List<string>();
        public List<string> missingPrimaryKey = new List<string>();
    }

    public interface IDatabase
    {
        void Initialise(XmlDocument xml);
        void CreateDBObjects(string connectionString, string database, string table, List<DatabaseData> fields);
        void SetParameter(string column, object data);
        void WriteRecord();
        CheckDBResults CheckDB(string connectionString, string databaseName, string tableName, List<JsonNode> jsonNodes, List<JsonNode> allJsonNodes, List<TopicNode> topicNodes);
        void CreateTable(string connectionString, string databaseName, string tableName, List<JsonNode> jsonNodes, List<JsonNode> allJsonNodes, List<TopicNode> topicNodes);
        string LastCommand { get; }
    }
}
