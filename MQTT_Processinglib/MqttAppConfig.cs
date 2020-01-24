using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;

namespace MQTT_Processinglib
{
    class MqttAppConfig
    {
        static public List<string> GetHandlerFiles()
        {
            List<string> files = new List<string>();

            XmlDocument xml = new XmlDocument();
            xml.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlNode node in xml.SelectNodes("/configuration/mqttEventHandlers/mqttEventHandler"))
            {
                files.Add(node.GetAttribute("File"));
            }
            return files;
        }
       
    }
}
