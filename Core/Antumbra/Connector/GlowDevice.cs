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
        const int DEAD = 0;
        const int IDLE = 1;
        const int ALIVE = 2;
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
        /// Returns the status of the Glow device
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// Last color this Device was successfully set to
        /// </summary>
        public Color lastColor { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="beta"></param>
        /// <param name="id"></param>
        public GlowDevice(bool beta, int id, IntPtr info)
        {
            this.info = info;
            this.beta = beta;
            this.id = id;
            this.dev = IntPtr.Zero;
            this.status = DEAD;
        }

        public override string ToString()
        {
            return "Glow device, index: " + this.id;
        }
    }
}
