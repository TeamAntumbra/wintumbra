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
using Antumbra.Glow.Utility.Settings;

namespace Antumbra.Glow.Connector
{
    /// <summary>
    /// Represents a physical Antumbra|Glow unit.
    /// </summary>
    public class GlowDevice
    {
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
        public DeviceSettings settings { get; private set; }
        /// <summary>
        /// ExtensionManager for this device
        /// </summary>
        private ExtensionManager extMgr;
        /// <summary>
        /// Integer representation of the device's status
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// Active GlowDriver for this device
        /// </summary>
        public GlowDriver ActiveDriver
        {
            get
            {
                return this.extMgr.ActiveDriver;
            }
        }

        public void SetDvrGbbrOrPrcsrExt(Guid id)
        {
            this.extMgr.UpdateExtension(id);
        }

        public bool SetDecOrNotf(Guid id)
        {
            return this.extMgr.ToggleDecOrNotf(id);
        }
        /// <summary>
        /// Active GlowScreenGrabber for this device
        /// </summary>
        public GlowScreenGrabber ActiveGrabber
        {
            get
            {
                return this.extMgr.ActiveGrabber;
            }
        }
        /// <summary>
        /// Active GlowScreenProcessor for this device
        /// </summary>
        public GlowScreenProcessor ActiveProcessor
        {
            get
            {
                return this.extMgr.ActiveProcessor;
            }
        }
        /// <summary>
        /// Active GlowDecorators for this device
        /// </summary>
        public List<GlowDecorator> ActiveDecorators
        {
            get
            {
                return this.extMgr.ActiveDecorators;
            }
        }

        public bool GetDecOrNotfStatus(Guid id)
        {
            foreach (GlowDecorator dec in this.ActiveDecorators)
                if (dec.id.Equals(id))
                    return true;
            foreach (GlowNotifier notf in this.ActiveNotifiers)
                if (notf.id.Equals(id))
                    return true;
            return false;
        }
        /// <summary>
        /// Active GlowNotifiers for this device
        /// </summary>
        public List<GlowNotifier> ActiveNotifiers
        {
            get
            {
                return this.extMgr.ActiveNotifiers;
            }
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
            this.extMgr = new ExtensionManager(lib, id, this.settings);
        }
        /// <summary>
        /// Start the device's extensions
        /// </summary>
        /// <returns>True if successfully started, else false</returns>
        internal bool Start()
        {
            return this.extMgr.Start();
        }

        public void LoadSettings()
        {
            Saver saver = Saver.GetInstance();
            this.settings.LoadSettings(saver.Load(this.id.ToString()));
            this.extMgr.LoadSettings(saver.Load("ExtMgr"));
        }

        public void SaveSettings()
        {
            this.settings.SaveSettings();
            this.extMgr.SaveSettings();
        }
        /// <summary>
        /// Attach an AntumbraColorObserver to the underlying extension manager
        /// </summary>
        /// <param name="observer"></param>
        public void AttachColorObserverToExtMgr(AntumbraColorObserver observer)
        {
            this.extMgr.AttachColorObserver(observer);
        }

        public void AttachToolbarNotifObserverToExtMgr(ToolbarNotificationObserver observer)
        {
            this.extMgr.AttachToolbarNotifObserver(observer);
        }

        public void AttachLogObserverToExtMgr(LogMsgObserver observer)
        {
            this.extMgr.AttachLogObserver(observer);
        }

        public void AttachGlowCommandObserverToExtMgr(GlowCommandObserver observer)
        {
            this.extMgr.RegisterDevice(this.id);
            this.extMgr.AttachGlowCommandObserver(observer);
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
