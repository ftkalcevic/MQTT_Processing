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
using System.Xml;
using System.Data;

namespace MQTT_Processinglib
{
    public class CheckDBResults
    {
        public string messages = "";
        public bool success = false;
        public bool hasTable = false;
        public bool hasData = false;
        public List<string> tooManyColumns = new List<string>();
        public List<string> wrongTypeColumns = new List<string>();
        public List<string> missingColumns = new List<string>();
        public List<string> wrongPrimaryKey = new List<string>();
        public List<string> missingPrimaryKey = new List<string>();
    }

    public interface IDatabase
    {
        void Initialise(XmlDocument xml);
        void CreateDBObjects(string connectionString, string database, string table, List<DatabaseData> fields);
        void SetParameter(string column, object data);
        void WriteRecord();
        CheckDBResults CheckDB(string connectionString, string databaseName, string tableName, List<JsonNode> jsonNodes, List<JsonNode> allJsonNodes, List<TopicNode> topicNodes);
        void CreateTable(string connectionString, string databaseName, string tableName, List<JsonNode> jsonNodes, List<JsonNode> allJsonNodes, List<TopicNode> topicNodes);
        string LastCommand { get; }
    }
}
