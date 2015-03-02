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
using Antumbra.Glow.Utility;
using Antumbra.Glow.Settings;
using System.Reflection;
using Microsoft.Win32;

namespace Antumbra.Glow
{
    public partial class AntumbraCore : Form
    {
        private DeviceManager GlowManager;
        private List<SettingsWindow> settingsWindows;
        private OutputLoopManager outManager;
        private const string extPath = "./Extensions/";
        private ExtensionLibrary extLibrary;
        private Logger logger;
        public bool goodStart { get; private set; }//start-up completion status
        /// <summary>
        /// AntumbraCore Constructor
        /// </summary>
        public AntumbraCore()
        {
            this.logger = new Logger("WintumbraLog.txt");
            this.logger.Log("Wintumbra Starting... @ " + DateTime.Now.ToString());
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerModeChanged);
            this.goodStart = true;
            InitializeComponent();
            try {
                this.extLibrary = new ExtensionLibrary(extPath);//load extensions and assign GUIDs
            }
            catch (System.Reflection.ReflectionTypeLoadException e) {
                string msg = "";
                foreach (var err in e.LoaderExceptions)
                    msg += err.Message;
                this.logger.Log("Exception occured while loading exceptions. Exceptions following:");
                this.logger.Log(msg);
                ShowMessage(10000, "Exception Occured While Loading Extensions", msg, ToolTipIcon.Error);
                this.goodStart = false;
                Thread.Sleep(10000);//wait for message
                return;//skip rest
            }
            this.logger.Log("Creating DeviceManager");
            this.GlowManager = new DeviceManager(0x16D0, 0x0A85, this.extLibrary);//find devices
            this.logger.Log("Devices Found: " + this.GlowManager.GlowsFound);
            this.logger.Log("Creating OutputLoopManager");
            this.outManager = new OutputLoopManager();
            this.logger.Log("Creating OutputLoops");
            foreach (var dev in this.GlowManager.Glows) {//create output loop
                this.outManager.CreateAndAddLoop(GlowManager, dev.id);
                this.toolStripDeviceList.Items.Add(dev);
            }
            this.settingsWindows = new List<SettingsWindow>();
            if (GlowManager.GlowsFound > 0) {//ready first device for output if any are found
                this.toolStripDeviceList.SelectedIndex = 0;
                this.settingsWindows.Add(new SettingsWindow(this.GlowManager.getDevice(0), this.extLibrary, this));
            }
            this.logger.Log("Core good start? - " + this.goodStart);
        }
        /// <summary>
        /// Event handler for when the menubar icon is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (contextMenu.Visible)
                contextMenu.Hide();
            else
                contextMenu.Show(Cursor.Position);
        }
        /// <summary>
        /// Event handler for the settings menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            if (this.GlowManager.GlowsFound == 0) {
                this.ShowMessage(3000, "No Glow Devices Found",
                    "No Devices were found to edit the settings of.", ToolTipIcon.Info);
                return;
            }
            GlowDevice current = (GlowDevice)toolStripDeviceList.SelectedItem;
            this.logger.Log("Opening settings window for device id: " + current.id);
            SettingsWindow win;
            if (this.settingsWindows.Count > current.id) {//in range
                win = this.settingsWindows.ElementAt<SettingsWindow>(current.id);
            }
            else {
                win = new SettingsWindow(current, this.extLibrary, this);
                this.settingsWindows.Add(win);
            }
            win.updateValues();
            win.Show();
        }
        /// <summary>
        /// Event handler for the start all devices button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAllItem_Click(object sender, System.EventArgs e)
        {
            this.StartAll();
        }
        /// <summary>
        /// Event handler for the stop all devices button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopAllItem_Click(object sender, System.EventArgs e)
        {
            this.StopAll();
        }
        /// <summary>
        /// Event handler for the quit program menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            this.logger.Log("Wintumbra Quitting...");
            this.Off();
            this.notifyIcon.Visible = false;
            this.notifyIcon.Dispose();
            this.contextMenu.Visible = false;
            this.contextMenu.Dispose();
            this.logger.Log("GlowManager cleaning up.");
            this.GlowManager.CleanUp();
            this.logger.Log("Cleaning up extension settings windows");
            foreach (var win in this.settingsWindows)
                win.CleanUp();
            this.Dispose();
            Application.Exit();
        }
        /// <summary>
        /// Event handler for current devices output rate menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void currentOutRateItem_Click(object sender, System.EventArgs e)
        {
            string outSpeeds = this.outManager.GetSpeedsStr();
            ShowMessage(3000, "Current Output Speed(s)", outSpeeds, ToolTipIcon.Info);
        }
        /// <summary>
        /// Event handler for whats currently configured menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void whatsMyConfig_Click(object sender, EventArgs e)
        {
            AnnounceConfig();
        }
        /// <summary>
        /// Announce the current devices extension configuration
        /// </summary>
        public void AnnounceConfig()
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
        public void ShowMessage(int time, string title, string msg, ToolTipIcon icon)//TODO somewhat replace with eventhandler and delegate for showing messages
        {
            this.logger.Log("Message shown to user in bubble. Message following.\n" + msg);
            this.notifyIcon.ShowBalloonTip(time, title, msg, icon);
        }
        //replace with custom event handler as well TODO
        public void Off()
        {
            this.StopAll();
            this.GlowManager.sendColor(Color.Black);
        }
        //TODO replace with custom event handler from dev mgr
        public void SendColor(int id, Color col)
        {
            this.GlowManager.sendColor(col, id);
        }
        /// <summary>
        /// Event handler for off menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void offToolStripMenuItem_Click(object sender, EventArgs e)//TODO make offCurrent
        {
            if (this.GlowManager.GlowsFound == 0)
                ShowMessage(3000, "No Devices Found", "No devices were found to turn off.", ToolTipIcon.Error);
            else
                this.Off();
        }
        /// <summary>
        /// Start all found Glows
        /// </summary>
        public void StartAll()
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
            ShowMessage(3000, "Started", "Extensions have been started.", ToolTipIcon.Info);
        }

        /// <summary>
        /// Start only the selected Glow
        /// </summary>
        public void StartCurrent()
        {
            StopCurrent();
            Start(this.toolStripDeviceList.SelectedIndex);
        }
        /// <summary>
        /// Start the device (if found) with the id passed
        /// </summary>
        /// <param name="id"></param>
        public void Start(int id)
        {
            this.logger.Log("Starting device id: " + id);
            Stop(id);
            var dev = this.GlowManager.getDevice(id);
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop == null)
                loop = this.outManager.CreateAndAddLoop(this.GlowManager, id);
            dev.AttachEventToExtMgr(loop);
            if (dev.Start()) {
                this.logger.Log("Device id: " + dev.id + " started successfully.");
                this.logger.Log("Current Configuration: " + dev.GetSetupDesc());
            }
            else {//starting failed
                dev.Stop();
                this.ShowMessage(3000, "Starting Failed", "Starting the selected extensions failed.", ToolTipIcon.Error);
                return;
            }
            loop.Start(dev.settings.weightingEnabled, dev.settings.newColorWeight);
            ShowMessage(3000, "Device " + id + " Started.", "The current device has been started.",
                ToolTipIcon.Info);
        }
        /// <summary>
        /// Stop the currently selected Glow
        /// </summary>
        public void StopCurrent()
        {
            this.Stop(this.toolStripDeviceList.SelectedIndex);
        }
        /// <summary>
        /// Stop the device (if found) with the id passed
        /// </summary>
        /// <param name="id"></param>
        public void Stop(int id)
        {
            this.logger.Log("Stopping device id: " + id);
            var dev = this.GlowManager.getDevice(id);
            if (!dev.Stop())
                Console.WriteLine("Device did not stop correctly.");
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop == null)
                return;//nothing to stop
            loop.Dispose();
            ShowMessage(3000, "Device " + id + " Stopped.", "The current device has been stopped.", ToolTipIcon.Info);
        }
        /// <summary>
        /// Event handler for start button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.GlowManager.GlowsFound == 0)
                ShowMessage(3000, "No Devices Found", "No devices were found to start.", ToolTipIcon.Error);
            else
                this.StartCurrent();
        }
        /// <summary>
        /// Stop all found devices
        /// </summary>
        public void StopAll()
        {
            if (this.GlowManager.GlowsFound == 0) {
                ShowMessage(3000, "No Devices Found", "No devices were found to stop.", ToolTipIcon.Error);
                return;
            }
            ShowMessage(3000, "Stopping All", "Extensions Stopping. Please wait.", ToolTipIcon.Info);
            foreach (var dev in this.GlowManager.Glows) {
                if (!dev.Stop())
                    Console.WriteLine("Device did not stop correctly.");
                var loop = this.outManager.FindLoopOrReturnNull(dev.id);
                if (loop != null)
                    loop.Dispose();//stop and dispose if exists
            }
            ShowMessage(3000, "Stopped", "Extensions Stopped.", ToolTipIcon.Info);
        }
        /// <summary>
        /// Event handler for stop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.GlowManager.GlowsFound == 0)
                ShowMessage(3000, "No Devices Found", "No devices were found to stop.", ToolTipIcon.Error);
            else
                this.StopCurrent();
        }
        /// <summary>
        /// Event handler for session switching. Used for handling locking and unlocking of the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason) {
                case SessionSwitchReason.SessionLock:
                    this.StopAll();
                    Console.WriteLine("locked");
                    break;
                case SessionSwitchReason.SessionUnlock:
                    Console.WriteLine("unlocked");
                    StartAllAfterDelay();
                    break;
            }
        }
        /// <summary>
        /// Event handler for PowerModeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            // User is putting the system into standby 
            if (e.Mode == PowerModes.Suspend) {
                this.Off();
                Console.WriteLine("Suspended.");
            }
            // User is putting the system into resume from standby 
            if (e.Mode == PowerModes.Resume) {
                //this.StartAllAfterDelay();
                Console.WriteLine("resumed...ignoring");
            }
        }

        private void StartAllAfterDelay()
        {
            Thread.Sleep(3000);//wait for screen to be available
            this.StartAll();
        }
    }
    /// <summary>
    /// Custom renderer for notifyicon contextMenu
    /// </summary>
    public class CustomRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle item = new Rectangle(new Point(e.ToolStrip.Location.X + e.Item.Bounds.X, e.ToolStrip.Location.Y + e.Item.Bounds.Location.Y), e.Item.Size);
            if (item.Contains(Cursor.Position)) {
                Color c = Color.FromArgb(44, 44, 44);
                Brush brush = new SolidBrush(c);
                Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(brush, rect);
                brush.Dispose();
            }
            else
                base.OnRenderMenuItemBackground(e);
        }
    }
}
