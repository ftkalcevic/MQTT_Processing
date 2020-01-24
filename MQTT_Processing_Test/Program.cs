using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTT_Processinglib;
using MqttConfig;
using System.Configuration;

namespace MQTT_Processing_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MqttProcessingApp app = new MqttProcessingApp();
            app.LogMsgEvent += App_LogMsgEvent;
            app.Run();
            System.Console.WriteLine("Hit any key to quit");
            System.Console.ReadLine();
            app.Stop();
        }

        private static void App_LogMsgEvent(string name, string msg)
        {
            Console.WriteLine($"{name}: {msg}");
        }
    }
}
