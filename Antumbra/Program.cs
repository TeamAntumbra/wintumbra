using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Antumbra.Glow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid)) {
                if (!mutex.WaitOne(0, false)) {
                    MessageBox.Show("Instance already running");
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                AntumbraCore core = new AntumbraCore();
                core.Hide();
                if (core.goodStart)
                    Application.Run();
                else {//if start up failed close the app before it even starts
                    core.Close();
                    core.Dispose();
                }
            }
        }

        private static string appGuid = "5e20e0ce-ca88-4e48-b4da-a5de166f5a3d";
    }
}
