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
        public ExtensionManager extMgr { get; private set; }
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

        public string GetSetupDesc()
        {
            var mgr = this.extMgr;
            string config = "Driver: " + mgr.ActiveDriver.ToString();
            config += "\nGrabber: " + mgr.ActiveGrabber.ToString();
            config += "\nProcessor: " + mgr.ActiveProcessor.ToString();
            config += "\nDecorator(s): ";
            var decStr = "None";
            var decs = mgr.ActiveDecorators;
            for (var i = 0; i < decs.Count; i += 1) {
                if (i == 0)//first one
                    decStr = "";
                var ele = decs.ElementAt(i).ToString();
                if (i == decs.Count - 1)//last one
                    decStr += ele;
                else
                    decStr += ele + ", ";
            }
            config += decStr;
            config += "\nNotifier(s): ";
            var notfs = mgr.ActiveNotifiers;
            var notfStr = "None";
            for (var i = 0; i < notfs.Count; i += 1) {
                if (i == 0)//first one
                    notfStr = "";
                var ele = notfs.ElementAt(i).ToString();
                if (i == notfs.Count - 1)//last one
                    notfStr += ele;
                else
                    notfStr += ele + ", ";
            }
            return config + notfStr + "\n";
        }

        public override string ToString()
        {
            return "Glow device, id: " + this.id;
        }
    }
}
