using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.Saving;
using Antumbra.Glow.Settings;
using System;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class ExtensionInstance : Loggable, LogMsgObserver, AntumbraColorSource, AntumbraColorObserver,
                                     ConfigurationObserver, GlowCommandObserver, GlowCommandSender,
                                     IDisposable, Savable
    {
        public const String SAVE_FILE_PREFIX = "ExtensionInstance_";
        public delegate void NewColor(Color16Bit newColor, int id, long index);
        public event NewColor NewColorAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;
        public delegate void NewGlowCommand(GlowCommand command);
        public event NewGlowCommand NewGlowCommandAvailEvent;
        public delegate void NewToolbarNotif(int time, String title, String msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;

        private long prevIndex;
        private ActiveExtensions Extensions;
        private int id;
        private const String FAILED_START_EXCEPTION_PREFIX = "Processor failed to start: ";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extensions"></param>
        public ExtensionInstance(int id, ActiveExtensions extensions)
        {
            this.id = id;
            this.AttachObserver(LoggerHelper.GetInstance());
            this.Extensions = extensions;
        }

        /// <summary>
        /// Start this instance
        /// </summary>
        /// <returns>Did it start as expected?</returns>
        public bool Start()
        {
            try {
                Extensions.ActiveDriver.AttachColorObserver(this);
                ObserveCmdsAndLog(Extensions.ActiveDriver);
                if (!Extensions.ActiveDriver.Start())
                    return false;
                foreach (var processor in Extensions.ActiveProcessors)
                    if (!processor.Start())
                        throw new Exception(FAILED_START_EXCEPTION_PREFIX + processor.ToString());
                foreach (var filter in Extensions.ActiveFilters)
                    if (!filter.Start())
                        throw new Exception(FAILED_START_EXCEPTION_PREFIX + filter.ToString());
                foreach (var notifier in Extensions.ActiveNotifiers)
                    if (!notifier.Start())
                        throw new Exception(FAILED_START_EXCEPTION_PREFIX + notifier.ToString());
            }
            catch (Exception ex) {
                Log("Exception starting ExtensionInstance " + id + ".\n" + ex.Message + '\n' + ex.StackTrace);
                Stop();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Stop this instance
        /// </summary>
        /// <returns>Did it stop as expected?</returns>
        public bool Stop()
        {
            try {
                bool result = true;
                if (Extensions != null) {
                    if (!Extensions.ActiveDriver.Stop())
                        result = false;
                    foreach (var filter in Extensions.ActiveFilters)
                        if (!filter.Stop())
                            result = false;
                    foreach (var notifier in Extensions.ActiveNotifiers)
                        if (!notifier.Stop())
                            result = false;
                }
                return result;
            }
            catch (Exception ex) {
                Log("Exception stopping ExtensionsInstance.\n" + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Save a serialized version of the ActiveExtensions object for this instance
        /// </summary>
        public void Save()
        {
            if (Extensions != null) {
                Saver saver = Saver.GetInstance();
                saver.Save(SAVE_FILE_PREFIX + id, Extensions);
            }
        }

        /// <summary>
        /// Load the Serialized ActiveExtensions info
        /// Note: The ExtensionInstance cannot be used until InitActives() is called
        /// </summary>
        public void Load()
        {
            Saver saver = Saver.GetInstance();
            Extensions = (ActiveExtensions)saver.Load(SAVE_FILE_PREFIX + id);
        }

        /// <summary>
        /// Initialize the Extensions
        /// </summary>
        /// <param name="lib"></param>
        public void InitActives(ExtensionLibrary lib)
        {
            Extensions.Init(lib);
        }

        /// <summary>
        /// Pass up any observed GlowCommands
        /// </summary>
        /// <param name="command"></param>
        public void NewGlowCommandAvail(GlowCommand command)
        {
            if (NewGlowCommandAvailEvent != null) {
                NewGlowCommandAvailEvent(command);
            }
        }

        /// <summary>
        /// Update Extensions when new Configuration is available
        /// </summary>
        /// <param name="config"></param>
        public void ConfigurationUpdate(Configurable config)
        {
            if (config is DeviceSettings) {
                DeviceSettings settings = (DeviceSettings)config;
                Extensions.ActiveDriver.stepSleep = settings.stepSleep;
                Extensions.ActiveDriver.weighted = settings.weightingEnabled;
                Extensions.ActiveGrabber.captureThrottle = settings.captureThrottle;
                Extensions.ActiveGrabber.x = settings.boundX;
                Extensions.ActiveGrabber.y = settings.boundY;
                Extensions.ActiveGrabber.width = settings.boundWidth;
                Extensions.ActiveGrabber.height = settings.boundHeight;
                foreach (var process in Extensions.ActiveProcessors)
                    if (settings.id == process.devId)
                        process.SetArea(settings.x, settings.y, settings.width, settings.height);
            }
        }

        /// <summary>
        /// Fake a color announcement with the expected values
        /// </summary>
        /// <param name="newColor">Color to send</param>
        public void FaslifyNewColorAvail(Color16Bit newColor)
        {
            if (NewColorAvailEvent != null) {
                NewColorAvailEvent(newColor, id, prevIndex + 1);
            }
        }

        /// <summary>
        /// Filter and announce a new NewColorAvailEvent
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="id"></param>
        /// <param name="index"></param>
        public void NewColorAvail(Color16Bit newColor, int id, long index)
        {
            if (NewColorAvailEvent != null) {
                long r = 0, g = 0, b = 0;
                int i;
                for (i = 0; i < Extensions.ActiveFilters.Count; i += 1) {
                    r += newColor.red;
                    g += newColor.green;
                    b += newColor.blue;
                }
                Color16Bit filtered = new Color16Bit(Convert.ToUInt16(r / i), Convert.ToUInt16(g / i), Convert.ToUInt16(b / i));
                prevIndex = index;
                NewColorAvailEvent(filtered, id, index);
            }
        }

        /// <summary>
        /// Announce any observed NewLogMsgAvail events for the logger
        /// </summary>
        /// <param name="source"></param>
        /// <param name="msg"></param>
        public void NewLogMsgAvail(String source, String msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent(source, msg);
        }

        /// <summary>
        /// Attach a GlowCommandObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(GlowCommandObserver observer)
        {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
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
        /// Attach an AntumbraColorObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        /// <summary>
        /// Dipose of Extensions
        /// </summary>
        public void Dispose()
        {
            Extensions.ActiveDriver.Dispose();
            foreach(var ext in Extensions.ActiveFilters) {
                ext.Dispose();
            }
            foreach (var ext in Extensions.ActiveNotifiers) {
                ext.Dispose();
            }
        }

        /// <summary>
        /// Attach this object as a LogMsgObserver and GlowCommandObserver if applicable
        /// </summary>
        /// <param name="extension"></param>
        private void ObserveCmdsAndLog(GlowExtension extension) {
            if (extension is Loggable) {
                Loggable log = (Loggable)extension;
                log.AttachObserver(this);
            }
            if (extension is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)extension;
                sender.AttachObserver(this);
            }
        }

        /// <summary>
        /// Log a message related to this object
        /// </summary>
        /// <param name="msg"></param>
        private void Log(String msg)
        {
            if (this.NewLogMsgAvailEvent != null)
                this.NewLogMsgAvailEvent("Extension Instance " + this.id, msg);
        }
    }
}
