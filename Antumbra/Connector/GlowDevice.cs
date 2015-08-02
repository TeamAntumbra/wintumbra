using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.Settings;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Utility.Saving;

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
        /// The device's latest status response
        /// </summary>
        public int status { get; set; }
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
        }
        /// <summary>
        /// Return string representation of this device.
        /// </summary>
        /// <returns>String describing this GlowDevice</returns>
        public override string ToString()
        {
            return "Glow device, id: " + this.id;
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifEvent += observer.NewToolbarNotifAvail;
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon)
        {
            if (this.NewToolbarNotifEvent != null)
                this.NewToolbarNotifEvent(time, title, msg, icon);
        }
    }
}
