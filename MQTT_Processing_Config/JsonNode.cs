using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace MQTT_Processing_Config
{
    public class DatabaseData
    {
        public string dbColumn { get; set; }
        public SqlDbType dbType { get; set; }
        public bool primaryKey { get; set; }
        public int dbSize { get; set; }
    }

    public class TopicNode : DatabaseData
    {
        public string regEx { get; private set; }
        public string sample { get; private set; }

        internal void SetRegEx(string exp, string sampleTopic)
        {
            regEx = exp;
            Regex ex = new Regex(exp);
            Match m = ex.Match(sampleTopic);
            if (m.Success && m.Groups.Count > 1)
            {
                sample = m.Groups[1].Value;
            }
            else
            {
                sample = "";
            }
        }
    }

    public class JsonNode: DatabaseData
    {
        public string name { get; private set; }
        public JTokenType type { get; private set; }
        public string value { get; private set; }
        public string parentPath { get; private set; }
        public List<JsonNode> children { get; private set; }
        public bool inUse { get; set; }

        public JsonNode() { }
        public JsonNode(string parentPath, string name, JTokenType type, string value)
        {
            this.name = name;
            this.dbColumn = name;
            this.type = type;
            this.value = value;
            this.parentPath = parentPath;
            this.children = new List<JsonNode>();
            switch (type)
            {
                case JTokenType.String: dbType = SqlDbType.VarChar; this.dbSize = 50; break;
                case JTokenType.Integer: dbType = SqlDbType.Int; this.dbSize = 0; break;
                case JTokenType.Float: dbType = SqlDbType.Float; this.dbSize = 0; break;
                case JTokenType.Boolean: dbType = SqlDbType.Bit; this.dbSize = 0; break;
                case JTokenType.Date: dbType = SqlDbType.DateTime; this.dbSize = 0; break;
                case JTokenType.Object: dbType = SqlDbType.Bit; this.dbSize = 0; break;
                default: throw new Exception($"Unhandled JTokenType={type}");
            }
        }

        public void AddChild(JsonNode child)
        {
            children.Add(child);
        }
    }
}
