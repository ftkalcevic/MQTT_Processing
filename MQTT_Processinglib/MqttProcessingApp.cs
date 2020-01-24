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
            MqttConfig.MqttConfig config = (MqttConfig.MqttConfig)ConfigurationManager.GetSection("mqttConfig");
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
