using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Logging
{
    /// <summary>
    /// Allows easy logging to a specified file
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Name of the log file for this Logger
        /// </summary>
        private string name;
        /// <summary>
        /// Constructor - Create a new logger with the logfile at path as passed
        /// </summary>
        /// <param name="name"></param>
        public Logger(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// Write the passed lines to the log file
        /// </summary>
        /// <param name="lines"></param>
        public void Log(String lines)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(name, true)) {
                file.WriteLine(lines);
            }
        }
    }
}
