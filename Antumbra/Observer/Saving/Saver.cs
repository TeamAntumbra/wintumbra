using Antumbra.Glow.Observer.Logging;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Antumbra.Glow.Observer.Saving {

    public class Saver : Loggable {

        #region Private Fields

        private static readonly object sync = new Object();

        private static Saver instance;

        private string path;

        #endregion Private Fields

        #region Private Constructors

        private Saver() {
            AttachObserver(LoggerHelper.GetInstance());
            this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Antumbra\\";
            if(!System.IO.Directory.Exists(path)) {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        #endregion Private Constructors

        #region Public Delegates

        public delegate void NewLogMsg(String source, String msg);

        #endregion Public Delegates

        #region Public Events

        public event NewLogMsg NewLogMsgAvailEvent;

        #endregion Public Events

        #region Public Methods

        public static Saver GetInstance() {
            if(instance == null) {
                instance = new Saver();
            }

            return instance;
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public Object Load(String fileName) {
            lock(sync) {
                try {
                    using(Stream stream = File.Open(path + fileName, FileMode.Open, FileAccess.Read)) {
                        BinaryFormatter formatter = new BinaryFormatter();
                        var result = formatter.Deserialize(stream);
                        stream.Flush();
                        stream.Close();
                        return result;
                    }
                } catch(Exception ex) {
                    Log("Loading failed!");
                    Log(ex.Message + '\n' + ex.StackTrace);
                    return null;
                }
            }
        }

        public void Save(String fileName, ISerializable serializable) {
            lock(sync) {
                try {
                    var file = path + fileName;
                    if(File.Exists(file)) {
                        File.Delete(file);
                    }

                    using(Stream stream = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write)) {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, serializable);
                        stream.Flush();
                        stream.Close();
                    }
                } catch(Exception ex) {
                    Log("Saving failed!");
                    Log(ex.Message + '\n' + ex.StackTrace);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Log(String msg) {
            if(NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("Saver Singleton", msg);
            }
        }

        #endregion Private Methods
    }
}
