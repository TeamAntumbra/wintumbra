using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
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
        private const string extPath = "./Extensions/";
        private ExtensionLibrary extLibrary;
        private Antumbra.Glow.View.ToolbarIcon toolbarIcon;
        public ToolbarIconController()
        {
            this.toolbarIcon = new Antumbra.Glow.View.ToolbarIcon();
            this.toolbarIcon.Hide();
            this.AttachObserver(this.toolbarIcon);
            MainWindowController mainController = new MainWindowController(this.toolbarIcon.ProductVersion, new EventHandler(Quit));
            mainController.AttachObserver(this);
            this.toolbarIcon.notifyIcon_MouseClickEvent += new EventHandler(mainController.showWindowEventHandler);
            this.AttachObserver(LoggerHelper.GetInstance());
            this.LogMsg("Wintumbra starting @ " + DateTime.Now.ToString());
        }

        private void Quit(object sender, EventArgs args)
        {
            this.toolbarIcon.Dispose();
            System.Windows.Forms.Application.Exit();
        }

        private void LogMsg(String msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("ToolbarIconController", msg);
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        /*public void Off(int id)
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
        */
        private void Log(string msg)
        {
            NewLogMsgAvailEvent("ToolbarIconController", msg);
        }

        /*// <summary>
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
        }*/

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
        }
    }
}
