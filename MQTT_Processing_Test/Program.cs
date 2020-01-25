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
