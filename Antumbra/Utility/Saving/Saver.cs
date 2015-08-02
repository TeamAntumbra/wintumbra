﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.Utility.Saving
{
    public class Saver : Loggable
    {
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;
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
            AttachObserver(LoggerHelper.GetInstance());
            this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Antumbra\\";
            if (!System.IO.Directory.Exists(this.path))
                System.IO.Directory.CreateDirectory(this.path);
        }

        public void Save(String fileName, String serializedSettings)
        {
            lock (sync) {
                try {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.path + fileName, false)) {
                        file.WriteLine(serializedSettings);
                    }
                }
                catch (Exception ex) {
                    Log(ex.Message + '\n' + ex.StackTrace);
                }
            }
        }

        public String Load(String fileName)
        {
            lock (sync) {
                try {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(this.path + fileName, true)) {
                        String contents = file.ReadToEnd();
                        return contents;
                    }
                }
                catch (Exception ex) {
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
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("Saver Singleton", msg);
        }
    }
}
