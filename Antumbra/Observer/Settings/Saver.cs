using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Settings
{
    public class Saver : SavableObserver
    {
        private object sync = new Object();
        private string path;
        private string name;
        public Saver(String name)
        {
            this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Antumbra\\";
            if (!System.IO.Directory.Exists(this.path))
                System.IO.Directory.CreateDirectory(this.path);
            this.name = name;
        }

        public void NewSettingsUpdate(Guid id, String settings)
        {
            Save(id, settings);
        }

        private void Save(Guid id, String serializedSettings)
        {
            lock (sync) {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.path + name, true)) {
                    file.WriteLine(id.ToString() + "-" + serializedSettings);
                }
            }
        }

        public String Load(Guid id)
        {
            lock (sync) {
                using (System.IO.StreamReader file = new System.IO.StreamReader(this.path + this.name, true)) {
                    String line = file.ReadLine();
                    if (line.StartsWith(id.ToString()))
                        return line;
                }
            }
            return null;
        }
    }
}
