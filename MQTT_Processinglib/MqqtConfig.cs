using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace MQTT_Processinglib
{
    public class MqqtConfig
    {
        public string description;
        public string subscribeTopic;
        public string sampleTopic;
        public string sampleMessage;
        public List<JsonNode> jsonNodes;
        public List<JsonNode> allJsonNodes;
        public List<TopicNode> topicNodes;
        public List<DatabaseData> activeNodes;
        public string connectionString;
        public string database;
        public string table;

        public MqqtConfig()
        {
            jsonNodes = new List<JsonNode>();
            allJsonNodes = new List<JsonNode>();
            topicNodes = new List<TopicNode>();
            activeNodes = new List<DatabaseData>();
        }

        public void UnpackXml(string filename)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            UnpackXml(xml);
        }

        public void UnpackXml(XmlDocument xml)
        {
            XmlNode root = xml.SelectSingleNode("/MQTTProcessing");
            description = root.GetAttribute("Description", "");

            subscribeTopic = xml.GetElementData("/MQTTProcessing/MQTT/SubscribeTopic", "");
            sampleTopic = xml.GetElementData("/MQTTProcessing/MQTT/SampleTopic", "");
            sampleMessage = xml.GetElementData("/MQTTProcessing/MQTT/SampleMessage", "");

            foreach (XmlNode node in xml.SelectNodes("/MQTTProcessing/Message/Fields/Field"))
            {
                JsonNode j = ReadField(null, node);
                jsonNodes.Add(j);
            }

            foreach (XmlNode node in xml.SelectNodes("/MQTTProcessing/Message/TopicFields/TopicField"))
            {
                TopicNode t = new TopicNode();
                t.dbColumn = node.GetAttribute("DBColumn", "");
                t.dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), node.GetAttribute("DBType", "VarChar"), ignoreCase: true);
                t.dbSize = int.Parse(node.GetAttribute("DBSize", "0"));
                t.primaryKey = bool.Parse(node.GetAttribute("PrimaryKey", "false"));
                t.SetRegEx(node.FirstChildData(""), sampleTopic);
                topicNodes.Add(t);
            }
            foreach (JsonNode node in allJsonNodes)
                if (node.inUse)
                    activeNodes.Add(node);
            foreach (TopicNode node in topicNodes)
                activeNodes.Add(node);

            connectionString = xml.GetElementData("/MQTTProcessing/Database/ConnectString", "");
            database = xml.GetElementData("/MQTTProcessing/Database/Database", "");
            table = xml.GetElementData("/MQTTProcessing/Database/Table", "");
        }

        private JsonNode ReadField(JsonNode parent, XmlNode node)
        {
            JsonNode j = new JsonNode(node.GetAttribute("ParentPath", ""),
                                      node.GetAttribute("Name", ""),
                                      (JTokenType)Enum.Parse(typeof(JTokenType), node.GetAttribute("JSONType", "String"), ignoreCase: true),
                                      node.GetAttribute("SampleValue", ""));
            j.dbColumn = node.GetAttribute("DBColumn", "");
            j.dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), node.GetAttribute("DBType", "VarChar"), ignoreCase: true);
            j.dbSize = int.Parse(node.GetAttribute("DBSize", "0"));
            j.inUse = bool.Parse(node.GetAttribute("InUse", "false"));
            j.primaryKey = bool.Parse(node.GetAttribute("PrimaryKey", "false"));
            if (parent != null)
                parent.AddChild(j);
            allJsonNodes.Add(j);

            foreach (XmlNode child in node.SelectNodes("Field"))
                ReadField(j, child);

            return j;
        }

        private XmlDocument PackXML()
        {
            XmlDocument xml = new XmlDocument();

            XmlNode root = xml.AppendChild(xml.CreateElement("MQTTProcessing"));
            root.AddAttribute("Description", description);

            XmlNode mqtt = root.AppendElement("MQTT");
            mqtt.AppendCDataNode("SubscribeTopic", subscribeTopic);
            mqtt.AppendCDataNode("SampleTopic", sampleTopic);
            mqtt.AppendCDataNode("SampleMessage", sampleMessage);

            XmlNode msg = root.AppendElement("Message");
            XmlNode fields = msg.AppendElement("Fields");
            foreach (JsonNode node in jsonNodes)
            {
                WriteField(fields, node);
            }
            XmlNode topics = msg.AppendElement("TopicFields");
            foreach (TopicNode node in topicNodes)
            {
                XmlNode topic = topics.AppendCDataNode("TopicField", node.regEx);
                topic.AddAttribute("DBColumn", node.dbColumn);
                topic.AddAttribute("DBType", node.dbType.ToString());
                topic.AddAttribute("DBSize", node.dbSize.ToString());
                topic.AddAttribute("PrimaryKey", node.primaryKey.ToString());
            }

            XmlNode db = root.AppendElement("Database");
            db.AppendCDataNode("ConnectString", connectionString);
            db.AppendElement("Database", database);
            db.AppendElement("Table", table);
            return xml;
        }

        private void WriteField(XmlNode parent, JsonNode node)
        {
            XmlNode field = parent.AppendElement("Field");
            field.AddAttribute("Name", node.name);
            field.AddAttribute("SampleValue", node.value);
            field.AddAttribute("JSONType", node.type.ToString());
            field.AddAttribute("InUse", node.inUse.ToString());
            field.AddAttribute("DBColumn", node.dbColumn);
            field.AddAttribute("DBType", node.dbType.ToString());
            field.AddAttribute("DBSize", node.dbSize.ToString());
            field.AddAttribute("PrimaryKey", node.primaryKey.ToString());
            field.AddAttribute("ParentPath", node.parentPath);

            foreach (var child in node.children)
            {
                WriteField(field, child);
            }
        }



    }
}
