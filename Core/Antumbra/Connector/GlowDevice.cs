using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Connector
{
    /// <summary>
    /// Represents a physical Antumbra|Glow unit.
    /// </summary>
    class GlowDevice
    {
        private IntPtr dev;
        /// <summary>
        /// The index this device is in the serialConnector's devs list
        /// </summary>
        public int index { get; private set; }
        /// <summary>
        /// ;D
        /// </summary>
        public bool beta { get; private set; }
        /// <summary>
        /// Unique id for this GlowDevice
        /// </summary>
        public int id { get; private set; }
        /// <summary>
        /// Stored value of last temperature read in mK (yeah, ikr)
        /// </summary>
        public int lastTemp { get; private set; }
        /// <summary>
        /// Returns the status of the Glow device
        /// </summary>
        public int status { get; private set; }
        /// <summary>
        /// Last color this Device was successfully set to
        /// </summary>
        public Color lastColor { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="beta"></param>
        /// <param name="index"></param>
        public GlowDevice(int id, bool beta, int index, IntPtr dev)
        {
            this.id = id;
            this.beta = beta;
            this.index = index;
            this.dev = dev;
        }

        public override string ToString()
        {
            return "Glow device, id: " + this.id;
        }
    }
}
