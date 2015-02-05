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
    public partial class AntumbraCore : MetroFramework.Forms.MetroForm, AntumbraColorObserver
    {
        private Color color;//newest generated color for displaying
        private Color prevColor;
        private Color weightedColor;
        //private SerialConnector serial;//serial connector
        private SettingsWindow settings;//settings window
        public int pollingWidth { get; set; }
        public int pollingHeight { get; set; }
        public int pollingX { get; set; }
        public int pollingY { get; set; }
        public int stepSleep { get; set; }
        public int stepSize { get; set; }
        public int fadeSteps { get; set; }
        public bool fadeEnabled { get; set; }
        public int changeThreshold { get; set; }//difference in colors needed to change

        public MEFHelper MEFHelper;
        private DeviceManager GlowManager;

        private GlowDriver GlowDriver;
        private GlowScreenGrabber ScreenGrabber;
        private GlowScreenProcessor ScreenProcessor;
        private List<GlowDecorator> GlowDecorators;//TODO convert the system for handeling extensions to ID based determined on startup
        private List<GlowNotifier> GlowNotifiers;
        private Task outputLoopTask;
        public double OutputLoopFPS { get { return outputLoopFPS.FPS; } }
        private FPSCalc outputLoopFPS = new FPSCalc();

        public AntumbraCore()
        {
            //this.serial = new SerialConnector(0x03EB, 0x2040);
            //this.serial.setup();
            this.GlowManager = new DeviceManager(this, 0x03EB, 0x2040);
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Visible = false;
            this.prevColor = Color.Black;
            this.color = Color.Black;
            this.changeThreshold = 3; //see shouldChange(Color, Color) (lower is more sensitive)
            this.pollingWidth = Screen.PrimaryScreen.Bounds.Width;
            this.pollingHeight = Screen.PrimaryScreen.Bounds.Height;
            this.pollingX = 0;//full screen settings
            this.pollingY = 0;
            this.stepSleep = 0;//no step sleep TODO turn this into output throttling
            this.stepSize = 2;
            this.fadeSteps = 10;
            this.fadeEnabled = true;
            this.MEFHelper = new MEFHelper("./Extensions/");//TODO move to extension manager class
            if (this.MEFHelper.didFail()) {
                Console.WriteLine("loading extensions failed. See output above.");
            }
            else {
                this.GlowDriver = this.MEFHelper.GetDefaultDriver();
            }
            this.settings = new SettingsWindow(this);
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

        public void setDriver(GlowDriver driver)//TODO move to extension manager class
        {
            this.GlowDriver = driver;
        }

        public string getCurrentDriverName()//TODO move to extension manager class
        {
            if (null == this.GlowDriver)
                return null;
            return this.GlowDriver.Name;
        }

        public void setScreenGrabber(GlowScreenGrabber screenGrabber)//TODO move to extension manager class
        {
            this.ScreenGrabber = screenGrabber;
        }

        public string getCurrentScreenGrabberName()//TODO move to extension manager class
        {
            if (null == this.ScreenGrabber)
                return null;
            return this.ScreenGrabber.Name;
        }

        public void setScreenProcessor(GlowScreenProcessor processor)//TODO move to extension manager class
        {
            this.ScreenProcessor = processor;
        }

        public string getCurrentScreenProcessorName()//TODO move to extension manager class
        {
            if (null == this.ScreenProcessor)
                return null;
            return this.ScreenProcessor.Name;
        }

        public void setDecorators(List<GlowDecorator> decorators)//TODO move to extension manager class
        {
            this.GlowDecorators = decorators;
        }

        public void setNotifiers(List<GlowNotifier> notifiers)//TODO move to extension manager class
        {
            this.GlowNotifiers = notifiers;
        }

        void AntumbraColorObserver.NewColorAvail(object sender, EventArgs args)
        {
            outputLoopFPS.Tick();
            lock (sync) {
                color = (Color)sender;
                foreach (GlowDecorator decorator in GlowDecorators)
                        color = decorator.Decorate(color);
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
                Color result = Interpolate(newColor, prevColor, step);
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
            newColor = AddColorToWeightedValue(newColor);
            changeTo(newColor.R, newColor.G, newColor.B);
        }

        private void changeTo(byte r, byte g, byte b)
        {
            
        }
  /*          Console.WriteLine(r + " - " + g + "  -  " + b);
            if (this.serial.send(r, g, b)) {//sucessful send
                updateLast(r, g, b);
                this.updateStatus(2);
            }
            else {
                this.updateStatus(0);//send failed, device is probably dead / not connected
                Console.WriteLine("color send failed!");
            }
        }

        public void checkStatus()//TODO move to device connection class
        {
            //updateStatus(this.serial.state);
        }
        */
        public void updateStatus(int status)//0 - dead, 1 - idle, 2 - alive
        {
            if (null == this.settings)
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
                this.settings.glowStatus.Text = newText;
            });
        }

        public void updateStatusText(int status)
        {
            if (null == this.settings)
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
                this.settings.glowStatus.Text = newText;
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
            this.settings.updateValues();
            this.settings.Visible = true;
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
            this.notifyIcon.Visible = false;
            this.contextMenu.Visible = false;
            if (this.settings.Visible)
                this.settings.Close();
            this.settings.Dispose();
            Application.Exit();
        }

        private void whatsMyConfig_Click(object sender, EventArgs e)
        {
            string driver = "Not set";
            string grabber = "Not set";
            string processor = "Not set";
            string decorators = "Not set";
            string notifiers = "Not set";
            if (this.GlowDriver != null)
                driver = this.GlowDriver.ToString();
            if (this.ScreenGrabber != null)
                grabber = this.ScreenGrabber.ToString();
            if (this.ScreenProcessor != null)
                processor = this.ScreenProcessor.ToString();
            if (null != this.GlowDecorators && this.GlowDecorators.Count != 0) {
                decorators = "";
                foreach (var decorator in this.GlowDecorators)
                    decorators += decorator.ToString();
            }
            if (null != this.GlowNotifiers && this.GlowNotifiers.Count != 0) {
                notifiers = "";
                foreach (var notifier in this.GlowNotifiers)
                    notifiers += notifier.ToString();
            }

            this.notifyIcon.ShowBalloonTip(5000, "Current Configuration", "Driver: " + driver +
                "\nGrabber: " + grabber + "\nProcessor: " + processor +
                "\nDecorators: " + decorators + "\nNotifiers: " + notifiers,
                ToolTipIcon.Info);
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

        private bool verifyExtensionChoices()//TODO move to extension manager class
        {
            this.settings.UpdateSelections();
            this.notifyIcon.ShowBalloonTip(3000, "Verifying Extensions", "Verifying the chosen extensions.", ToolTipIcon.Info);
            if (null == this.GlowDriver) {
                this.GlowDriver = this.MEFHelper.GetDriver("Screen Driver Coupler");//default to coupler
            }
            if (this.GlowDriver is GlowScreenDriverCoupler) {//screen based driver selected
                if (null == this.ScreenGrabber || null == this.ScreenProcessor) {//no grabber or processor set
                    this.notifyIcon.ShowBalloonTip(3000, "Driver Issue",
                        "Please select a screen driver and screen processor for the Screen Driver Coupler to use.", ToolTipIcon.Error);
                    return false;
                }
                this.GlowDriver = new GlowScreenDriverCoupler(this, this.ScreenGrabber, this.ScreenProcessor);
                this.notifyIcon.ShowBalloonTip(3000, "Screen Based Driver Found", "A screen based driver was found: " +
                    this.GlowDriver.Name + ".", ToolTipIcon.Info);
            }
            else {
                this.notifyIcon.ShowBalloonTip(3000, "Independent Driver Found.", "An independent driver was found: " +
                    this.GlowDriver.Name + ".", ToolTipIcon.Info);
            }
            return true;
        }

        public void Start()
        {
            Stop();
            if (verifyExtensionChoices()) {
                if (this.GlowDriver.Start()) {
                    this.notifyIcon.ShowBalloonTip(3000, "Driver Started", this.GlowDriver.Name + " was started successfully.", ToolTipIcon.Info);
                    this.GlowDriver.AttachEvent(this);
                    foreach (var decorator in this.GlowDecorators) {
                        //this.notifyIcon.ShowBalloonTip(3000, "Decorator Found", decorator.Name + " was found.", ToolTipIcon.Info);
                        decorator.Start();
                    }
                    foreach (var notifier in this.GlowNotifiers) {
                        //this.notifyIcon.ShowBalloonTip(3000, "Notifier Found", notifier.Name + " was found.", ToolTipIcon.Info);
                        notifier.Start();
                    }
                    this.Active = true;
                    outputLoopTask = Task.Factory.StartNew(outputLoopTarget);
                }
                else {
                    this.GlowDriver.Stop();
                    this.notifyIcon.ShowBalloonTip(3000, "Driver Issue", this.GlowDriver.Name + " reported that it did not start successfully.",
                        ToolTipIcon.Error);
                }
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
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.settings.speed.Text = outputLoopFPS.FPS.ToString();
                        });
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
            Active = false;
            if (null != this.GlowDriver) {
                if (this.GlowDriver.Stop())
                    this.notifyIcon.ShowBalloonTip(3000, "Driver Stopped", this.GlowDriver.Name + " was stopped successfully.", ToolTipIcon.Info);
            }
            if (null != this.GlowDecorators)
                foreach (var decorator in this.GlowDecorators)
                    decorator.Stop();
            if (null != this.GlowNotifiers)
                foreach (var notifier in this.GlowNotifiers)
                    notifier.Stop();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Stop();
        }
    }
}
