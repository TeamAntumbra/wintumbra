using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.Settings;
using Antumbra.Glow.ExtensionFramework;

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
        /// Last color this Device was successfully set to
        /// </summary>
        public Color lastColor { get; set; }
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
            set
            {
                this.extMgr.ActiveDriver = value;
            }
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
            set
            {
                this.extMgr.ActiveGrabber = value;
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
            set
            {
                this.extMgr.ActiveProcessor = value;
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
        /// <summary>
        /// Remove the passed Decorator from the ActiveDecorators
        /// or add it if not found.
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public bool RemoveDecOrAddIfNew(GlowDecorator dec) {
            GlowDecorator toRemove = null;
            foreach (var d in this.ActiveDecorators) {
                if (d.id == dec.id) {
                    toRemove = d;
                    break;
                }
            }
            if (toRemove != null) {
                this.ActiveDecorators.Remove(toRemove);
                return true;
            }
            this.ActiveDecorators.Add(dec);
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
        /// Remove the passed Notifier from ActiveNotifiers or add
        /// it if not found.
        /// </summary>
        /// <param name="notf"></param>
        /// <returns></returns>
        public bool RemoveNotfOrAddIfNew(GlowNotifier notf)
        {
            GlowNotifier toRemove = null;
            foreach (var d in this.ActiveNotifiers) {
                if (d.id == notf.id) {
                    toRemove = d;
                    break;
                }
            }
            if (toRemove != null) {
                this.ActiveNotifiers.Remove(toRemove);
                return true;//removed
            }
            this.ActiveNotifiers.Add(notf);
            return false;//added
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="beta"></param>
        /// <param name="id"></param>
        public GlowDevice(bool beta, int id, IntPtr info, string path)
        {
            this.info = info;
            this.beta = beta;
            this.id = id;
            this.dev = IntPtr.Zero;
            this.settings = new DeviceSettings(id);
            this.extMgr = new ExtensionManager(path, id, settings);
        }
        /// <summary>
        /// Start the device's extensions
        /// </summary>
        /// <returns>True if successfully started, else false</returns>
        internal bool Start()
        {
            return this.extMgr.Start();
        }
        /// <summary>
        /// Attach an AntumbraColorObserver to the underlying extension manager
        /// </summary>
        /// <param name="observer"></param>
        public void AttachEventToExtMgr(AntumbraColorObserver observer)
        {
            this.extMgr.AttachEvent(observer);
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
    }
}
