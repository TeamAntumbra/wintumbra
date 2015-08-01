using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility.Settings;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.ToolbarNotifications;

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
        public delegate void NewColor(Color16Bit newColor, int id);
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
        private DeviceSettings Settings;
        private ActiveExtensions Extensions;
        private int id;
        private static readonly String FAILED_START_EXCEPTION_PREFIX = "Processor failed to start: ";
        public ExtensionInstance(int id, ActiveExtensions extensions, DeviceSettings settings)
        {
            this.id = id;
            this.Settings = settings;
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
                Log("Exception starting ExtensionsInstance " + id + ".\n" + ex.Message + '\n' + ex.StackTrace);
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
                saver.Save(ExtensionManager.configFileBase + this.id, this.Extensions.ToString());
            }
        }

        public void NewGlowCommandAvail(GlowCommand command)
        {

        }

        public void ConfigurationUpdate(Configurable config)
        {
            // Do nothing, has direct Setting object access
        }

        public void NewColorAvail(Color16Bit newColor, int id)
        {
            if (NewColorAvailEvent != null)
                NewColorAvailEvent(newColor, id);
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
