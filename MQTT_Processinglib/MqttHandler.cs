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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using uPLibrary.Networking.M2Mqtt;
using System.Xml;

namespace MQTT_Processinglib
{
    public class MqttHandler
    {
        public delegate void LogMsgEventHandler(string name, string msg);
        public event LogMsgEventHandler LogMsgEvent;

        string name;
        MqttClient mqttClient;
        string clientId;
        MqttHandlerConfig config;
        IDatabase db;

        public MqttHandler(string name)
        {
            this.name = name;
        }

        public void Init(string host, int port, string handlerFile)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(handlerFile);
            Init(host, port, xml);
        }

        public void Init(string host, int port, XmlDocument xml)
        {
            config = new MqttHandlerConfig();
            config.UnpackXml(xml);

            db = new SqlDatabase(); // TODO database selection should be in the config file.
            db.CreateDBObjects(config.connectionString, config.database, config.table, config.activeNodes);

            mqttClient = new MqttClient(host, port, false, null, null, MqttSslProtocols.None);
            clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(clientId);
            mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
            mqttClient.Subscribe(new string[] { config.subscribeTopic }, new byte[] { 0 });
        }

        public void Stop()
        {
            if (mqttClient != null && mqttClient.IsConnected)
            {
                mqttClient.Disconnect();
                mqttClient = null;
            }
        }

        public void Log(string msg)
        {
            if (LogMsgEvent != null)
                LogMsgEvent(name, msg);
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            try
            {
                string topic = e.Topic;
                string msgString = System.Text.Encoding.UTF8.GetString(e.Message);
                JObject msg = JObject.Parse(msgString);

                Log(topic);
                Log(msgString);

                foreach (var col in config.activeNodes)
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
                Log(db.LastCommand);

            }
            catch (Exception ex)
            {
                Log("Failed processing mqtt message: " + ex.ToString());
            }

        }
    }
}
