using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json.Linq;
using uPLibrary.Networking.M2Mqtt;

namespace MQTT_Processing_Config
{

    public partial class TestHandler : Form
    {
        // handler members
        string description;
        string subscribeTopic;
        string connectionString;
        string database;
        string table;
        List<DatabaseData> fields;

        MqttClient mqttClient;
        string host;
        int port;

        IDatabase db;

        public TestHandler(string mqttHost, int mqttPort, System.Xml.XmlDocument xml)
        {
            host = mqttHost;
            port = mqttPort;

            InitializeComponent();

            InitialiseHandler(xml);
            db = new SqlDatabase();
            db.CreateDBObjects(connectionString, database, table, fields);
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            try
            {
                string topic = e.Topic;
                string msgString = System.Text.Encoding.UTF8.GetString(e.Message);
                JObject msg = JObject.Parse(msgString);

                this.Invoke(new Action(() => 
                { 
                    Log(topic);
                    Log(msgString);
                }));

                foreach (var col in fields)
                {
                    if (col is JsonNode)
                    {
                        JsonNode n = (JsonNode)col;
                        JToken value = msg.SelectToken(n.parentPath + "." + n.name);

                        db.SetParameter(col.dbColumn, value);
                    }
                    else if (col is TopicNode)
                    {
                        Regex re = new Regex(((TopicNode)col).regEx);
                        Match m = re.Match(topic);
                        string match = "";
                        if (m.Success && m.Groups.Count > 1)
                            match = m.Groups[1].Value;
                        db.SetParameter(col.dbColumn, match);
                    }
                }
                db.WriteRecord();
                this.Invoke(new Action(() => { Log(db.LastCommand); }));

            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => { Log("Failed processing mqtt message: " + ex.ToString()); }));
            }

        }

        private void InitialiseHandler(XmlDocument xml)
        {
            XmlNode root = xml.SelectSingleNode("/MQTTProcessing");
            description = root.GetAttribute("Description", "");
            subscribeTopic = xml.GetElementData("/MQTTProcessing/MQTT/SubscribeTopic");

            fields = new List<DatabaseData>();
            foreach (XmlNode node in xml.SelectNodes("/MQTTProcessing/Message/Fields//Field"))
            {
                if (bool.Parse(node.GetAttribute("InUse")))
                {
                    string name = node.GetAttribute("Name");
                    string parentPath = node.GetAttribute("ParentPath");
                    JTokenType jsonType = (JTokenType)Enum.Parse(typeof(JTokenType), node.GetAttribute("JSONType"), ignoreCase: true);
                    JsonNode j = new JsonNode(parentPath, name, jsonType, "");
                    j.dbColumn = node.GetAttribute("DBColumn");
                    j.dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), node.GetAttribute("DBType"), ignoreCase: true);
                    j.dbSize = int.Parse(node.GetAttribute("DBSize"));
                    j.primaryKey = bool.Parse(node.GetAttribute("PrimaryKey"));

                    fields.Add(j);
                }
            }

            foreach (XmlNode node in xml.SelectNodes("/MQTTProcessing/Message/TopicFields/TopicField"))
            {
                TopicNode t = new TopicNode();
                t.dbColumn = node.GetAttribute("DBColumn");
                t.dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), node.GetAttribute("DBType"), ignoreCase: true);
                t.dbSize = int.Parse(node.GetAttribute("DBSize"));
                t.primaryKey = bool.Parse(node.GetAttribute("PrimaryKey"));
                t.SetRegEx(node.FirstChildData(), "");
                fields.Add(t);
            }

            connectionString = xml.GetElementData("/MQTTProcessing/Database/ConnectString");
            database = xml.GetElementData("/MQTTProcessing/Database/Database");
            table = xml.GetElementData("/MQTTProcessing/Database/Table");
        }

        private void TestHandler_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mqttClient != null && mqttClient.IsConnected)
                mqttClient.Disconnect();
            mqttClient = null;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (mqttClient == null)
            {
                btnStart.Text = "Stop";
                mqttClient = new MqttClient(host, port, false, null, null, MqttSslProtocols.None);
                string clientId = Guid.NewGuid().ToString();
                mqttClient.Connect(clientId);
                mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                mqttClient.Subscribe(new string[] { subscribeTopic }, new byte[] { 0 });
            }
            else
            {
                btnStart.Text = "Start";
                mqttClient.Disconnect();
                mqttClient = null;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Log(string msg)
        {
            txtLog.AppendText(msg+"\n");
        }
    }
}
