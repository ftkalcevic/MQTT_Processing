using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace MQTT_Processing_Config
{
    public partial class CreateMQTTEventHandler : Form
    {
        string mqttHost;
        int mqttPort;
        string sampleTopic;
        string sampleMessage;
        List<JsonNode> jsonNodes;
        List<JsonNode> allJsonNodes;
        List<TopicNode> topicNodes;
        IDatabase db;

        public CreateMQTTEventHandler(string host, int port)
        {
            CommonConstructor(host, port);
        }

        public CreateMQTTEventHandler(string host, int port, string Topic, string Message)
        {
            sampleTopic = Topic;
            sampleMessage = Message;
            CommonConstructor(host, port);
            ParseMessage(sampleMessage);
            txtSubscribeTopic.Text = sampleTopic;
        }

        public CreateMQTTEventHandler(string host, int port, XmlDocument xml)
        {
            CommonConstructor(host, port);
            UnpackXml(xml);
        }

        private void CommonConstructor(string host, int port)
        {
            mqttHost = host;
            mqttPort = port;
            InitializeComponent();
            txtSampleTopic.Text = sampleTopic;
            txtSampleMessage.Text = sampleMessage;
            jsonNodes = new List<JsonNode>();
            allJsonNodes = new List<JsonNode>();
            topicNodes = new List<TopicNode>();
            olvTopic.SetObjects(topicNodes);
            
            treeListView.CanExpandGetter = CanExpandGetter;
            treeListView.ChildrenGetter = ChildrenGetter;

            db = new SqlDatabase();
        }

        public bool CanExpandGetter(object x)
        {
            JsonNode node = (JsonNode)x;
            return node.children.Count > 0;
        }

        public IEnumerable<JsonNode> ChildrenGetter(object x)
        {
            JsonNode node = (JsonNode)x;
            return node.children;
        }

        private void UnpackXml(XmlDocument xml)
        {
            XmlNode root = xml.SelectSingleNode("/MQTTProcessing");
            txtDescription.Text = root.GetAttribute("Description", "");

            txtSubscribeTopic.Text = xml.GetElementData("/MQTTProcessing/MQTT/SubscribeTopic", "");
            txtSampleTopic.Text = xml.GetElementData("/MQTTProcessing/MQTT/SampleTopic", "");
            txtSampleMessage.Text = xml.GetElementData("/MQTTProcessing/MQTT/SampleMessage","");

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
                t.SetRegEx(node.FirstChildData(""), txtSampleTopic.Text);
                topicNodes.Add(t);
            }

            txtConnectionString.Text = xml.GetElementData("/MQTTProcessing/Database/ConnectString", "");
            txtDatabase.Text = xml.GetElementData("/MQTTProcessing/Database/Database", "");
            txtTable.Text = xml.GetElementData("/MQTTProcessing/Database/Table", "");

            olvTopic.SetObjects(topicNodes);
            treeListView.SetObjects(jsonNodes);
            treeListView.ExpandAll();
            treeListView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
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

        public void iterateJson(JsonNode parent, string parentPath, JObject json)
        {
            foreach (var x in json)
            {
                string name = x.Key;
                string value = x.Value.ToString();
                JTokenType type = x.Value.Type;

                if (type == JTokenType.Object)
                {
                    JsonNode newParent = new JsonNode(parentPath, name, type, "");
                    if (parent != null)
                        parent.AddChild(newParent);
                    else
                        jsonNodes.Add(newParent);
                    allJsonNodes.Add(newParent);
                    iterateJson(newParent, parentPath + (parentPath.Length > 0 ? "." : "") + name, (JObject)x.Value);
                }
                else
                {
                    JsonNode newChild = new JsonNode(parentPath, name, type, value);
                    if (parent != null)
                        parent.AddChild(newChild);
                    else
                        jsonNodes.Add(newChild);
                    allJsonNodes.Add(newChild);
                }
            }
        }
    

        public void ParseMessage(string message)
        {
            JObject json = (JObject)JsonConvert.DeserializeObject(message);
            iterateJson(parent: null, parentPath: "", json: json);

            treeListView.SetObjects(jsonNodes);
            treeListView.ExpandAll();
            treeListView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

        }

        private void treeListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void btnCheckDB_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            CheckDBResults results = db.CheckDB(txtConnectionString.Text, txtDatabase.Text, txtTable.Text, jsonNodes, allJsonNodes, topicNodes);

            Cursor.Current = Cursors.Default;

            if (!results.success)
            {
                MessageBox.Show("Errors occurred validating the database" + Environment.NewLine + results.messages, "Database Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCreateTable.Enabled = false;
            }
            else if ( results.messages.Length > 0 )
            {
                MessageBox.Show("Issues validating the database" + Environment.NewLine + results.messages, "Check Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (results.hasTable)
                    btnCreateTable.Text = "Update Table";
                else
                    btnCreateTable.Text = "Create Table";
                btnCreateTable.Enabled = true;
            }
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            XmlDocument xml = PackXML();

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "MQTT Processing files (*.mqtp)|*.mqtp|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string filepath = dialog.FileName;
                    Program.mru.AddFile(filepath);

                    using (var s = File.CreateText(filepath))
                    {
                        s.Write(xml.OuterXml);
                        s.Close();

                    }
                }
            }

        }

        private XmlDocument PackXML()
        {
            XmlDocument xml = new XmlDocument();

            XmlNode root = xml.AppendChild(xml.CreateElement("MQTTProcessing"));
            root.AddAttribute("Description", txtDescription.Text);

            XmlNode mqtt = root.AppendElement("MQTT");
            mqtt.AppendCDataNode("SubscribeTopic", txtSubscribeTopic.Text);
            mqtt.AppendCDataNode("SampleTopic", txtSampleTopic.Text);
            mqtt.AppendCDataNode("SampleMessage", txtSampleMessage.Text);

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
            db.AppendCDataNode("ConnectString", txtConnectionString.Text);
            db.AppendElement("Database", txtDatabase.Text);
            db.AppendElement("Table", txtTable.Text);
            return xml;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TopicNode t = new TopicNode();
            t.SetRegEx(".*", txtSampleTopic.Text);
            t.dbColumn = "";
            t.dbType = SqlDbType.VarChar;
            t.primaryKey = false;

            topicNodes.Add(t);
            olvTopic.SetObjects(topicNodes);
        }

        private void olvTopic_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.Column.Index == 0 && !e.Cancel)
            {
                ((TopicNode)e.RowObject).SetRegEx(e.NewValue.ToString(),txtSampleTopic.Text);
                olvTopic.RefreshItem(e.ListViewItem);
                e.Cancel = true;
            }
        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                db.CreateTable(txtConnectionString.Text, txtDatabase.Text, txtTable.Text, jsonNodes, allJsonNodes, topicNodes);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Errors creating database:\n" + ex.ToString(), "Database Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            XmlDocument xml = PackXML();
            TestHandler dialog = new TestHandler(mqttHost, mqttPort, xml);
            dialog.ShowDialog(this);
        }
    }
}
