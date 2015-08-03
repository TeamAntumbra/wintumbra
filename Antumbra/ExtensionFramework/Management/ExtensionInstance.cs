using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility.Saving;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.ExtensionFramework.Types;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class ExtensionInstance : Loggable, LogMsgObserver, AntumbraColorSource, AntumbraColorObserver,
                                     ConfigurationObserver, GlowCommandObserver, GlowCommandSender,
                                     ToolbarNotificationObserver, ToolbarNotificationSource
    {
        /// <summary>
        /// Delegate for NewColorAvailEvent, handles a new Color being available
        /// </summary>
        /// <param name="newColor">The new color to send</param>
        /// <param name="id">The device id the color pertains to</param>
        /// <param name="index">Index given to this color to ensure correct ordering of output</param>
        public delegate void NewColor(Color16Bit newColor, int id, long index);
        /// <summary>
        /// NewColorAvail Event, occurs when a new color is available
        /// </summary>
        public event NewColor NewColorAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;
        public delegate void NewGlowCommand(GlowCommand command);
        public event NewGlowCommand NewGlowCommandAvailEvent;
        public delegate void NewToolbarNotif(int time, String title, String msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        private ActiveExtensions Extensions;
        private int id;
        private static readonly String FAILED_START_EXCEPTION_PREFIX = "Processor failed to start: ";
        private static readonly String SAVE_FILE_PREFIX = "ExtensionInstance_";
        public ExtensionInstance(int id, ActiveExtensions extensions)
        {
            this.id = id;
            this.AttachObserver(LoggerHelper.GetInstance());
            this.Extensions = extensions;
        }

        public bool Start()
        {
            try {
                Extensions.ActiveDriver.AttachColorObserver(this);
                ObserveAll(Extensions.ActiveDriver);
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

        public void Save()
        {
            if (Extensions != null) {
                Saver saver = Saver.GetInstance();
                saver.Save(SAVE_FILE_PREFIX + this.id, this.Extensions.ToString());
            }
        }

        public void NewGlowCommandAvail(GlowCommand command)
        {

        }

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
                NewColorAvailEvent(filtered, id, index);
            }
        }

        public void NewLogMsgAvail(String source, String msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent(source, msg);
        }

        public void NewToolbarNotifAvail(int time, String title, String msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        private void ObserveAll(GlowExtension extension) {
            if (extension is Loggable) {
                Loggable log = (Loggable)extension;
                log.AttachObserver(this);
            }
            if (extension is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)extension;
                sender.AttachObserver(this);
            }
            if (extension is ToolbarNotificationSource) {
                ToolbarNotificationSource src = (ToolbarNotificationSource)extension;
                src.AttachObserver(this);
            }
        }

        private void Log(String msg)
        {
            if (this.NewLogMsgAvailEvent != null)
                this.NewLogMsgAvailEvent("Extension Instance " + this.id, msg);
        }
    }
}
