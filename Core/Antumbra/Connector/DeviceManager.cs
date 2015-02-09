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
        private SerialConnector Connector;//move to GlowDevice(s)?
        private List<GlowDevice> Glows;
        private int vid, pid;
        public DeviceManager(AntumbraCore core, int vid, int pid)
        {
            this.core = core;
            this.Connector = new SerialConnector(vid, pid);
        }
        /// <summary>
        /// Will return the status of the Glow device with
        /// the passed id. Will return -1 if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int getStatus(int id)
        {
            foreach (var device in Glows)
                if (device.id == id)
                    return device.status;
            return -1;
        }

        public void sendColor(int id, byte r, byte g, byte b)
        {
        /*    GlowDevice dev = getDevice(id);
            if (dev != null)
                dev.changeTo(r, g, b);*/
        }

        private GlowDevice getDevice(int id)
        {
            foreach (var dev in this.Glows)
                if (dev.id == id)
                    return dev;
            return null;
        }

        private void updateLast(int id, Color last)
        {
            GlowDevice dev = getDevice(id);
            if (dev != null)
                dev.lastColor = last;
        }

        private void changeTo(byte r, byte g, byte b)
        {
      /*      Console.WriteLine(r + " - " + g + "  -  " + b);
            if (this.Connector.send(r, g, b)) {//sucessful send
                updateLast(r, g, b);
                this.updateStatus(2);
            }
            else {
                this.updateStatus(0);//send failed, device is probably dead / not connected
                Console.WriteLine("color send failed!");
            }*/
        }

        public void checkStatus()
        {
            updateStatus(this.Connector.state);
        }

        private void updateStatus(int state)
        {
            this.core.updateStatusText(state);
        }
    }
}
