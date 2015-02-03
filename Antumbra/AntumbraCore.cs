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
        private Color currentColor;//most recent successfully sent set command color
        private ColorPickerDialog picker;
        private byte lastR, lastG, lastB;
        private double hz;
        private SerialConnector serial;//serial connector
        private SettingsWindow settings;//settings window
        public int pollingWidth { get; set; }
        public int pollingHeight { get; set; }
        public int pollingX { get; set; }
        public int pollingY { get; set; }
        public int stepSleep { get; set; }
        public int stepSize { get; set; }
        public int changeThreshold { get; set; }//difference in colors needed to change

        public MEFHelper MEFHelper;

        private GlowDriver GlowDriver;
        private GlowScreenGrabber ScreenGrabber;
        private GlowScreenProcessor ScreenProcessor;
        private List<GlowDecorator> GlowDecorators;//todo convert the system for handeling extensions to ID based determined on startup
        private List<GlowNotifier> GlowNotifiers;

        private DateTime last;

        public AntumbraCore()
        {
            this.serial = new SerialConnector(0x03EB, 0x2040);
            this.serial.setup();
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Visible = false;
            this.lastR = 0;
            this.lastG = 0;
            this.lastB = 0;
            this.hz = 0;
            this.currentColor = Color.Black;//depends on how the Glow starts up
            this.color = Color.Black;
            this.changeThreshold = 3; //see shouldChange(Color, Color) (lower is more sensitive)
            this.pollingWidth = Screen.PrimaryScreen.Bounds.Width;
            this.pollingHeight = Screen.PrimaryScreen.Bounds.Height;
            this.pollingX = 0;//full screen settings
            this.pollingY = 0;
            this.stepSleep = 0;//no sleep
            this.stepSize = 2;
            this.MEFHelper = new MEFHelper("./Extensions/");
            if (this.MEFHelper.didFail()) {
                Console.WriteLine("loading extensions failed. See output above.");
            }
            else {
                this.GlowDriver = this.MEFHelper.GetDefaultDriver();
            }
            this.settings = new SettingsWindow(this);
        }

        public void setDriver(GlowDriver driver)
        {
            this.GlowDriver = driver;
        }

        public string getCurrentDriverName()
        {
            if (null == this.GlowDriver)
                return null;
            return this.GlowDriver.Name;
        }

        public void setScreenGrabber(GlowScreenGrabber screenGrabber)
        {
            this.ScreenGrabber = screenGrabber;
        }

        public string getCurrentScreenGrabberName()
        {
            if (null == this.ScreenGrabber)
                return null;
            return this.ScreenGrabber.Name;
        }

        public void setScreenProcessor(GlowScreenProcessor processor)
        {
            this.ScreenProcessor = processor;
        }

        public string getCurrentScreenProcessorName()
        {
            if (null == this.ScreenProcessor)
                return null;
            return this.ScreenProcessor.Name;
        }

        public void setDecorators(List<GlowDecorator> decorators)
        {
            this.GlowDecorators = decorators;
        }

        public void setNotifiers(List<GlowNotifier> notifiers)
        {
            this.GlowNotifiers = notifiers;
        }

        void AntumbraColorObserver.NewColorAvail(object sender, EventArgs args)
        {
            double time = DateTime.Now.Subtract(this.last).TotalSeconds;
            if (time != 0) {//ignore when zero TODO find why this happens when decorators are toggled
                this.hz = (this.hz * .01)+ ((1.0 / time) * .99);//weighted average giving each new value 1%
                this.hz = (double)Math.Round((decimal)this.hz, 5);
                string newText = this.hz.ToString() + " hz";
                this.Invoke((MethodInvoker)delegate
                {
                    this.settings.speed.Text = newText;
                });
                this.last = DateTime.Now;
            }
            Color newColor = (Color)sender;
            foreach (var decorator in this.GlowDecorators) {
                newColor = decorator.Decorate(newColor);
            }
            SetColorTo(newColor);
        }

        public void SetColorTo(Color newColor)
        {
            changeTo(newColor.R, newColor.G, newColor.B);
            //fade(newColor, this.stepSleep, this.stepSize);//broken
        }

        private void fade(Color newColor, int sleepTime, int stepSize)//probably will become a decorator, either way it's moving out of here soon
        {
            float r = this.lastR;
            float g = this.lastG;
            float b = this.lastB;
            int rSteps = (int)(newColor.R - r) / stepSize;
            bool rDown = false;
            bool gDown = false;
            bool bDown = false;
            if (rSteps < 0) {
                rDown = true;
                rSteps = Math.Abs(rSteps);
            }
            int gSteps = (int)(newColor.G - g) / stepSize;
            if (gSteps < 0) {
                gDown = true;
                gSteps = Math.Abs(gSteps);
            }
            int bSteps = (int)(newColor.B - b) / stepSize;
            if (bSteps < 0) {
                bDown = true;
                bSteps = Math.Abs(bSteps);
            }
            int maxSteps = Math.Max(Math.Max(rSteps,gSteps),bSteps);
            for (int i = 0; i < maxSteps; i++) {
                Console.WriteLine(i + " - " + stepSize + " - " + r + ","+g+","+b + 
                    " - " + rSteps +" - " + gSteps + " - " + bSteps);
                if (rSteps >= i)
                    if (rDown)
                        r -= stepSize;
                    else
                        r += stepSize;
                if (gSteps >= i)
                    if (gDown)
                        g -= stepSize;
                    else
                        g += stepSize;
                if (bSteps >= i)
                    if (bDown)
                        b -= stepSize;
                    else
                        b += stepSize;
                changeTo((byte)r, (byte)g, (byte)b);
                if(sleepTime != 0)
                    Thread.Sleep(sleepTime);
            }
        }

        private void changeTo(byte r, byte g, byte b)
        {
            if (this.serial.send(r, g, b)) {//sucessful send
                updateLast(r, g, b);
                this.updateStatus(2);
            }
            else {
                this.updateStatus(0);//send failed, device is probably dead
                Console.WriteLine("color send failed!");
            }
        }

        public void checkStatus()
        {
            this.updateStatus(this.serial.state);
        }

        private void updateStatus(int status)//0 - dead, 1 - idle, 2 - alive
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
                this.settings.speed.Text = newText;
            });
        }

        private void updateLast(byte r, byte g, byte b)
        {
            this.lastR = r;
            this.lastG = g;
            this.lastB = b;
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
            this.notifyIcon.Visible = false;
            this.contextMenu.Visible = false;
            if (this.settings.Visible)
                this.settings.Close();
            this.settings.Dispose();
            Application.Exit();
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

        private bool verifyExtensionChoices()
        {
            this.notifyIcon.ShowBalloonTip(3000, "Verifying Extensions", "Verifying the chosen extensions.", ToolTipIcon.Info);
            this.settings.UpdateSelections();
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
            return true;
        }

        public void Start()
        {
            if (verifyExtensionChoices()) {
                this.last = DateTime.Now;
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
                }
                else {
                    this.notifyIcon.ShowBalloonTip(3000, "Driver Issue", this.GlowDriver.Name + " reported that it did not start successfully.",
                        ToolTipIcon.Error);
                }
                //TODO start other extensions as well
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Start();
        }

        public void Stop()
        {
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
