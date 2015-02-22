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

namespace Antumbra.Glow
{
    public partial class AntumbraCore : Form
    {
        private DeviceManager GlowManager;
        private List<SettingsWindow> settingsWindows;
        private OutputLoopManager outManager;
        private const string extPath = "./Extensions/";
        private ExtensionLibrary extLibrary;
        public bool goodStart { get; private set; }//start-up completion status
        /// <summary>
        /// AntumbraCore Constructor
        /// </summary>
        public AntumbraCore()
        {
            this.goodStart = true;
            InitializeComponent();
            try {
                this.extLibrary = new ExtensionLibrary(extPath);
            }
            catch (System.Reflection.ReflectionTypeLoadException e) {
                string msg = "";
                foreach (var err in e.LoaderExceptions)
                    msg += err.Message;
                ShowMessage(10000, "Exception Occured While Loading Extensions", msg, ToolTipIcon.Error);
                this.goodStart = false;
                Thread.Sleep(10000);//wait for message
                return;//skip rest
            }
            this.GlowManager = new DeviceManager(0x16D0, 0x0A85, extPath);//find devices
            this.outManager = new OutputLoopManager();
            foreach (var dev in this.GlowManager.Glows) {//create output loop
                this.outManager.CreateAndAddLoop(GlowManager, dev.id);
                this.toolStripDeviceList.Items.Add(dev);
            }
            this.settingsWindows = new List<SettingsWindow>();
            if (GlowManager.GlowsFound > 0) {//ready first device for output if any are found
                this.toolStripDeviceList.SelectedIndex = 0;
                this.settingsWindows.Add(new SettingsWindow(this.GlowManager.getDevice(0), this.extLibrary, this));
            }
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
            StopAll();
            this.notifyIcon.Visible = false;
            this.contextMenu.Visible = false;
            this.GlowManager.CleanUp();
            foreach (var win in this.settingsWindows)
                win.CleanUp();
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

        public void ShowMessage(int time, string title, string msg, ToolTipIcon icon)//TODO somewhat replace with eventhandler and delegate for showing messages
        {
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
                var loop = this.outManager.FindLoopOrReturnNull(dev.id);
                if (loop == null)
                    loop = this.outManager.CreateAndAddLoop(this.GlowManager, dev.id);
                dev.AttachEventToExtMgr(loop);
                dev.Start();
                loop.Start(dev.settings.weightingEnabled, dev.settings.newColorWeight);
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

        public void Start(int id)
        {
            Stop(id);
            var dev = this.GlowManager.getDevice(id);
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop == null)
                loop = this.outManager.CreateAndAddLoop(this.GlowManager, id);
            dev.AttachEventToExtMgr(loop);
            dev.Start();
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

        public void Stop(int id)
        {
            var dev = this.GlowManager.getDevice(id);
            if (!dev.Stop())
                Console.WriteLine("Device did not stop correctly.");
            var loop = this.outManager.FindLoopOrReturnNull(id);
            if (loop == null)
                return;//nothing to stop
            loop.Dispose();
            ShowMessage(3000, "Device " + id + " Stopped.", "The current device has been stopped.", ToolTipIcon.Info);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.GlowManager.GlowsFound == 0)
                ShowMessage(3000, "No Devices Found", "No devices were found to start.", ToolTipIcon.Error);
            else
                this.StartCurrent();
        }

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

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.GlowManager.GlowsFound == 0)
                ShowMessage(3000, "No Devices Found", "No devices were found to stop.", ToolTipIcon.Error);
            else
                this.StopCurrent();
        }
    }

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
