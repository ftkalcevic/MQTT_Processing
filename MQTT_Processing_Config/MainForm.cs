using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using System.Xml;

namespace MQTT_Processing_Config
{
    public partial class MainForm : Form
    {
        MqttClient mqttClient;
        bool listening;
        BindingSource mqttMessages;
        DataGridViewCellEventArgs mouseLocation;

        public MainForm()
        {
            listening = false;
            mqttMessages = null;
            InitializeComponent();
            txtMQTTHostname.Text = Properties.Settings.Default.MQTTHost;
            txtMQTTPort.Text = Properties.Settings.Default.MQTTPort.ToString();
            txtTopicString.Text = Properties.Settings.Default.MQTTSearchTopic;
            EnableMenuItems();
        }

        private void btnMQTTListen_Click(object sender, EventArgs e)
        {
            if (listening)
            {
                mqttClient.Disconnect();
                mqttClient = null;
                listening = false;
                btnMQTTListen.Text = "Listen";
            }
            else
            {
                mqttMessages = new BindingSource();
                dataGridView1.DataSource = mqttMessages;
                dataGridView1.AutoSize = true;

                string host = txtMQTTHostname.Text;
                int port = int.Parse(txtMQTTPort.Text);
                mqttClient = new MqttClient(host, port, false, null, null, MqttSslProtocols.None);
                string clientId = Guid.NewGuid().ToString();
                mqttClient.Connect(clientId);
                mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                mqttClient.Subscribe(new string[] { txtTopicString.Text }, new byte[] { 0 });
                listening = true;
                btnMQTTListen.Text = "Stop Listening";
                mouseLocation = null;
            }
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            // This callback is on a different thread
            dataGridView1.Invoke(new Action(() =>
           {
               System.Diagnostics.Debug.WriteLine("Got msg: " + e.Topic + " " + e.Message.ToString());
               mqttMessages.Add(new MQTTMessage(e.Topic, Encoding.ASCII.GetString(e.Message)));
           }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listening)
            {
                mqttClient.Disconnect();
                mqttClient = null;
                listening = false;
            }
            Properties.Settings.Default.MQTTHost = txtMQTTHostname.Text;
            Properties.Settings.Default.MQTTPort = int.Parse(txtMQTTPort.Text);
            Properties.Settings.Default.MQTTSearchTopic = txtTopicString.Text;
        }

        private void EnableMenuItems()
        {
            menuItemCreateMQTTEvent.Enabled = mqttMessages != null && mqttMessages.Current != null;
        }

        private void menuItemCreateMQTTEvent_Click(object sender, EventArgs e)
        {
            if (mouseLocation != null)
            {
                int row = mouseLocation.RowIndex;
                dataGridView1.Rows[row].Selected = true;
                MQTTMessage msg = (MQTTMessage)dataGridView1.Rows[row].DataBoundItem;
                CreateMQTTEvent(msg.Topic, msg.Message);
            }
        }

        private void CreateMQTTEvent(string topic, string message)
        {
            string host = txtMQTTHostname.Text;
            int port = int.Parse(txtMQTTPort.Text);

            Form f = new CreateMQTTEventHandler(host, port, topic, message);
            f.ShowDialog(this);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            EnableMenuItems();
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "MQTT Processing files (*.mqtp)|*.mqtp|All files (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                dialog.Multiselect = false;
                dialog.CheckPathExists = true;
                dialog.CheckFileExists = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string filepath = dialog.FileName;
                    Program.mru.AddFile(filepath);

                    OpenFileDialog(filepath);
                }
            }
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            // Update MRU on file menu
            if (Program.mru.mruFiles.Count > 0)
            {
                ToolStripItemCollection items = fileToolStripMenuItem.DropDownItems;

                int index = 0;
                for (; index < items.Count; index++)
                {
                    if ((string)items[index].Tag == "MRU")
                    {
                        // Remove anything after the tag, till the next separator
                        while (items.Count > index+1 && !(items[index + 1] is ToolStripSeparator))
                        {
                            items.RemoveAt(index + 1);
                        }
                        // Hide tag
                        items[index].Visible = false;
                        index++;
                        // Add MRUs
                        foreach (MRUFile f in Program.mru.mruFiles)
                        {
                            ToolStripMenuItem item = new ToolStripMenuItem(f.filename);
                            item.ToolTipText = f.filepath;
                            items.Insert(index, item);
                            item.Click += MRU_Item_Click;
                            index++;
                        }
                        break;
                    }
                }
            }
        }

        private void MRU_Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            OpenFileDialog(item.ToolTipText);
        }

        private void OpenFileDialog(string filepath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filepath);
            string host = txtMQTTHostname.Text;
            int port = int.Parse(txtMQTTPort.Text);
            CreateMQTTEventHandler form = new CreateMQTTEventHandler(host,port,xml);
            form.ShowDialog(this);
        }
    }
}