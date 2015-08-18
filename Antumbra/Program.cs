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
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            logger.NewLogMsgAvail("Program Class", "Starting initialization of objects...");
            ToolbarIconController controller = new ToolbarIconController();
            try {
                logger.NewLogMsgAvail("Program Class", "Starting run...");
                Application.Run();//start independent of form
            }
            catch (Exception ex) {
                logger.NewLogMsgAvail("Program Class", ex.Message + '\n' + ex.StackTrace);
            }
            finally {
                if (controller != null) {
                    controller.Dispose();
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
