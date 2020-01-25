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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqttConfig;
using System.Configuration;

namespace MQTT_Processinglib
{
    public class MqttProcessingApp
    {
        public event MqttHandler.LogMsgEventHandler LogMsgEvent;
        List<MqttHandler> handlers;

        public MqttProcessingApp()
        {
            handlers = new List<MqttHandler>();
        }

        public void Run()
        {
            global::MqttConfig.MqttConfig config = (global::MqttConfig.MqttConfig)ConfigurationManager.GetSection("mqttConfig");
            string server = config.BrokerServer;
            int port = config.BrokerPort;
            List<String> handlerConfigs = new List<string>();
            foreach (MqttEventHandler h in config.MqttEventHandlers)
            {
                string name = h.Key;
                string file = h.File;
                MqttHandler handler = new MqttHandler(name);
                handler.LogMsgEvent += Handler_LogMsgEvent;
                handler.Init(server,port,file);
                handlers.Add(handler);
            }
        }

        private void Handler_LogMsgEvent(string name, string msg)
        {
            if (LogMsgEvent != null)
                LogMsgEvent(name, msg);
        }

        public void Stop()
        {
            foreach (var handler in handlers)
            {
                handler.Stop();
            }
        }
    }
}
