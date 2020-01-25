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
using System.Xml;

namespace MQTT_Processinglib
{
    public static class xmlExtensions
    {
        public static XmlNode AddAttribute(this XmlNode node, string name, string value)
        {
            XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
            attr.Value = value;
            return node.Attributes.Append(attr);
        }

        public static XmlNode AppendElement(this XmlNode node, string name, string value="")
        {
            XmlElement elem = node.OwnerDocument.CreateElement(name);
            if (value.Length > 0)
                elem.AppendChild(node.OwnerDocument.CreateTextNode(value));
            return node.AppendChild(elem);
        }

        public static XmlNode AppendCDataNode(this XmlNode node, string name, string data)
        {
            XmlElement elem = node.OwnerDocument.CreateElement(name);
            XmlCDataSection cdata = node.OwnerDocument.CreateCDataSection(data);
            elem.AppendChild(cdata);
            return node.AppendChild(elem);
        }

        public static string GetElementData(this XmlNode node, string xpath, string defaultValue=null)
        {
            XmlNode n = node.SelectSingleNode(xpath);
            return n.FirstChildData(defaultValue);
        }

        public static string FirstChildData(this XmlNode node, string defaultValue=null)
        {
            if (node != null)
            {
                if (node.FirstChild is XmlCDataSection)
                {
                    XmlCDataSection cdata = (XmlCDataSection)node.FirstChild;
                    if (cdata != null)
                        return cdata.Value;
                }
                if (node.FirstChild is XmlText)
                {
                    XmlText text = (XmlText)node.FirstChild;
                    if (text != null)
                        return text.Value;
                }
            }
            if (defaultValue == null)
                throw new XmlException($"Missing required Text or CData node '{node.Name}'");
            return defaultValue;
        }

        public static string GetAttribute(this XmlNode node, string name, string defaultValue = null )
        {
            XmlAttribute attr = (XmlAttribute)node.Attributes.GetNamedItem(name);
            if (attr == null)
            {
                if (defaultValue == null)
                    throw new XmlException($"Required attibute '{name}' missing");
                return defaultValue;
            }
            else 
                return attr.Value;
        }


        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }

        public static void AddRange<T>(this IEnumerable<T> source, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Collection is null");
            if (source == null)
                throw new ArgumentNullException("Source is null");

            foreach (var item in collection)
            {
                source.Append(item);
            }
        }
    }
}
