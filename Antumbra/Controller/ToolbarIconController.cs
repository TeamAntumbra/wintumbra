using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Settings;
using System.Drawing;

namespace Antumbra.Glow.Controller
{
    public class ToolbarIconController : Loggable, ToolbarNotificationObserver, ToolbarNotificationSource
    {
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewLogMsgAvail(string source, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        private DeviceManager GlowManager;
        private AdvancedSettingsWindowManager SettingsWindowManager;
        private OutputLoopManager outManager;
        private const string extPath = "./Extensions/";
        private ExtensionLibrary extLibrary;
        private ToolbarIcon toolbarIcon;
        public ToolbarIconController()
        {
            this.toolbarIcon = new ToolbarIcon();
            this.AttachObserver(LoggerHelper.GetInstance());
            this.LogMsg("Wintumbra starting @ " + DateTime.Now.ToString());
            try {
                this.extLibrary = new ExtensionLibrary(extPath);//load extensions into lib
            }
            catch (System.Reflection.ReflectionTypeLoadException e) {
                string msg = "";
                foreach (var err in e.LoaderExceptions)
                    msg += err.Message;
                NewToolbarNotifAvail(10000, "Exception Occured While Loading Extensions", msg, 2);
                Thread.Sleep(10000);//wait for message
                throw e;//pass up
            }
            this.LogMsg("Creating DeviceManager");
            this.GlowManager = new DeviceManager(0x16D0, 0x0A85, this.extLibrary);//find devices
            this.LogMsg("Creating OutputLoopManager");
            this.outManager = new OutputLoopManager();//TODO have device manager create outputloop manager upon creation...maybe
            this.LogMsg("Creating OutputLoops");
            foreach (var dev in this.GlowManager.Glows) {//create output loop
                this.outManager.CreateAndAddLoop(GlowManager, dev.id);
            }
            this.SettingsWindowManager = new AdvancedSettingsWindowManager(this.toolbarIcon.ProductVersion, this.extLibrary);
            //this.SettingsWindowManager.AttachObserver((GlowCommandObserver)this);
            this.SettingsWindowManager.AttachObserver((ToolbarNotificationObserver)this);
            if (GlowManager.GlowsFound > 0) {//ready first device for output if any are found
                GlowDevice dev = this.GlowManager.getDevice(0);
                this.SettingsWindowManager.CreateAndAddNewController(dev);
            }
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            if (this.NewToolbarNotifAvailEvent != null)
                this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        private void LogMsg(String msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("ToolbarIconController", msg);
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            if (this.NewLogMsgAvailEvent != null)
                this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void Off(int id)
        {
            if (id == -1) {
                this.StopAll();
                this.GlowManager.sendColor(Color.Black);
            }
            else {
                this.Stop(id);
                this.GlowManager.sendColor(Color.Black, id);
            }
        }

        private void Off()
        {
            this.StopAll();
            this.GlowManager.sendColor(Color.Black);
        }
        public void SendColor(int id, Color col)
        {
            this.GlowManager.sendColor(col, id);
        }
        /// <summary>
        /// Start all found Glows
        /// </summary>
        private void StartAll()//TODO move
        {
            if (this.GlowManager.GlowsFound == 0) {
                NewToolbarNotifAvail(3000, "No Devices Found", "No devices were found to start.", 2);
                return;
            }
            StopAll();
            NewToolbarNotifAvail(3000, "Starting All", "Extensions are being started. Please wait.", 0);

            foreach (var dev in this.GlowManager.Glows) {//start each output loop
                this.Start(dev.id);
            }
        }
        /// <summary>
        /// Start the device (if found) with the id passed
        /// </summary>
        /// <param name="id"></param>
        public void Start(int id)//TODO move
        {
            //      this.logger.Log("Starting device id: " + id);
            Stop(id);
            var dev = this.GlowManager.getDevice(id);
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop == null)//needs to be created
                loop = this.outManager.CreateAndAddLoop(this.GlowManager, id);
            //dev.AttachLogObserverToExtMgr(this);
            dev.AttachToolbarNotifObserverToExtMgr(this);
            //dev.AttachGlowCommandObserverToExtMgr(this);
            dev.AttachColorObserverToExtMgr(loop);
            if (dev.Start()) {
                NewToolbarNotifAvail(3000, "Device id: " + dev.id + " Started.",
                    "Device id: " + dev.id + " started successfully.", 0);
                this.Log(dev.GetSetupDesc());
            }
            else {//starting failed
                dev.Stop();
                NewToolbarNotifAvail(3000, "Starting Failed", "Starting the selected extensions failed.", 2);
                return;
            }
            loop.Start(dev.settings.weightingEnabled, dev.settings.newColorWeight);
        }

        private void Log(string msg)
        {
            NewLogMsgAvailEvent("ToolbarIconController", msg);
        }

        /// <summary>
        /// Stop the device (if found) with the id passed
        /// </summary>
        /// <param name="id"></param>
        public void Stop(int id)//TODO move
        {
            var dev = this.GlowManager.getDevice(id);
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
        /// <summary>
        /// Stop all found devices
        /// </summary>
        private void StopAll()
        {
            if (this.GlowManager.GlowsFound == 0) {
                NewToolbarNotifAvail(3000, "No Devices Found", "No devices were found to stop.", 2);
                return;
            }
            foreach (var dev in this.GlowManager.Glows) {
                this.Stop(dev.id);
            }
        }

        /// <summary>
        /// Announce the current devices extension configuration
        /// </summary>
        private void AnnounceConfig()
        {
            NewToolbarNotifAvail(5000, "Current Configurations", this.GlowManager.GetDeviceSetupDecs(), 0);
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
        }
    }
}
