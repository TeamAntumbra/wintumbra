using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.Observer.Saving
{
    public class Saver : Loggable
    {
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;

        private static readonly object sync = new Object();

        private string path;
        private static Saver instance;

        public static Saver GetInstance()
        {
            if (instance == null) {
                instance = new Saver();
            }

            return instance;
        }
        private Saver()
        {
            AttachObserver(LoggerHelper.GetInstance());
            this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Antumbra\\";
            if (!System.IO.Directory.Exists(path)) {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public void Save(String fileName, ISerializable serializable)
        {
            lock (sync) {
                try {
                    using (Stream stream = File.Open(path + fileName, FileMode.Create)) {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, serializable);
                    }
                }
                catch (Exception ex) {
                    Log("Saving failed!");
                    Log(ex.Message + '\n' + ex.StackTrace);
                }
            }
        }

        public Object Load(String fileName)
        {
            lock (sync) {
                try {
                    using (Stream stream = File.Open(path + fileName, FileMode.Open)) {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return formatter.Deserialize(stream);
                    }
                }
                catch (Exception ex) {
                    Log("Loading failed!");
                    Log(ex.Message + '\n' + ex.StackTrace);
                    return null;
                }
            }
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        private void Log(String msg)
        {
            if (NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("Saver Singleton", msg);
            }
        }
    }
}
