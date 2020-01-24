using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json.Linq;
using uPLibrary.Networking.M2Mqtt;
using MQTT_Processinglib;

namespace MQTT_Processing_Config
{

    public partial class TestHandler : Form
    {
        MqttHandler handler;

        public TestHandler(string mqttHost, int mqttPort, System.Xml.XmlDocument xml)
        {
            InitializeComponent();

            handler = new MqttHandler("");
            handler.LogMsgEvent += Handler_LogMsgEvent;
            handler.Init(mqttHost, mqttPort, xml);
        }

        private void Handler_LogMsgEvent(string name, string msg)
        {
            txtLog.Invoke(new Action(() =>
            {
                Log(msg);
            }));
        }


        private void TestHandler_FormClosing(object sender, FormClosingEventArgs e)
        {
            handler.Stop();
            handler = null;
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
