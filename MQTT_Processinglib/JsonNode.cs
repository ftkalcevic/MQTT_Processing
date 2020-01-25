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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace MQTT_Processinglib
{
    public class JsonNode: DatabaseData
    {
        public string name { get; private set; }
        public JTokenType type { get; private set; }
        public string value { get; private set; }
        public string parentPath { get; private set; }
        public List<JsonNode> children { get; private set; }
        public bool inUse { get; set; }

        public JsonNode(string parentPath, string name, JTokenType type, string value)
        {
            this.name = name;
            this.dbColumn = name;
            this.type = type;
            this.value = value;
            this.parentPath = parentPath;
            this.children = new List<JsonNode>();
            switch (type)
            {
                case JTokenType.String: dbType = SqlDbType.VarChar; this.dbSize = 50; break;
                case JTokenType.Integer: dbType = SqlDbType.Int; this.dbSize = 0; break;
                case JTokenType.Float: dbType = SqlDbType.Float; this.dbSize = 0; break;
                case JTokenType.Boolean: dbType = SqlDbType.Bit; this.dbSize = 0; break;
                case JTokenType.Date: dbType = SqlDbType.DateTime; this.dbSize = 0; break;
                case JTokenType.Object: dbType = SqlDbType.Bit; this.dbSize = 0; break;
                default: throw new Exception($"Unhandled JTokenType={type}");
            }
        }

        public void AddChild(JsonNode child)
        {
            children.Add(child);
        }
    }
}
