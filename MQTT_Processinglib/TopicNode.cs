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
