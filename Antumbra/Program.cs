using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Antumbra.Glow.Controller;
using Antumbra.Glow.Observer.Logging;

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
            LoggerHelper.Logger logger = LoggerHelper.GetInstance();
            logger.NewLogMsgAvail("Program Class", "Starting...");
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid)) {
                if (!mutex.WaitOne(0, false)) {
                    MessageBox.Show("Instance already running");
                    logger.NewLogMsgAvail("Program Class", "MUTEX TAKEN...");
                    return;
                }
                if (Environment.OSVersion.Version.Major >= 6)
                    SetProcessDPIAware();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                logger.NewLogMsgAvail("Program Class", "Starting initialization of objects...");
                ToolbarIconController controller = new ToolbarIconController();
                if (!controller.failed) {//did setup succeed
                    logger.NewLogMsgAvail("Program Class", "Starting run...");
                    Application.Run();//start independent of form
                }
                else {//failed
                    controller.Dispose();
                    logger.NewLogMsgAvail("Program Class", "SETUP FAILED! FATAL");
                }
            }
        }

        private static string appGuid = "5e20e0ce-ca88-4e48-b4da-a5de166f5a3d";

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
