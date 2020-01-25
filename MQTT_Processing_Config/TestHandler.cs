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
