using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Logging {
    /// <summary>
    /// Allows easy logging to a specified file
    /// </summary>
    public static class LoggerHelper {
        public class Logger : LogMsgObserver {
            /// <summary>
            /// Sync object
            /// </summary>
            private static readonly object sync = new Object();
            /// <summary>
            /// Name of the log file
            /// </summary>
            private string filename;
            /// <summary>
            /// Path to the log file
            /// </summary>
            private string path;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="filename">log file filename</param>
            public Logger(string filename) {
                this.filename = filename;
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Antumbra\\";
                if(!System.IO.Directory.Exists(path)) {
                    System.IO.Directory.CreateDirectory(path);
                }
            }

            /// <summary>
            /// Log the recieved information
            /// </summary>
            /// <param name="source"></param>
            /// <param name="msg"></param>
            public void NewLogMsgAvail(string source, string msg) {
                StringBuilder sb = new StringBuilder(source);
                sb.Append("\t-");
                foreach(string line in msg.Split('\n')) {
                    sb.Append('\t').Append(line);
                }

                Log(sb.ToString());
            }

            /// <summary>
            /// Write the passed lines to the log file
            /// </summary>
            /// <param name="lines"></param>
            private void Log(String lines) {
                lock(sync) {
                    Console.WriteLine(lines);
                    using(System.IO.StreamWriter file = new System.IO.StreamWriter(path + filename, true)) {
                        file.WriteLine(lines);
                    }
                }
            }
        }

        private static Logger instance;

        public static Logger GetInstance() {
            if(instance == null) {
                instance = new Logger("wintumbra.log");
            }
            return instance;
        }
    }
}
