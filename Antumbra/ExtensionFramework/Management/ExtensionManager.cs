using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antumbra.Glow.ExtensionFramework.Management {

    /// <summary>
    /// Manages the Extensions for use with a Glow device
    /// </summary>
    public class ExtensionManager : AntumbraColorObserver, AntumbraColorSource, LogMsgObserver, Loggable,
                                    GlowCommandObserver, ConnectionEventObserver, ConfigurationObserver//TODO add observer for notifiers
    {
        #region Private Fields

        private ExtensionInstance CaptureInstance;

        private int deviceCount;

        private List<ExtensionInstance> Instances;

        private ExtensionLibrary Lib;

        private PresetBuilder PresetBuilder;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor - Creates a new ExtensionManager
        /// </summary>
        /// <param name="path"></param>
        /// <param name="settings"></param>
        public ExtensionManager() {
            deviceCount = 0;
            Lib = new ExtensionLibrary();
            Instances = new List<ExtensionInstance>();
            CaptureInstance = null;
            try {
                Lib.Update();
                PresetBuilder = new Management.PresetBuilder(Lib);
            } catch(Exception) {
                throw;
            }
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewColor(Color16Bit newColor, int id, long index);

        public delegate void NewLogMsg(String source, String msg);

        #endregion Public Delegates

        #region Public Events

        public event NewColor NewColorAvailEvent;

        public event NewLogMsg NewLogMsgAvailEvent;

        #endregion Public Events

        #region Public Enums

        public enum MODE {
            EMPTY = -1,
            HSV = 0,
            SIN = 1,
            NEON = 2,
            MIRROR = 3,
            AUGMENT = 4,
            LOAD = 5
        }

        #endregion Public Enums

        #region Public Methods

        /// <summary>
        /// Attach a color observer
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        /// <summary>
        /// Attach a LogMsgObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        /// <summary>
        /// Route the ConfigurationUpdate to the correct ExtensionInstance
        /// </summary>
        /// <param name="config"></param>
        public void ConfigurationUpdate(Configurable config) {
            foreach(ExtensionInstance Instance in Instances) {
                Instance.ConfigurationUpdate(config);
            }
        }

        /// <summary>
        /// Handle a ConnectionUpdate event
        /// </summary>
        /// <param name="devCount"></param>
        public void ConnectionUpdate(int devCount) {
            deviceCount = devCount;
            // Different than the current number of instances
            if(devCount != Instances.Count) {
                // Save and dispose each of the current instances
                foreach(ExtensionInstance instance in Instances) {
                    instance.Save();
                    instance.Dispose();
                }
                Instances.Clear();
                //Create, Load & Init new ExtensionInstances
                for(int id = 0; id < devCount; id += 1) {
                    ExtensionInstance instance = CreateInstance(id, MODE.LOAD);
                    try {
                        instance.Load();
                        instance.InitActives(Lib);
                    } catch(System.IO.FileNotFoundException) {
                        Log("ExtensionInstance (id: " + id + ") loading failed (FileNotFound). Using EMPTY preset.");
                    } finally {
                        Instances.Add(instance);
                    }
                }
            }
        }

        /// <summary>
        /// Create an ExtensionInstance for the specified id and mode
        /// </summary>
        /// <param name="id"></param>
        /// <param name="preset"></param>
        /// <returns></returns>
        public ExtensionInstance CreateInstance(int id, MODE preset) {
            ActiveExtensions actives;
            switch(preset) {
                case MODE.HSV:
                    actives = PresetBuilder.GetHSVFadePreset();
                    break;

                case MODE.SIN:
                    actives = PresetBuilder.GetSinFadePreset();
                    break;

                case MODE.NEON:
                    actives = PresetBuilder.GetNeonFadePreset();
                    break;

                case MODE.MIRROR://Both use same extensions, just different settings
                    actives = PresetBuilder.GetMirrorPreset();
                    break;

                case MODE.AUGMENT:
                    actives = PresetBuilder.GetAugmentMirrorPreset();
                    break;

                case MODE.LOAD:
                    actives = null;
                    ExtensionInstance inst = new ExtensionInstance(id, new ActiveExtensions());
                    inst.Load();
                    inst.AttachObserver((AntumbraColorObserver)this);
                    return inst;

                case MODE.EMPTY:
                default:
                    actives = new ActiveExtensions();
                    break;
            }

            ExtensionInstance instance = new ExtensionInstance(id, actives);
            instance.AttachObserver((AntumbraColorObserver)this);
            return instance;
        }

        public string getOutRatesMessage() {
            StringBuilder sb = new StringBuilder();
            foreach(ExtensionInstance instance in Instances) {
                sb.Append(instance.id)
                  .Append(" @ ")
                  .Append(instance.GetOutputRate())
                  .Append(" FPS.\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Load all ExtensionInstances
        /// </summary>
        public void LoadAllInstances() {
            for(int i = 0; i < Instances.Count; i += 1) {
                Instances[i].Load();
            }
        }

        /// <summary>
        /// Handle a NewColorAvail event
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="id"></param>
        /// <param name="index"></param>
        public void NewColorAvail(Color16Bit newColor, int id, long index) {
            if(NewColorAvailEvent != null) {
                try {
                    NewColorAvailEvent(newColor, id, index);
                } catch(ArgumentException ex) {
                    Log(ex.Message + '\n' + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Handle a NewGlowCommandAvail event
        /// </summary>
        /// <param name="command"></param>
        public void NewGlowCommandAvail(GlowCommand command) {
            command.ExecuteCommand(this);
        }

        /// <summary>
        /// Handler a NewLogMsgAvail event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="msg"></param>
        public void NewLogMsgAvail(String source, String msg) {
            if(NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent(source, msg);
        }

        /// <summary>
        /// Turn off the specified device, -1 for all
        /// </summary>
        /// <param name="id"></param>
        public void Off(int id) {
            if(id == -1) {
                // Stop & send black NewColorAvail call for each instance
                for(var i = 0; i < Instances.Count; i += 1) {
                    Off(i);
                }
                return;
            }

            SaveInstance(id);
            StopAndSendColor(new Color16Bit(), id);
        }

        /// <summary>
        /// Reset the ExtensionInstance objects
        /// </summary>
        public void Reset() {
            // dispose and create stock ones
        }

        /// <summary>
        /// Save all ExtensionInstances
        /// </summary>
        public void SaveAllInstances() {
            for(int i = 0; i < Instances.Count; i += 1) {
                SaveInstance(i);
            }
        }

        /// <summary>
        /// Save a certain ExtensionInstance
        /// </summary>
        /// <param name="id"></param>
        public void SaveInstance(int id) {
            try {
                Instances[id].Save();
            } catch(IndexOutOfRangeException) {
                Log("Saving instance (id: " + id + ") failed. Index out of range.");
            }
        }

        public void SetInstance(int id, MODE mode) {
            if(id == -1) {
                for(int i = 0; i < deviceCount; i += 1) {
                    SetInstance(i, mode);
                }
                return;
            }

            try {
                Stop(id);
                Instances[id].Dispose();
            } catch(KeyNotFoundException) {
                Log("Key " + id + " not found...Creating Instance...");
            } finally {
                Instances[id] = CreateInstance(id, mode);
            }
        }

        public void SoftSendColor(Color16Bit newColor, int id) {
            if(id == -1) {
                for(int i = 0; i < Instances.Count; i += 1) {
                    SoftSendColor(newColor, id);
                }
                return;
            }

            ExtensionInstance Instance = Instances[id];
            if(!Instance.running) {
                if(NewColorAvailEvent != null) {
                    NewColorAvailEvent(newColor, id, Instance.prevIndex);
                }
            }
        }

        public void Start(int id) {
            if(id == -1) {
                for(var i = 0; i < Instances.Count; i += 1) {
                    Start(i);
                }
                return;
            }

            Instances[id].InitActives(Lib);

            Guid grabber = Instances[id].GetGrabber();
            if(!grabber.Equals(Guid.Empty)) {
                if(CaptureInstance == null) {
                    CaptureInstance = Instances[id];
                    CaptureInstance.Start();
                } else {
                    CaptureInstance.AddScreenProcessorsFromInstance(Instances[id]);
                }
            } else {
                Instances[id].Start();
            }
        }

        public void Stop(int id) {
            if(id == -1) {
                for(var i = 0; i < Instances.Count; i += 1) {
                    Stop(i);
                }
                return;
            }

            if(Instances[id].Equals(CaptureInstance)) {
                CaptureInstance = null;
            }

            Instances[id].Stop();
        }

        public void StopAndSendColor(Color16Bit newColor, int id) {
            if(id == -1) {
                for(int i = 0; i < Instances.Count; i += 1) {
                    StopAndSendColor(newColor, i);
                }
                return;
            }

            ExtensionInstance Instance = Instances[id];
            Instance.Stop();
            if(NewColorAvailEvent != null) {
                NewColorAvailEvent(newColor, id, Instance.prevIndex);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Log an ExtensionManager related message
        /// </summary>
        /// <param name="msg"></param>
        private void Log(String msg) {
            if(NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("ExtensionManager", msg);
            }
        }

        #endregion Private Methods
    }
}
