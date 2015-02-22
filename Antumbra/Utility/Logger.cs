using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Utility
{
    public class Logger
    {
        private string name;
        public Logger(string name)
        {
            this.name = name;
        }

        public void Log(String lines)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(name, true)) {
                file.WriteLine(lines);
            }
        }
    }
}
