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

using System.Text.RegularExpressions;

namespace MQTT_Processinglib
{
    public class TopicNode : DatabaseData
    {
        public string regEx { get; private set; }
        public string sample { get; private set; }

        public void SetRegEx(string exp, string sampleTopic="")
        {
            regEx = exp;
            Regex ex = new Regex(exp);
            Match m = ex.Match(sampleTopic);
            if (m.Success && m.Groups.Count > 1)
            {
                sample = m.Groups[1].Value;
            }
            else
            {
                sample = "";
            }
        }
    }
}
