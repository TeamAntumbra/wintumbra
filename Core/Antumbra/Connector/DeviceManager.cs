using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Connector
{
    /// <summary>
    /// Manages dealing with connected Glow units via the SerialConnector class.
    /// Also manages device status.
    /// </summary>
    class DeviceManager
    {
        private AntumbraCore core;
        private SerialConnector Connector;
        private List<GlowDevice> Glows;
        private List<GlowDevice> ActiveGlows;
        public DeviceManager(AntumbraCore core, int vid, int pid)
        {
            this.core = core;
            this.Connector = new SerialConnector(vid, pid);
            this.Glows = new List<GlowDevice>();
            this.ActiveGlows = new List<GlowDevice>();
            int len = this.Connector.UpdateDeviceList();
            for (var i = 0; i < len; i += 1) {
                this.Glows.Add(new GlowDevice(true, i, this.Connector.GetDeviceInfo(i)));
            }
            if (this.Glows.Count > 0) {//at least 1 Glow found
                GlowDevice device = this.Glows.First<GlowDevice>();
                this.ActiveGlows.Add(device);//make the first the default
            }
        }

        private IntPtr OpenDevice(int index)
        {
            if (index < 0 || index >= this.Glows.Count)//invalid
                return IntPtr.Zero;
            int outerr;
            IntPtr result = this.Connector.OpenDevice(getDevice(index).info, out outerr);
            if (outerr != 0)
                Console.WriteLine("Error detected opening device. Code: " + outerr);
            return result;
        }
        /// <summary>
        /// Will return the status of the Glow device with
        /// the passed id. Will return -1 if not found.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int getStatus(int index)
        {
            foreach (var device in Glows)
                if (device.id == index)
                    return device.status;
            return -1;
        }

        public void sendColor(Color newColor)
        {
            sendColor(newColor.R, newColor.G, newColor.B);
        }

        public void sendColor(byte r, byte g, byte b)//TODO modifiy for multi-Glow use
        {
            foreach (var activeDev in this.ActiveGlows) {
                int err;
                if (activeDev.dev == IntPtr.Zero) {//needs opening
                    activeDev.dev = this.Connector.OpenDevice(activeDev.info, out err);
                }
                int status = this.Connector.SetDeviceColor(activeDev.id, activeDev.dev, r, g, b);
                activeDev.lastColor = Color.FromArgb(r, g, b);
                updateStatus(status);
            }
        }

        private GlowDevice getDevice(int index)
        {
            foreach (var dev in this.Glows)
                if (dev.id == index)
                    return dev;
            return null;
        }

        private void updateStatus(int state)
        {
            this.core.updateStatusText(state);
        }
    }
}
