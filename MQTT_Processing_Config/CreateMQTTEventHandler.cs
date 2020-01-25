// MQTT Processing - MQTT Message event handler and configuration
// 
// Copyright(C) 2020  Frank Tkalcevic (frank@franksworkshop.com.au)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using MQTT_Processinglib;

namespace MQTT_Processing_Config
{
    public partial class CreateMQTTEventHandler : Form
    {
        MqttHandlerConfig config;
        string mqttHost;
        int mqttPort;
        IDatabase db;

        public CreateMQTTEventHandler(string host, int port)
        {
            CommonConstructor(host, port);
        }

        public CreateMQTTEventHandler(string host, int port, string Topic, string Message)
        {
            CommonConstructor(host, port);
            config.sampleTopic = Topic;
            config.sampleMessage = Message;
            ParseMessage(config.sampleMessage);
            txtSampleTopic.Text = config.sampleTopic;
            txtSampleMessage.Text = config.sampleMessage;
            txtSubscribeTopic.Text = config.sampleTopic;
        }

        public CreateMQTTEventHandler(string host, int port, XmlDocument xml)
        {
            CommonConstructor(host, port);
            config.UnpackXml(xml);

            txtDescription.Text = config.description;
            txtSubscribeTopic.Text = config.subscribeTopic;
            txtSampleTopic.Text = config.sampleTopic;
            txtSampleMessage.Text = config.sampleMessage;
            txtConnectionString.Text = config.connectionString;
            txtDatabase.Text = config.database;
            txtTable.Text = config.table;
            olvTopic.SetObjects(config.topicNodes);
            treeListView.SetObjects(config.jsonNodes);
            treeListView.ExpandAll();
            treeListView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void CommonConstructor(string host, int port)
        {
            config = new MqttHandlerConfig();
            mqttHost = host;
            mqttPort = port;
            InitializeComponent();
            txtSampleTopic.Text = config.sampleTopic;
            txtSampleMessage.Text = config.sampleMessage;
            
            treeListView.CanExpandGetter = CanExpandGetter;
            treeListView.ChildrenGetter = ChildrenGetter;

            db = new SqlDatabase();
        }

        public bool CanExpandGetter(object x)
        {
            System.Diagnostics.Debug.Assert(x != null);
            JsonNode node = (JsonNode)x;
            return node.children.Count > 0;
        }

        public IEnumerable<JsonNode> ChildrenGetter(object x)
        {
            JsonNode node = (JsonNode)x;
            return node.children;
        }

        public void ParseMessage(string message)
        {
            config.ParseJSON(message);

            treeListView.SetObjects(config.jsonNodes);
            treeListView.ExpandAll();
            treeListView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        

        private void btnCheckDB_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            CheckDBResults results = db.CheckDB(txtConnectionString.Text, txtDatabase.Text, txtTable.Text, config.jsonNodes, config.allJsonNodes, config.topicNodes);

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
            config.description = txtDescription.Text;
            config.subscribeTopic = txtSubscribeTopic.Text;
            config.sampleTopic = txtSampleTopic.Text;
            config.sampleMessage = txtSampleMessage.Text;
            config.connectionString = txtConnectionString.Text;
            config.database = txtDatabase.Text;
            config.table = txtTable.Text;

            XmlDocument xml = config.PackXML();
            return xml;
        }

        private void ctxmnuDelete_Click(object sender, EventArgs e)
        {
            if (olvTopic.SelectedItem != null)
            {
                config.topicNodes.RemoveAt(olvTopic.SelectedIndex);
                olvTopic.SetObjects(config.topicNodes);
            }
        }

        private void ctxmnuAdd_Click(object sender, EventArgs e)
        {
            TopicNode t = new TopicNode();
            t.SetRegEx(".*", txtSampleTopic.Text);
            t.dbColumn = "";
            t.dbType = SqlDbType.VarChar;
            t.primaryKey = false;

            config.topicNodes.Add(t);
            olvTopic.SetObjects(config.topicNodes);
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
                db.CreateTable(txtConnectionString.Text, txtDatabase.Text, txtTable.Text, config.jsonNodes, config.allJsonNodes, config.topicNodes);
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

        private void btnProcessSample_Click(object sender, EventArgs e)
        {
            if (config.allJsonNodes.Count != 0)
            {
                if (MessageBox.Show("Processing the sample JSON will delete existing configuration", "Process Sample", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;
            }
            ParseMessage(txtSampleMessage.Text);
        }

    }
}
