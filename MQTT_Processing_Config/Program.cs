using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQTT_Processing_Config
{
    static class Program
    {
        public static MRU mru;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            mru = new MRU();

            mru.LoadMRU(Properties.Settings.Default.MRU);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            Properties.Settings.Default.MRU = mru.SaveMRU();

            Properties.Settings.Default.Save();
        }
    }
}
