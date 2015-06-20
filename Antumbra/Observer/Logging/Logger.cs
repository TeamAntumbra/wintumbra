using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Logging
{
    /// <summary>
    /// Allows easy logging to a specified file
    /// </summary>
    public static class LoggerHelper
    {
        public class Logger : LogMsgObserver {
            private object sync = new object();
            /// <summary>
            /// Name of the log file for this Logger
            /// </summary>
            private string name;
            private string path;
            internal Logger(string name) {
                this.name = name;
                this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\Antumbra\\";
                if (!System.IO.Directory.Exists(this.path))
                    System.IO.Directory.CreateDirectory(this.path);
            }

            public void NewLogMsgAvail(String source, String msg)
            {
                this.Log(source + " - " + msg);
            }

            /// <summary>
            /// Write the passed lines to the log file
            /// </summary>
            /// <param name="lines"></param>
            private void Log(String lines)
            {
                lock (sync) {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.path + name, true)) {
                        file.WriteLine(lines);
                    }
                }
            }
        }

        private static Logger instance;

        public static Logger GetInstance()
        {
            if (instance == null)
                instance = new Logger("wintumbra.log");
            return instance;
        }
    }
}
