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
    public class Logger
    {
        public object sync = new object();
        /// <summary>
        /// Name of the log file for this Logger
        /// </summary>
        private string name;
        /// <summary>
        /// Constructor - Create a new logger with the logfile at path as passed
        /// </summary>
        /// <param name="name"></param>
        private string path;
        public Logger(string name)
        {
            this.name = name;
            this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Antumbra\\";
            if (!System.IO.Directory.Exists(this.path))
                System.IO.Directory.CreateDirectory(this.path);
        }
        /// <summary>
        /// Write the passed lines to the log file
        /// </summary>
        /// <param name="lines"></param>
        public void Log(String lines)
        {
            lock (sync) {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.path + name, false)) {
                    file.WriteLine(lines);
                }
            }
        }
    }
}
