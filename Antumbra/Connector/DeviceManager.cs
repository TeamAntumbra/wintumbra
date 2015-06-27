using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Utility;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.GlowCommands;

namespace Antumbra.Glow.Connector
{
    /// <summary>
    /// Manages dealing with connected Glow units via the SerialConnector class.
    /// Also manages device command sending on a higher level.
    /// </summary>
    public class DeviceManager : ToolbarNotificationObserver, ToolbarNotificationSource, Loggable,
                                 GlowCommandObserver//TODO add reloading
    {
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewLogMsgAvail(string title, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        private SerialConnector Connector;
        private OutputLoopManager outManager;
        public List<GlowDevice> Glows { get; private set; }
        public int status { get; private set; }
        public int GlowsFound { get; private set; }

        public DeviceManager(int vid, int pid, ExtensionLibrary lib, string productVersion)
        {
            this.status = 0;
            this.GlowsFound = 0;
            this.Connector = new SerialConnector(vid, pid);
            this.Glows = new List<GlowDevice>();
            int len = this.Connector.UpdateDeviceList();
            for (var i = 0; i < len; i += 1) {
                this.Glows.Add(new GlowDevice(true, i, this.Connector.GetDeviceInfo(i), lib));
            }
            this.GlowsFound = this.Glows.Count;
            this.outManager = new OutputLoopManager();
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void NewGlowCommandAvail(GlowCommand cmd)
        {
            cmd.ExecuteCommand(this);
        }

        private bool StartAll()
        {
            bool result = true;
            foreach (GlowDevice dev in this.Glows)
                if (!Start(dev.id))//failed to start
                    result = false;
            return result;
        }

        public bool Start(int id)
        {
            if (id == -1) {
                return StartAll();
            }
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop == null)//needs to be created
                loop = this.outManager.CreateAndAddLoop(this, id);
            GlowDevice dev = getDevice(id);
            dev.AttachColorObserverToExtMgr(loop);
            if (dev.Start()) {
                this.Log("Device id: " + dev.id + " started successfully.");
                this.Log(dev.GetSetupDesc());//use this format as to always null check TODO
            }
            else {//starting failed
                dev.Stop();
                this.Log("Starting the selected extensions failed. " + dev.GetSetupDesc());
                return false;
            }
            loop.Start(dev.settings.weightingEnabled, dev.settings.newColorWeight);
            return true;
        }

        private void StopAll()
        {
            foreach (GlowDevice dev in this.Glows)
                Stop(dev.id);
        }

        public void Stop(int id)
        {
            if (id == -1) {
                StopAll();
                return;//cancel this call
            }
            var dev = this.getDevice(id);
            if (dev == null)//no device
                return;
            if (!dev.Stop())
                this.Log("Device " + id + " reported that it did not stop correctly.");
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop != null) {
                loop.Dispose();
            }
            this.Log("Device " + id + " Stopped.");
        }

        public void sendColor(Antumbra.Glow.Observer.Colors.Color16Bit newColor, int id)
        {
            if (id == -1) {
                foreach (GlowDevice device in this.Glows) {
                    newColor = device.ApplyDecorations(newColor);
                    newColor = device.ApplyBrightness(newColor);
                    newColor = device.ApplyWhiteBalance(newColor);
                    sendColor(newColor.red, newColor.green, newColor.blue, device.id);
                }
                return; //cancel this call
            }
            GlowDevice dev = this.getDevice(id);
            if (dev == null)
                return;//no device found matching passed id
            newColor = dev.ApplyDecorations(newColor);
            newColor = dev.ApplyBrightness(newColor);
            newColor = dev.ApplyWhiteBalance(newColor);
            sendColor(newColor.red, newColor.green, newColor.blue, id);
        }

        public void sendColor(UInt16 r, UInt16 g, UInt16 b, int id)
        {
            GlowDevice activeDev = getDevice(id);
            if (activeDev == null)//device not found
                return;
            int err;
            if (activeDev.dev == IntPtr.Zero) {//needs opening
                activeDev.dev = this.Connector.OpenDevice(activeDev.info, out err);
                if (err != 0) {//error occured
                    System.Threading.Thread.Sleep(10);
                    return;
                }
            }
            int status = this.Connector.SetDeviceColor(activeDev.id, activeDev.dev, r, g, b);
            //if (status != 0)//did not work as expected
            //    status = this.Connector.SetDeviceColor(activeDev.id, activeDev.dev, r, g, b);//try again
            this.status = status;
        }

        public void Log(string msg)
        {
            if (this.NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("Device Manager", msg);
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
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

        public GlowDevice getDevice(int id)
        {
            foreach (var dev in this.Glows)
                if (dev.id == id)
                    return dev;
            return null;
        }

        public void CleanUp()
        {
            sendColor(new Observer.Colors.Color16Bit(0, 0, 0), -1);
            foreach (GlowDevice dev in this.Glows) {
                dev.SaveSettings();
                dev.Stop();
            }
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
