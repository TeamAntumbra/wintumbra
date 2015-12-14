using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.Saving;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Utility;
using System;

namespace Antumbra.Glow.ExtensionFramework.Management {

    public class ExtensionInstance : Loggable, LogMsgObserver, AntumbraColorSource, AntumbraColorObserver,
                                     ConfigurationObserver, GlowCommandObserver, GlowCommandSender,
                                     IDisposable, Savable {

        #region Public Fields

        public const String SAVE_FILE_PREFIX = "ExtensionInstance_";

        public int id;

        #endregion Public Fields

        #region Private Fields

        private const String FAILED_START_EXCEPTION_PREFIX = "Processor failed to start: ";

        private long _prevIndex;

        private ActiveExtensions Extensions;

        private FPSCalc FPSCalculator;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extensions"></param>
        public ExtensionInstance(int id, ActiveExtensions extensions) {
            FPSCalculator = new FPSCalc();
            this.id = id;
            AttachObserver(LoggerHelper.GetInstance());
            Extensions = extensions;
            UpdateExtensionsDevId();
            Extensions.Ready();
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewColor(Color16Bit newColor, int id, long index);

        public delegate void NewGlowCommand(GlowCommand command);

        public delegate void NewLogMsg(String source, String msg);

        #endregion Public Delegates

        #region Public Events

        public event NewColor NewColorAvailEvent;

        public event NewGlowCommand NewGlowCommandAvailEvent;

        public event NewLogMsg NewLogMsgAvailEvent;

        #endregion Public Events

        #region Public Properties

        public long prevIndex {
            get {
                return _prevIndex++;
            }
            private set {
                _prevIndex = value;
            }
        }

        public bool running { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void AddScreenProcessorsFromInstance(ExtensionInstance InstanceToMerge) {
            foreach(var mappings in InstanceToMerge.Extensions.ActiveProcessors[0].GetMappings()) {
                Extensions.ActiveProcessors[0].SetArea(mappings.Value.X, mappings.Value.Y, mappings.Value.Width, mappings.Value.Height, mappings.Key);
            }
        }

        /// <summary>
        /// Attach a GlowCommandObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(GlowCommandObserver observer) {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        /// <summary>
        /// Attach a LogMsgObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        /// <summary>
        /// Attach an AntumbraColorObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        /// <summary>
        /// Update Extensions when new Configuration is available
        /// </summary>
        /// <param name="config"></param>
        public void ConfigurationUpdate(Configurable config) {
            if(config is DeviceSettings && Extensions != null) {
                DeviceSettings settings = (DeviceSettings)config;
                if(id != settings.id) {
                    return;
                }

                bool wasRunning = running;
                if(wasRunning) {
                    Stop();
                }

                if(Extensions.ActiveDriver != null) {
                    Extensions.ActiveDriver.stepSleep = settings.stepSleep;
                    Extensions.ActiveDriver.weighted = settings.weightingEnabled;
                }

                if(Extensions.ActiveGrabber != null) {
                    Extensions.ActiveGrabber.captureThrottle = settings.captureThrottle;
                    Extensions.ActiveGrabber.x = settings.boundX;
                    Extensions.ActiveGrabber.y = settings.boundY;
                    Extensions.ActiveGrabber.width = settings.boundWidth;
                    Extensions.ActiveGrabber.height = settings.boundHeight;
                }

                foreach(var process in Extensions.ActiveProcessors) {
                    if(settings.id == process.devId) {
                        process.SetArea(settings.x, settings.y, settings.width, settings.height, settings.id);
                    }
                }

                if(wasRunning) {
                    Start();
                }
            }
        }

        /// <summary>
        /// Dipose of Extensions
        /// </summary>
        public void Dispose() {
            if(Extensions != null) {
                if(Extensions.ActiveDriver != null) {
                    Extensions.ActiveDriver.Dispose();
                }
                if(Extensions.ActiveFilters != null) {
                    foreach(var ext in Extensions.ActiveFilters) {
                        ext.Dispose();
                    }
                }
                if(Extensions.ActiveNotifiers != null) {
                    foreach(var ext in Extensions.ActiveNotifiers) {
                        ext.Dispose();
                    }
                }
            }
        }

        public Guid GetGrabber() {
            if(Extensions != null && Extensions.ActiveGrabber != null) {
                return Extensions.ActiveGrabber.id;
            }

            return Guid.Empty;
        }

        public int GetOutputRate() {
            return Convert.ToInt32(FPSCalculator.FPS);
        }

        /// <summary>
        /// Initialize the Extensions
        /// </summary>
        /// <param name="lib"></param>
        public void InitActives(ExtensionLibrary lib) {
            if(Extensions != null) {
                Extensions.Init(lib);
                UpdateExtensionsDevId();
            }
        }

        /// <summary>
        /// Load the Serialized ActiveExtensions info
        /// Note: The ExtensionInstance cannot be used until InitActives() is called
        /// </summary>
        public void Load() {
            Stop();
            Dispose();
            Saver saver = Saver.GetInstance();
            Extensions = (ActiveExtensions)saver.Load(SAVE_FILE_PREFIX + id);
        }

        /// <summary>
        /// Filter and announce a new NewColorAvailEvent
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="id"></param>
        /// <param name="index"></param>
        public void NewColorAvail(Color16Bit newColor, int id, long index) {
            _prevIndex = index;
            if(NewColorAvailEvent != null) {
                if(Extensions.ActiveFilters.Count > 0) {
                    long r = 0, g = 0, b = 0;
                    int i;
                    for(i = 0; i < Extensions.ActiveFilters.Count; i += 1) {
                        newColor = Extensions.ActiveFilters[i].Filter(newColor);
                        r += newColor.red;
                        g += newColor.green;
                        b += newColor.blue;
                    }
                    newColor.red = Convert.ToUInt16(r / i);
                    newColor.green = Convert.ToUInt16(g / i);
                    newColor.blue = Convert.ToUInt16(b / i);
                }

                NewColorAvailEvent(newColor, id, index);
                FPSCalculator.Tick();
            }
        }

        /// <summary>
        /// Pass up any observed GlowCommands
        /// </summary>
        /// <param name="command"></param>
        public void NewGlowCommandAvail(GlowCommand command) {
            if(NewGlowCommandAvailEvent != null) {
                NewGlowCommandAvailEvent(command);
            }
        }

        /// <summary>
        /// Announce any observed NewLogMsgAvail events for the logger
        /// </summary>
        /// <param name="source"></param>
        /// <param name="msg"></param>
        public void NewLogMsgAvail(String source, String msg) {
            if(NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent(source, msg);
        }

        /// <summary>
        /// Save a serialized version of the ActiveExtensions object for this instance
        /// </summary>
        public void Save() {
            if(Extensions != null) {
                Saver saver = Saver.GetInstance();
                saver.Save(SAVE_FILE_PREFIX + id, Extensions);
            }
        }

        /// <summary>
        /// Start this instance
        /// </summary>
        /// <returns>Did it start as expected?</returns>
        public bool Start() {
            try {
                Log("Starting...");
                Extensions.Ready();
                Extensions.ActiveDriver.AttachColorObserver(this);
                ObserveCmdsAndLog(Extensions.ActiveDriver);
                if(!Extensions.ActiveDriver.Start())
                    return false;
                foreach(var filter in Extensions.ActiveFilters)
                    if(!filter.Start())
                        throw new Exception(FAILED_START_EXCEPTION_PREFIX + filter.ToString());
                foreach(var notifier in Extensions.ActiveNotifiers)
                    if(!notifier.Start())
                        throw new Exception(FAILED_START_EXCEPTION_PREFIX + notifier.ToString());
            } catch(Exception ex) {
                Log("Exception starting ExtensionInstance " + id + ".\n" + ex.Message + '\n' + ex.StackTrace);
                Stop();
                return false;
            }
            running = true;
            return true;
        }

        /// <summary>
        /// Stop this instance
        /// </summary>
        /// <returns>Did it stop as expected?</returns>
        public bool Stop() {
            if(!running) {
                return false;
            }

            try {
                Log("Stopping...");
                bool result = true;
                prevIndex = long.MinValue;
                if(Extensions != null) {
                    if(!Extensions.ActiveDriver.Stop())
                        result = false;
                    foreach(var filter in Extensions.ActiveFilters)
                        if(!filter.Stop())
                            result = false;
                    foreach(var notifier in Extensions.ActiveNotifiers)
                        if(!notifier.Stop())
                            result = false;
                }
                running = false;
                return result;
            } catch(Exception ex) {
                Log("Exception stopping ExtensionsInstance.\n" + ex.StackTrace);
                return false;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Log a message related to this object
        /// </summary>
        /// <param name="msg"></param>
        private void Log(String msg) {
            if(this.NewLogMsgAvailEvent != null)
                this.NewLogMsgAvailEvent("Extension Instance " + this.id, msg);
        }

        /// <summary>
        /// Attach this object as a LogMsgObserver and GlowCommandObserver if applicable
        /// </summary>
        /// <param name="extension"></param>
        private void ObserveCmdsAndLog(GlowExtension extension) {
            if(extension is Loggable) {
                Loggable log = (Loggable)extension;
                log.AttachObserver(this);
            }
            if(extension is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)extension;
                sender.AttachObserver(this);
            }
        }

        private void UpdateExtensionsDevId() {
            if(Extensions != null) {
                if(Extensions.ActiveDriver != null) {
                    Extensions.ActiveDriver.devId = id;
                }
                if(Extensions.ActiveGrabber != null) {
                    Extensions.ActiveGrabber.devId = id;
                }
                foreach(var extension in Extensions.ActiveProcessors) {
                    extension.devId = id;
                }
                foreach(var extension in Extensions.ActiveFilters) {
                    extension.devId = id;
                }
                foreach(var extension in Extensions.ActiveNotifiers) {
                    extension.devId = id;
                }
            }
        }

        #endregion Private Methods
    }
}
