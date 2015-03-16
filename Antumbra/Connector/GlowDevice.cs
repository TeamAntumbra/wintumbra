using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.Settings;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Utility.Settings;

namespace Antumbra.Glow.Connector
{
    /// <summary>
    /// Represents a physical Antumbra|Glow unit.
    /// </summary>
    public class GlowDevice : ConfigurationObserver, Configurable
    {
        public delegate void ConfigUpdate(Configurable obj);
        public event ConfigUpdate ConfigUpdateAvail;
        /// <summary>
        /// Device pointer
        /// </summary>
        public IntPtr dev {get; set;}
        /// <summary>
        /// Info pointer
        /// </summary>
        public IntPtr info { get; private set; }
        /// <summary>
        /// LightInfo pointer
        /// </summary>
        public IntPtr lightInfo { get; private set; }
        /// <summary>
        /// The id of this device as given by the manager upon creation
        /// </summary>
        public int id { get; private set; }
        /// <summary>
        /// ;D
        /// </summary>
        public bool beta { get; private set; }
        /// <summary>
        /// Stored value of last temperature read in mK (yeah, ikr)
        /// </summary>
        public int lastTemp { get; private set; }
        /// <summary>
        /// DeviceSettings obj for this device
        /// </summary>
        public DeviceSettings settings { get; private set; } //TODO make private
        /// <summary>
        /// ExtensionManager for this device
        /// </summary>
        private ExtensionManager extMgr;
        /// <summary>
        /// Integer representation of the device's status
        /// </summary>
        public int status { get; set; }

        public void SetDvrGbbrOrPrcsrExt(Guid id)
        {
            this.extMgr.UpdateExtension(id);
        }

        public bool SetDecOrNotf(Guid id)
        {
            bool result = this.extMgr.ToggleDecOrNotf(id);
            return result;
        }

        public void Notify()
        {
            this.settings.Notify();
            this.extMgr.activeExts.Notify();
        }

        public bool GetDecOrNotfStatus(Guid id)
        {
            foreach (GlowDecorator dec in this.extMgr.activeExts.ActiveDecorators)
                if (dec.id.Equals(id))
                    return true;
            foreach (GlowNotifier notf in this.extMgr.activeExts.ActiveNotifiers)
                if (notf.id.Equals(id))
                    return true;
            return false;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="beta"></param>
        /// <param name="id"></param>
        public GlowDevice(bool beta, int id, IntPtr info, ExtensionLibrary lib)
        {
            this.info = info;
            this.beta = beta;
            this.id = id;
            this.dev = IntPtr.Zero;
            this.settings = new DeviceSettings(id);
            this.settings.AttachObserver(this);
            this.extMgr = new ExtensionManager(lib, id);
            this.extMgr.activeExts.AttachObserver(this);
            this.settings.AttachObserver(this.extMgr);
            this.settings.Notify();//force init update
        }
        /// <summary>
        /// Start the device's extensions
        /// </summary>
        /// <returns>True if successfully started, else false</returns>
        public bool Start()
        {
            return this.extMgr.Start();
        }

        public void LoadSettings()
        {
            Saver saver = Saver.GetInstance();
            this.settings.LoadSave(saver.Load(this.id.ToString()));
            this.extMgr.LoadSave(saver.Load(ExtensionManager.configFileBase + this.id));
        }

        public void SaveSettings()
        {
            this.settings.Save();
            this.extMgr.Save();
        }
        /// <summary>
        /// Attach an AntumbraColorObserver to the underlying extension manager
        /// </summary>
        /// <param name="observer"></param>
        public void AttachColorObserverToExtMgr(AntumbraColorObserver observer)
        {
            this.extMgr.AttachObserver(observer);
        }

        public int ApplyDriverRecomSettings()
        {
            this.extMgr.activeExts.ActiveDriver.RecmmndCoreSettings();
            return this.extMgr.activeExts.ActiveDriver.stepSleep;
        }

        public void AttachObserver(ConfigurationObserver o)
        {
            this.ConfigUpdateAvail += o.ConfigurationUpdate;
        }

        public void ConfigurationUpdate(Configurable obj)
        {
            if (this.ConfigUpdateAvail != null)
                this.ConfigUpdateAvail(obj);//pass through
        }

        public void AttachToolbarNotifObserverToExtMgr(ToolbarNotificationObserver observer)
        {
            this.extMgr.AttachObserver(observer);
        }

        public void AttachLogObserverToExtMgr(LogMsgObserver observer)
        {
            this.extMgr.AttachObserver(observer);
        }

        public void AttachGlowCommandObserverToExtMgr(GlowCommandObserver observer)
        {
            this.extMgr.RegisterDevice(this.id);
            this.extMgr.AttachObserver(observer);
        }
        /// <summary>
        /// Get a string representation of the extensions activated for this device
        /// </summary>
        /// <returns>String representation of active extensions.</returns>
        public string GetSetupDesc()
        {
            return extMgr.GetSetupDesc();
        }
        /// <summary>
        /// Return string representation of this device.
        /// </summary>
        /// <returns>String describing this GlowDevice</returns>
        public override string ToString()
        {
            return "Glow device, id: " + this.id;
        }
        /// <summary>
        /// Stop the extensions for this device
        /// </summary>
        /// <returns>True if successful, else false</returns>
        internal bool Stop()
        {
            return this.extMgr.Stop();
        }

        public bool GetExtSettingsWin(Guid id)
        {
            return this.extMgr.GetExtSettingsWin(id);
        }
    }
}
