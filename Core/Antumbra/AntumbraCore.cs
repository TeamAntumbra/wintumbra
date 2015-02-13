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
    public partial class AntumbraCore : Form//, AntumbraColorObserver
    {
        private MEFHelper MEFHelper;
        private DeviceManager GlowManager;
        private SettingsWindow settingsWindow;
        private List<OutputLoop> outLoops;

        public AntumbraCore()
        {
            this.MEFHelper = new MEFHelper("./Extensions/");
            if (this.MEFHelper.failed)
                this.ShowMessage(3000, "Extension Loading Failed",
                    "The Extension Manager reported that loading of one or more extensions failed."
                    + " Please report this with your error log. Thank you.", ToolTipIcon.Error);
            this.GlowManager = new DeviceManager(0x16D0, 0x0A85, MEFHelper);//find devices
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Visible = false;
            this.outLoops = new List<OutputLoop>();
            foreach (var dev in this.GlowManager.Glows) {
                this.outLoops.Add(new OutputLoop(this.GlowManager, dev.id));
                this.toolStripDeviceList.Items.Add(dev);
                this.toolStripDeviceList.SelectedIndex = 0;
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (contextMenu.Visible)
                contextMenu.Hide();
            else
                contextMenu.Show(Cursor.Position);
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            if (this.GlowManager.GlowsFound == 0)
                this.ShowMessage(3000, "No Glow Devices Found",
                    "No Devices were found to edit the settings of.", ToolTipIcon.Info);
            GlowDevice current = (GlowDevice)toolStripDeviceList.SelectedItem;
            this.settingsWindow = new SettingsWindow(current, this);
            this.settingsWindow.Show();
        }

        private void startAllItem_Click(object sender, System.EventArgs e)
        {
            this.StartAll();
        }

        private void stopAllItem_Click(object sender, System.EventArgs e)
        {
            this.StopAll();
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            this.notifyIcon.Visible = false;
            this.contextMenu.Visible = false;
            this.GlowManager.CleanUp();
            if (this.settingsWindow != null)
                this.settingsWindow.CleanUp();
            Application.Exit();
        }

        private void currentOutRateItem_Click(object sender, System.EventArgs e)
        {
            string outSpeeds = "";
            if (outLoops.Count == 0)
                outSpeeds = "No output loops found.";
            else {
                foreach (var loop in outLoops)
                    outSpeeds += "ID: " + loop.id + " - " + Math.Round(loop.FPS, 3) +" hz.\n";
            }
            ShowMessage(3000, "Current Output Speed(s)", outSpeeds, ToolTipIcon.Info);
        }

        private void whatsMyConfig_Click(object sender, EventArgs e)
        {
            AnnounceConfig();
        }

        public void AnnounceConfig()
        {
            ShowMessage(5000, "Current Configurations", this.GlowManager.GetDeviceSetupDecs(), ToolTipIcon.Info);
        }

        public void ShowMessage(int time, string title, string msg, ToolTipIcon icon)
        {
            this.notifyIcon.ShowBalloonTip(time, title, msg, icon);
        }

        public void Off()
        {
            this.StopAll();
            this.GlowManager.sendColor(Color.Black);
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Off();
        }

        public void StartAll()//currently start and stop refers to all devices     TODO change
        {
            StopAll();
            ShowMessage(3000, "Starting All", "Extensions are being started. Please wait.", ToolTipIcon.Info);
            foreach (var dev in this.GlowManager.Glows) {
                this.outLoops.Add(new OutputLoop(this.GlowManager, dev.id));//setup output loop for each Glow
            }
            for (var i = 0; i < this.GlowManager.GlowsFound; i += 1) {//start each output loop
                var loop = this.outLoops.ElementAt(i);
                var dev = this.GlowManager.Glows.ElementAt(i);
                var mgr = dev.extMgr;
                mgr.AttachEvent(loop);
                mgr.Start();
                loop.Start(dev.settings.weightingEnabled, dev.settings.newColorWeight);
            }
            ShowMessage(3000, "Started", "Extensions have been started.", ToolTipIcon.Info);
        }

        /// <summary>
        /// Start only the selected device
        /// </summary>
        public void Start()
        {
            Stop();
            int current = this.settingsWindow.currentDevice.id;
            var dev = this.GlowManager.getDevice(current);
            var loop = new OutputLoop(this.GlowManager, dev.id);
            this.outLoops.Add(loop);
            var mgr = dev.extMgr;
            mgr.AttachEvent(loop);
            mgr.Start();
            loop.Start(dev.settings.weightingEnabled, dev.settings.newColorWeight);
            ShowMessage(3000, "Device " + current + " Started.", "The current device has been started.",
                ToolTipIcon.Info);
        }

        public void Stop()
        {
            int current = this.settingsWindow.currentDevice.id;
            foreach (var loop in this.outLoops)
                if (loop.id == current)
                    loop.Stop();
            var mgr = this.GlowManager.getDevice(current).extMgr;
            mgr.Stop();
            ShowMessage(3000, "Device " + current + " Stopped.", "The current device has been stopped.", ToolTipIcon.Info);
            
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Start();
        }

        public void StopAll()
        {
            ShowMessage(3000, "Stopping All", "Extensions Stopping. Please wait.", ToolTipIcon.Info);
            foreach (var loop in this.outLoops) {//stop outLoops
                loop.Stop();
            }
            this.outLoops = new List<OutputLoop>();
            for (var i = 0; i < this.GlowManager.GlowsFound; i += 1) {
                var mgr = this.GlowManager.Glows.ElementAt(i).extMgr;
                mgr.Stop();
            }
            ShowMessage(3000, "Stopped", "Extensions Stopped.", ToolTipIcon.Info);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Stop();
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
