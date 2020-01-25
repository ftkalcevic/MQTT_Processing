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
