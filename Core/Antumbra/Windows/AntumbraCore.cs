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
        private Color prevColor;
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
        public int changeThreshold { get; set; }//difference in colors needed to change

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
            this.prevColor = Color.Black;
            this.color = Color.Black;
            this.changeThreshold = 3;
            this.pollingWidth = Screen.PrimaryScreen.Bounds.Width;
            this.pollingHeight = Screen.PrimaryScreen.Bounds.Height;
            this.pollingX = 0;//full screen settings
            this.pollingY = 0;
            this.stepSleep = 0;
            this.stepSize = 2;
            this.fadeSteps = 5;
            this.fadeEnabled = true;
            this.ExtensionManager = new ExtensionManager(this, "./Extensions/");
            this.settingsWindow = new SettingsWindow(this);
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

        private Color Interpolate(Color color1, Color color2, double fraction)
        {
            double r = Interpolate(color1.R, color2.R, fraction);
            double g = Interpolate(color1.G, color2.G, fraction);
            double b = Interpolate(color1.B, color2.B, fraction);
            int rI = (int)Math.Round(r) % 255;
            int gI = (int)Math.Round(g) % 255;
            int bI = (int)Math.Round(b) % 255;
            if (rI < 0)
                rI = 0;
            if (gI < 0)
                gI = 0;
            if (bI < 0)
                bI = 0;
            return Color.FromArgb(rI, gI, bI);
        }

        private double Interpolate(double d1, double d2, double fraction)
        {
            return d1 + (d1 - d2) * fraction;
        }

        private Color AddColorToWeightedValue(Color newColor)
        {
            if (this.weightedColor == null)
                this.weightedColor = Color.Black;
            int newR = (int)(this.weightedColor.R * .8) + (int)(newColor.R * .2);
            int newG = (int)(this.weightedColor.G * .8) + (int)(newColor.G * .2);
            int newB = (int)(this.weightedColor.B * .8) + (int)(newColor.B * .2);
            newR %= 255;
            newG %= 255;
            newB %= 255;
            this.weightedColor = Color.FromArgb(newR, newG, newB);
            return this.weightedColor;
        }

        private void FadeColorTo(Color newColor)
        {
            for (double step = 0.0; step <= 1; step += (1.0 / this.fadeSteps)) {
                Color result = Interpolate(newColor, color, step);
                if (shouldChange(result))
                    SetColorTo(result);
                else
                    return;//done
            }

        }

        private void SetColorTo(Color newColor)//TODO move to device connection class NOTE always use this to set color and allow decorators to run
        {
            //foreach (GlowDecorator decorator in GlowDecorators)
             //   newColor = decorator.Decorate(newColor);
            foreach (GlowDecorator decorator in ExtensionManager.ActiveDecorators)
                newColor = decorator.Decorate(newColor);
            newColor = AddColorToWeightedValue(newColor);
            this.settingsWindow.updateSwatch(newColor);
            changeTo(newColor.R, newColor.G, newColor.B);
        }

        private void changeTo(byte r, byte g, byte b)
        {
            //TODO
        }
      
        public void checkStatus()//TODO move to device connection class
        {
            //updateStatus(this.serial.state);
        }
        
        public void updateStatus(int status)//0 - dead, 1 - idle, 2 - alive
        {
            if (null == this.settingsWindow)
                return;
            string newText = "Invalid Status";
            switch(status) {
                case 0:
                    newText = "No Glow Found";
                    //dead
                    break;
                case 1:
                    newText = "Idle";
                    //idle
                    break;
                case 2:
                    newText = "Sending/Recieving Successfully";
                    //good
                    break;
            }
            this.Invoke((MethodInvoker)delegate
            {
                this.settingsWindow.glowStatus.Text = newText;
            });
        }

        public void updateStatusText(int status)
        {
            if (null == this.settingsWindow)
                return;
            string newText = "Invalid Status";
            switch (status) {
                case 0:
                    newText = "No Glow Found";
                    //dead
                    break;
                case 1:
                    newText = "Idle";
                    //idle
                    break;
                case 2:
                    newText = "Sending/Recieving Successfully";
                    //good
                    break;
            }
            this.Invoke((MethodInvoker)delegate
            {
                this.settingsWindow.glowStatus.Text = newText;
            });
        }

        private void updateLast(byte r, byte g, byte b)
        {
            this.prevColor = Color.FromArgb(r, g, b);
        }

        private void updateLast(Color last)
        {
            this.prevColor = last;
        }

        private bool shouldChange(Color newColor)
        {
            int diff = 0;
            diff += Math.Abs(prevColor.R - newColor.R);
            diff += Math.Abs(prevColor.G - newColor.G);
            diff += Math.Abs(prevColor.B - newColor.B);
            return diff > changeThreshold;
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
            this.SetColorTo(Color.Black);
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
                ExtensionManager.ActiveDriver.AttachEvent(this);
                this.Active = true;
                outputLoopTask = Task.Factory.StartNew(outputLoopTarget);
            }
        }

        private void outputLoopTarget()
        {
            try {
                while (Active) {
                    if (shouldChange(color)) {
                        if (fadeEnabled)
                            FadeColorTo(color);
                        else
                            SetColorTo(color);
                    }
                    //Task.Delay(5);
                }
            }
            catch (Exception e) {
                lock (sync) {
                    Active = false;
                    Console.WriteLine("Exception in outputLoopTarget: " + e.Message);
                }
            }
            finally {
                this.Stop();
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Start();
        }

        public void Stop()
        {
            this.Active = false;
            if (this.outputLoopFPS != null)
                this.outputLoopFPS = null;
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
