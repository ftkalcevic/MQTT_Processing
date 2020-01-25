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
