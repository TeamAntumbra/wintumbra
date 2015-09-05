using Antumbra.Glow.Controller;
using Antumbra.Glow.Observer.Logging;
using System;
using System.Windows.Forms;

namespace Antumbra.Glow {

    internal static class Program {

        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            LoggerHelper.Logger logger = LoggerHelper.GetInstance();
            logger.NewLogMsgAvail("Program Class", "Starting...");
            if(Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            logger.NewLogMsgAvail("Program Class", "Starting initialization of objects...");
            ToolbarIconController controller = new ToolbarIconController();
            try {
                if(!controller.failed) {
                    logger.NewLogMsgAvail("Program Class", "Starting run...");
                    Application.Run();//start independent of form
                }
            } finally {
                if(controller != null) {
                    controller.Dispose();
                }
                Application.Exit();
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        #endregion Private Methods
    }
}
