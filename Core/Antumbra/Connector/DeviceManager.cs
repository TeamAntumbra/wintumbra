using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Settings;
using Antumbra.Glow.ExtensionFramework;

namespace Antumbra.Glow.Connector
{
    /// <summary>
    /// Manages dealing with connected Glow units via the SerialConnector class.
    /// Also manages device command sending on a higher level.
    /// </summary>
    public class DeviceManager
    {
        private SerialConnector Connector;
        public List<GlowDevice> Glows { get; private set; }
        //public List<GlowDevice> ActiveGlows { get; private set; }
        public int status { get; private set; }
        public int GlowsFound { get; private set; }

        public DeviceManager(int vid, int pid, MEFHelper mef)
        {
            this.status = 0;
            this.GlowsFound = 0;
            this.Connector = new SerialConnector(vid, pid);
            this.Glows = new List<GlowDevice>();
            int len = this.Connector.UpdateDeviceList();
            for (var i = 0; i < len; i += 1) {
                this.Glows.Add(new GlowDevice(true, i, this.Connector.GetDeviceInfo(i), mef));
            }
            this.GlowsFound = this.Glows.Count;
        }

        private bool OpenDevice(int id)
        {
            if (id < 0 || id >= this.Glows.Count)//invalid
                return false;
            int outerr;
            IntPtr result = this.Connector.OpenDevice(getDevice(id).info, out outerr);
            if (outerr != 0)
                return false;
            this.getDevice(id).dev = result;
            return true;
        }

        public void sendColor(Color newColor) {
            foreach (var dev in this.Glows)
                sendColor(newColor, dev.id);
        }

        public void sendColor(Color newColor, int id)
        {
            sendColor(newColor.R, newColor.G, newColor.B, id);
        }

        public void sendColor(byte r, byte g, byte b, int id)
        {
            GlowDevice activeDev = getDevice(id);
            if (activeDev == null)//device not found
                return;
            int err;
            if (activeDev.dev == IntPtr.Zero) {//needs opening
                activeDev.dev = this.Connector.OpenDevice(activeDev.info, out err);
                if (err != 0)//error occured
                    return;
            }
            int status = this.Connector.SetDeviceColor(activeDev.id, activeDev.dev, r, g, b);
            this.status = status;
            activeDev.lastColor = Color.FromArgb(r, g, b);
        }

        public string GetDeviceSetupDecs()
        {
            string result = "";
            foreach (var dev in this.Glows)
                result += dev.GetSetupDesc();
            if (result.Equals(""))
                return "No devices found";
            return result;
        }

        public DeviceSettings getDeviceSettings(int id)
        {
            GlowDevice dev = getDevice(id);
            if (dev == null)
                return null;
            return dev.settings;
        }

        public GlowDevice getDevice(int id)
        {
            foreach (var dev in this.Glows)
                if (dev.id == id)
                    return dev;
            return null;
        }

        public void CleanUp()
        {
            CloseAll();
            FreeList();
        }

        private void CloseAll()
        {
            foreach (var active in this.Glows) {
                IntPtr ptr = active.dev;
                if (!ptr.Equals(IntPtr.Zero))//actually open?
                    this.Connector.CloseDevice(active.dev);
            }
        }

        private void FreeList()
        {
            this.Connector.FreeList();
        }
    }
}
