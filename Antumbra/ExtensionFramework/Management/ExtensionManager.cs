using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Settings;
using System;
using System.Collections.Generic;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    /// <summary>
    /// Manages the Extensions for use with a Glow device
    /// </summary>
    public class ExtensionManager : AntumbraColorObserver, LogMsgObserver, Loggable,
                                    GlowCommandObserver, ConnectionEventObserver//TODO add observer for notifiers
    {
        public enum MODE
        {
            EMPTY = -1,
            HSV = 0,
            SIN = 1,
            NEON = 2,
            MIRROR = 3,
            SMOOTH = 4,
            AUGMENT = 5,
            GAME = 6
        }

        public delegate void NewColor(Color16Bit newColor, int id, long index);
        public event NewColor NewColorAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;
        public delegate void NewToolbarNotif(int time, String title, String msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommand(GlowCommand command);
        public event NewGlowCommand NewGlowCommandEvent;

        private ExtensionLibrary Lib;
        private Dictionary<int, ExtensionInstance> Instances;
        private PresetBuilder PresetBuilder;
        /// <summary>
        /// Constructor - Creates a new ExtensionManager
        /// </summary>
        /// <param name="path"></param>
        /// <param name="settings"></param>
        public ExtensionManager(ExtensionLibrary extLib)
        {
            Instances = new Dictionary<int, ExtensionInstance>();
            Lib = extLib;
        }

        public void SetInstance(int id, MODE mode)
        {
            Instances[id].Stop();
            Instances[id].Dispose();
            Instances[id] = CreateInstance(id, mode);
        }

        /// <summary>
        /// Save all ExtensionInstances
        /// </summary>
        public void SaveAllInstances()
        {
            for (int i = 0; i < Instances.Count; i += 1) {
                Instances[i].Save();
            }
        }

        /// <summary>
        /// Load all ExtensionInstances
        /// </summary>
        public void LoadAllInstances()
        {
            for (int i = 0; i < Instances.Count; i += 1) {
                Instances[i].Load();
            }
        }

        /// <summary>
        /// Save a certain ExtensionInstance
        /// </summary>
        /// <param name="id"></param>
        public void SaveInstance(int id)
        {
            try {
                Instances[id].Save();
            }
            catch(IndexOutOfRangeException) {
                Log("Saving instance (id: " + id + ") failed. Index out of range.");
            }
        }

        /// <summary>
        /// Create an ExtensionInstance for the specified id and mode
        /// </summary>
        /// <param name="id"></param>
        /// <param name="preset"></param>
        /// <returns></returns>
        public ExtensionInstance CreateInstance(int id, MODE preset)
        {
            if (Instances.ContainsKey(id)) {
                throw new Exception("Instance already exists! Id: " + id);
            }
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
                case MODE.MIRROR:
                    actives = PresetBuilder.GetMirrorPreset();
                    break;
                case MODE.SMOOTH:
                    actives = PresetBuilder.GetSmoothMirrorPreset();
                    break;
                case MODE.AUGMENT:
                    actives = PresetBuilder.GetAugmentMirrorPreset();
                    break;
                case MODE.GAME:
                    actives = PresetBuilder.GetGameMirrorPreset();
                    break;
                case MODE.EMPTY:
                default:
                    actives = new ActiveExtensions();
                    break;
            }
            return new ExtensionInstance(id, actives);
        }

        /// <summary>
        /// Handle a ConnectionUpdate event
        /// </summary>
        /// <param name="devCount"></param>
        public void ConnectionUpdate(int devCount)
        {
            // Different than the current number of instances
            if (devCount != Instances.Count) {
                // Save and dispose each of the current instances
                foreach(ExtensionInstance instance in Instances.Values) {
                    instance.Save();
                    instance.Dispose();
                }
                Instances.Clear();
                //Create, Load & Init new ExtensionInstances
                for (int id = 0; id < devCount; id += 1) {
                    ExtensionInstance instance = CreateInstance(id, MODE.EMPTY);
                    try {
                        instance.Load();
                        instance.InitActives(Lib);
                    }
                    catch (System.IO.FileNotFoundException) {
                        Log("ExtensionInstance (id: " + id + ") loading failed (FileNotFound). Using EMPTY preset.");
                    }
                    finally {
                        Instances.Add(id, instance);
                    }
                }
            }
        }

        /// <summary>
        /// Handle a NewColorAvail event
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="id"></param>
        /// <param name="index"></param>
        public void NewColorAvail(Color16Bit newColor, int id, long index)
        {
            if (NewColorAvailEvent != null) {
                NewColorAvailEvent(newColor, id, index);
            }
        }

        /// <summary>
        /// Reset the ExtensionInstance objects
        /// </summary>
        public void Reset()
        {
            // dispose and create stock ones
        }

        /// <summary>
        /// Attach a LogMsgObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        /// <summary>
        /// Handler a NewLogMsgAvail event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="msg"></param>
        public void NewLogMsgAvail(String source, String msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent(source, msg);
        }

        /// <summary>
        /// Handle a NewGlowCommandAvail event
        /// </summary>
        /// <param name="command"></param>
        public void NewGlowCommandAvail(GlowCommand command)
        {
            command.ExecuteCommand(this);
        }

        /// <summary>
        /// Turn off the specified device, -1 for all
        /// </summary>
        /// <param name="id"></param>
        internal void Off(int id)
        {
            if (id == -1) {
                // Stop & send black NewColorAvail call for each instance
                for (var i = 0; i < Instances.Count; i += 1) {
                    Off(i);
                }
                return;
            }

            ExtensionInstance instance = Instances[id];
            instance.Stop();
            instance.FaslifyNewColorAvail(new Color16Bit());
        }

        internal void SendColor(Color16Bit newColor, int id)
        {
            if (id == -1) {
                for (int i = 0; i < Instances.Count; i += 1) {
                    SendColor(i, newColor);
                }
                return;
            }

            Instances[id].FaslifyNewColorAvail(newColor);
        }

        internal void Start(int id)
        {
            if (id == -1) {
                for (var i = 0; i < Instances.Count; i += 1) {
                    Start(id);
                }
                return;
            }

            Instances[id].Start();
        }

        internal void Stop(int id)
        {
            if (id == -1) {
                for (var i = 0; i < Instances.Count; i += 1) {
                    Stop(i);
                }
                return;
            }

            Instances[id].Stop();
        }

        /// <summary>
        /// Log an ExtensionManager related message
        /// </summary>
        /// <param name="msg"></param>
        private void Log(String msg)
        {
            if (NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("ExtensionManager", msg);
            }
        }
    }
}
