using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
}
