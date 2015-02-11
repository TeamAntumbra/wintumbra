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
using Antumbra.Glow.Windows;
using System.Reflection;

namespace Antumbra.Glow
{
    public partial class AntumbraCore : Form, AntumbraColorObserver
    {
        private Color color;//newest generated color for displaying
        private Color weightedColor;
        private SettingsWindow settingsWindow;
        public int pollingWidth { get; set; }//TODO move these into the settings class
        public int pollingHeight { get; set; }
        public int pollingX { get; set; }
        public int pollingY { get; set; }
        public int stepSleep { get; set; }
        public int stepSize { get; set; }
        public int fadeSteps { get; set; }
        public bool fadeEnabled { get; set; }
        public bool weightingEnabled { get; set; }
        public double newColorWeight { get; set; }

        public ExtensionManager ExtensionManager { get; private set; }
        private DeviceManager GlowManager;
        private Task outputLoopTask;
        public double OutputLoopFPS { get { return outputLoopFPS.FPS; } }
        private FPSCalc outputLoopFPS = new FPSCalc();

        public AntumbraCore()
        {
            this.GlowManager = new DeviceManager(this, 0x16D0, 0x0A85);
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Visible = false;
            this.color = Color.Black;
            this.pollingWidth = Screen.PrimaryScreen.Bounds.Width;
            this.pollingHeight = Screen.PrimaryScreen.Bounds.Height;
            this.pollingX = 0;//full screen settings
            this.pollingY = 0;
            this.stepSleep = 0;
            this.stepSize = 2;
            this.fadeSteps = 5;
            this.fadeEnabled = true;
            this.newColorWeight = .05;
            this.weightingEnabled = true;
            this.ExtensionManager = new ExtensionManager(this, "./Extensions/");
            if (this.ExtensionManager.LoadingFailed())
                this.ShowMessage(3000, "Extension Loading Failed",
                    "The Extension Manager reported that loading of one or more extensions failed."
                    + " Please report this with your error log. Thank you.", ToolTipIcon.Error);
            this.settingsWindow = new SettingsWindow(this);
            this.settingsWindow.glowsFound.Text = this.GlowManager.GlowsFound.ToString();
        }

        /// <summary>
        /// Synchronisation object
        /// </summary>
        private object sync = new object();

        private bool _active = false;
        /// <summary>
        /// Setting this to false will stop the output thread
        /// </summary>
        /// <remarks>Thread Safe</remarks>
        public bool Active
        {
            get
            {
                lock (sync)
                    return _active;
            }
            set
            {
                lock (sync)
                    _active = value;
            }
        }

        void AntumbraColorObserver.NewColorAvail(object sender, EventArgs args)
        {
            outputLoopFPS.Tick();
            this.Invoke((MethodInvoker)delegate
            {
                this.settingsWindow.speed.Text = outputLoopFPS.FPS.ToString();
            });
            lock (sync) {
                color = (Color)sender;
            }
        }

        private Color AddColorToWeightedValue(Color newColor)
        {
            if (this.weightedColor == null)
                this.weightedColor = Color.Black;
            double prevWeight = 1.00 - this.newColorWeight;
            int newR = (int)(this.weightedColor.R * prevWeight) + (int)(newColor.R * this.newColorWeight);
            int newG = (int)(this.weightedColor.G * prevWeight) + (int)(newColor.G * this.newColorWeight);
            int newB = (int)(this.weightedColor.B * prevWeight) + (int)(newColor.B * this.newColorWeight);
            newR %= 255;
            newG %= 255;
            newB %= 255;
            this.weightedColor = Color.FromArgb(newR, newG, newB);
            return this.weightedColor;
        }

        private Color Decorate(Color newColor)
        {
            foreach (GlowDecorator decorator in ExtensionManager.ActiveDecorators)//TODO allow config of decorator order or avg their results or something
                newColor = decorator.Decorate(newColor);
            return newColor;
        }

        private void FadeColorTo(Color newColor)
        {
            for (double step = 0.0; step <= 1; step += (1.0 / this.fadeSteps)) {
                Color result = Mixer.Interpolate(newColor, color, step);
                //if (shouldChange(result))
                SetColorTo(result);
               // else
                //    return;//done fading
            }

        }

        private void SetColorTo(Color newColor)//send to all
        {
            for (var i = 0; i < this.GlowManager.GlowsFound; i += 1) {//for all devices
                SetColorTo(newColor, i);
            }
        }

        private void SetColorTo(Color newColor, int index)//send to one
        {
            if (this.weightingEnabled)
                this.SetColorToWithWeighting(newColor, index);
            else
                this.SetColorToWithoutWeighting(newColor, index);
        }

        /// <summary>
        /// Set the Glow Device in mention directly to the passed color without using the weighted average
        /// Only to be used when doing things such as manual setting and turning the light off
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="index"></param>
        private void SetColorToWithoutWeighting(Color newColor, int index)
        {
            newColor = Decorate(newColor);
            this.GlowManager.sendColor(newColor, index);
        }

