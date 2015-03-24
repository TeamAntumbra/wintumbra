using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Settings;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Colors;

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
        private AdvancedSettingsWindowManager advancedSettingsWinMgr;
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
            this.advancedSettingsWinMgr = new AdvancedSettingsWindowManager(productVersion, lib);
            foreach (var dev in this.Glows) {//create output loops
                this.outManager.CreateAndAddLoop(this, dev.id);
                this.advancedSettingsWinMgr.CreateAndAddNewController(dev);
            }
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

        public void Start(int id)
        {
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop == null)//needs to be created
                loop = this.outManager.CreateAndAddLoop(this, id);
            GlowDevice dev = getDevice(id);
            dev.AttachColorObserverToExtMgr(loop);
            if (dev.Start()) {
                NewToolbarNotifAvail(3000, "Device id: " + dev.id + " Started.",
                    "Device id: " + dev.id + " started successfully.", 0);
                this.Log(dev.GetSetupDesc());//use this format as to always null check TODO
            }
            else {//starting failed
                dev.Stop();
                NewToolbarNotifAvail(3000, "Starting Failed", "Starting the selected extensions failed.", 2);
                return;
            }
            loop.Start(dev.settings.weightingEnabled, dev.settings.newColorWeight);
        }

        public void Stop(int id)
        {
            var dev = this.getDevice(id);
            bool wasRunning = dev.running;
            if (wasRunning)//only show notifs if actually stopping device (still make call to clean up)
                this.NewToolbarNotifAvail(3000, "Stopping device id: " + id, "Stopping device id " + id +
                    " please wait.", 0);
            if (!dev.Stop())
                NewToolbarNotifAvail(3000, "Device " + id + " Did Not Stop Correctly",
                    "Device " + id + " reported that it did not stop correctly.",
                    1);
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop != null) {
                loop.Dispose();
            }
            if (wasRunning)
                NewToolbarNotifAvail(3000, "Device " + id + " Stopped.", "The current device has been stopped.", 1);
        }

        public void sendColor(Color16Bit newColor, int id)
        {
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
                if (err != 0)//error occured
                    return;
            }
            int status = this.Connector.SetDeviceColor(activeDev.id, activeDev.dev, r, g, b);
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
