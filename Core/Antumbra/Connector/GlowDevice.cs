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
        public IntPtr dev {get; set;}
        public IntPtr info { get; private set; }
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
        public DeviceSettings settings { get; private set; }
        private ExtensionManager extMgr;
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
        public List<GlowDecorator> ActiveDecorators
        {
            get
            {
                return this.extMgr.ActiveDecorators;
            }
            set
            {
                this.extMgr.ActiveDecorators = value;
            }
        }
        public List<GlowNotifier> ActiveNotifiers
        {
            get
            {
                return this.extMgr.ActiveNotifiers;
            }
            set
            {
                this.extMgr.ActiveNotifiers = value;
            }
        }
        public int status { get; set; }
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

        internal bool Start()
        {
            return this.extMgr.Start();
        }

        public void AttachEventToExtMgr(AntumbraColorObserver observer)
        {
            this.extMgr.AttachEvent(observer);
        }

        public string GetSetupDesc()
        {
            return extMgr.GetSetupDesc();
        }

        public override string ToString()
        {
            return "Glow device, id: " + this.id;
        }

        internal bool Stop()
        {
            return this.extMgr.Stop();
        }
    }
}