        private void SetColorToWithoutWeighting(Color newColor)//send to all
        {
            for (var i = 0; i < this.GlowManager.GlowsFound; i += 1)
                SetColorToWithoutWeighting(newColor, i);
        }

        private void SetColorToWithWeighting(Color newColor, int index)
        {
            newColor = Decorate(newColor);
            newColor = AddColorToWeightedValue(newColor);
            this.GlowManager.sendColor(newColor, index);
        }

        public void updateStatusText(int status)
        {
            if (null == this.settingsWindow)
                return;
            string newText = "Invalid Status";
            switch (status) {
                case 0:
                    newText = "Sending/Recieving Successfully";
                    break;
                case 1:
                    newText = "Glow Device Disconnected";
                    break;
                case 2:
                    newText = "LibAntumbra Memory Allocation Failed";
                    break;
                case 3:
                    newText = "LibUSB Exception";
                    break;
                case 4:
                    newText = "Device in Invalid State for Operation";
                    break;
                case 5:
                    newText = "Index or Size Out of Range";
                    break;
                case 6:
                    newText = "Protocol Command Not Supported";
                    break;
                case 7:
                    newText = "Command Failure";
                    break;
                case 8:
                    newText = "Protocol Error";
                    break;
            }
            this.Invoke((MethodInvoker)delegate
            {
                this.settingsWindow.glowStatus.Text = newText;
            });
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenu.Show(Cursor.Position);
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            this.settingsWindow.updateValues();
            this.settingsWindow.Visible = true;
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            this.notifyIcon.Visible = false;
            this.contextMenu.Visible = false;
            if (this.settingsWindow.Visible)
                this.settingsWindow.Close();
            this.settingsWindow.Dispose();
            Application.Exit();
        }

        private void whatsMyConfig_Click(object sender, EventArgs e)
        {
            AnnounceConfig();
        }

        public void AnnounceConfig()
        {
            string driver = "Not set";
            string grabber = "Not set";
            string processor = "Not set";
            string decorators = "Not set";
            string notifiers = "Not set";
            if (ExtensionManager.ActiveDriver != null)
                driver = ExtensionManager.ActiveDriver.ToString();
            if (ExtensionManager.ActiveGrabber != null)
                grabber = ExtensionManager.ActiveGrabber.ToString();
            if (ExtensionManager.ActiveProcessor != null)
                processor = ExtensionManager.ActiveProcessor.ToString();
            GlowDecorator[] decs = ExtensionManager.ActiveDecorators.ToArray();
            for (int i = 0; i < decs.Length; i += 1) {
                if (i == 0)//reset string
                    decorators = "";
                decorators += decs[i].ToString();
                if (i != decs.Length - 1)//not last
                    decorators += ", ";
            }
            GlowNotifier[] notfs = ExtensionManager.ActiveNotifiers.ToArray();
            for (int i = 0; i < notfs.Length; i += 1) {
                if (i == 0)//reset string
                    notifiers = "";
                notifiers += notfs[i].ToString();
                if (i != notfs.Length - 1)//not last
                    notifiers += ", ";
            }
            this.notifyIcon.ShowBalloonTip(5000, "Current Configuration", "Driver: " + driver +
                "\nGrabber: " + grabber + "\nProcessor: " + processor +
                "\nDecorators: " + decorators + "\nNotifiers: " + notifiers,
                ToolTipIcon.Info);
        }

        public void ShowMessage(int time, string title, string msg, ToolTipIcon icon)
        {
            this.notifyIcon.ShowBalloonTip(time, title, msg, icon);
        }

        public void Off()
        {
            this.Stop();
            this.SetColorToWithoutWeighting(Color.Black);
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Off();
        }

        private void contextMenu_MouseLeave(object sender, EventArgs e)
        {
            contextMenu.Close();
        }

        public void Start()
        {
            Stop();
            this.outputLoopFPS = new FPSCalc();
            if (ExtensionManager.Start()) {
                this.settingsWindow.updateValues();
                ExtensionManager.ActiveDriver.AttachEvent(this);
                this.Active = true;
                outputLoopTask = Task.Factory.StartNew(outputLoopTarget);
            }
        }

        private void outputLoopTarget()
        {
            try {
                while (Active) {
                    this.settingsWindow.updateSwatch(color);
                    if (fadeEnabled)
                        FadeColorTo(color);
                    else
                        SetColorTo(color);
                }
            }
            catch (Exception e) {
                lock (sync) {
                    Active = false;
                    Console.WriteLine("Exception in outputLoopTarget: " + e.Message);
                }
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Start();
        }

        public void Stop()
        {
            this.Active = false;
            if (this.outputLoopTask != null) {
                this.outputLoopTask.Wait(3000);
                if (this.outputLoopTask.IsCompleted || this.outputLoopTask.IsCanceled)
                    this.outputLoopTask.Dispose();
                this.outputLoopTask = null;
            }
            ExtensionManager.Stop();
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
