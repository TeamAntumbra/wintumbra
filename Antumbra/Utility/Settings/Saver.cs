using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Utility.Settings
{
    public class Saver
    {
        private object sync = new Object();
        private string path;
        private static Saver instance;

        public static Saver GetInstance()
        {
            if (instance == null)
                instance = new Saver();
            return instance;
        }
        private Saver()
        {
            this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Antumbra\\";
            if (!System.IO.Directory.Exists(this.path))
                System.IO.Directory.CreateDirectory(this.path);
        }

        public void Save(String id, String serializedSettings)
        {
            lock (sync) {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.path + id, false)) {
                    file.WriteLine(serializedSettings);
                }
            }
        }

        public String Load(String id)
        {
            lock (sync) {
                using (System.IO.StreamReader file = new System.IO.StreamReader(this.path + id.ToString(), true)) {
                    String contents = file.ReadToEnd();
                    return contents;
                }
            }
        }
    }
}
