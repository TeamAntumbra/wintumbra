using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Antumbra.Glow.Connector;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using System.Reflection;
using Microsoft.Win32;

namespace Antumbra.Glow
{
    public partial class ToolbarIconController : Form, Loggable, ToolbarNotificationObserver
    {
        public delegate void NewLogMsgAvail(string source, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        private DeviceManager GlowManager;
        private SettingsWindowManager SettingsWindowManager;
        private OutputLoopManager outManager;
        private const string extPath = "./Extensions/";
        private ExtensionLibrary extLibrary;
        //private Logger logger;
        /// <summary>
        /// ToolbarIconController Constructor - Main entry point into the system
        /// </summary>
        public ToolbarIconController()
        {
            this.AttachObserver(LoggerHelper.GetInstance());
            this.LogMsg("Wintumbra starting @ " + DateTime.Now.ToString());
            InitializeComponent();
            try {
                this.extLibrary = new ExtensionLibrary(extPath);//load extensions into lib
            }
            catch (System.Reflection.ReflectionTypeLoadException e) {
                string msg = "";
                foreach (var err in e.LoaderExceptions)
                    msg += err.Message;
                ShowMessage(10000, "Exception Occured While Loading Extensions", msg, ToolTipIcon.Error);
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
            this.SettingsWindowManager = new SettingsWindowManager(this.ProductVersion, this.extLibrary);
            //this.SettingsWindowManager.AttachObserver((GlowCommandObserver)this);
            this.SettingsWindowManager.AttachObserver((ToolbarNotificationObserver)this);
            if (GlowManager.GlowsFound > 0) {//ready first device for output if any are found
                GlowDevice dev = this.GlowManager.getDevice(0);
                this.SettingsWindowManager.CreateAndAddNewController(dev);
            }
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

        public void NewToolbarNotifAvail(int time, String title, String msg, int icon)
        {
            ToolTipIcon notifIcon = ToolTipIcon.None;//default
            switch (icon) {
                case 0:
                    notifIcon = ToolTipIcon.Info;
                    break;
                case 1:
                    notifIcon = ToolTipIcon.Warning;
                    break;
                case 2:
                    notifIcon = ToolTipIcon.Error;
                    break;
            }
            this.ShowMessage(time, title, msg, notifIcon);
        }
        /// <summary>
        /// Event handler for when the menubar icon is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                Console.Write("TODO");//TODO
        }
        /// <summary>
        /// Announce the current devices extension configuration
        /// </summary>
        private void AnnounceConfig()
        {
            ShowMessage(5000, "Current Configurations", this.GlowManager.GetDeviceSetupDecs(), ToolTipIcon.Info);
        }
        /// <summary>
        /// Show the passed message as a balloon of the applications NotifyIcon
        /// </summary>
        /// <param name="time"></param>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="icon"></param>
        private void ShowMessage(int time, string title, string msg, ToolTipIcon icon)
        {
        //    this.logger.Log("Message shown to user in bubble. Message following.\n" + msg);
            this.notifyIcon.ShowBalloonTip(time, title, msg, icon);
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
                ShowMessage(3000, "No Devices Found", "No devices were found to start.", ToolTipIcon.Error);
                return;
            }
            StopAll();
            ShowMessage(3000, "Starting All", "Extensions are being started. Please wait.", ToolTipIcon.Info);

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
                this.ShowMessage(3000, "Device id: " + dev.id + " Started.", 
                    "Device id: " + dev.id + " started successfully.", ToolTipIcon.Info);
                this.Log(dev.GetSetupDesc());
            }
            else {//starting failed
                dev.Stop();
                this.ShowMessage(3000, "Starting Failed", "Starting the selected extensions failed.", ToolTipIcon.Error);
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
                this.ShowMessage(3000, "Stopping device id: " + id, "Stopping device id " + id +
                    " please wait.", ToolTipIcon.Info);
            if (!dev.Stop())
                ShowMessage(3000, "Device " + id + " Did Not Stop Correctly",
                    "Device " + id + " reported that it did not stop correctly.",
                    ToolTipIcon.Warning);
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop != null) {
                loop.Dispose();
            }
            if (wasRunning)
                ShowMessage(3000, "Device " + id + " Stopped.", "The current device has been stopped.", ToolTipIcon.Info);
        }
        /// <summary>
        /// Stop all found devices
        /// </summary>
        private void StopAll()
        {
            if (this.GlowManager.GlowsFound == 0) {
                ShowMessage(3000, "No Devices Found", "No devices were found to stop.", ToolTipIcon.Error);
                return;
            }
            foreach (var dev in this.GlowManager.Glows) {
                this.Stop(dev.id);
            }
        }
    }
}
