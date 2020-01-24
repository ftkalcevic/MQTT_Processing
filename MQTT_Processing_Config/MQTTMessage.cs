using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Processing_Config
{
    class MQTTMessage
    {
        public string Topic { get; private set; }
        public string Message { get; private set; }
        public MQTTMessage(string Topic, string Message)
        {
            this.Topic = Topic;
            this.Message = Message;
        }
    }
}
