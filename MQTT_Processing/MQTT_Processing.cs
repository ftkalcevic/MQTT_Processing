using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using MQTT_Processinglib;


namespace MQTT_Processing
{
    public partial class MQTT_Processing : ServiceBase
    {
        MqttProcessingApp app;

        public MQTT_Processing()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            app = new MqttProcessingApp();
            app.LogMsgEvent += App_LogMsgEvent;
            app.Run();
        }

        protected override void OnStop()
        {
            app.Stop();
            app = null;
        }

        private static void App_LogMsgEvent(string name, string msg)
        {
            Trace.TraceInformation($"{name}: {msg}");
        }

    }
}
