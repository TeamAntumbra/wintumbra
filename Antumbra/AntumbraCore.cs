﻿using System;
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
    public partial class AntumbraCore : MetroFramework.Forms.MetroForm, AntumbraColorObserver//, IObserver<Color>, IObserver<Notification>
        //Main driver for application
    {
        //private System.Timers.Timer screenTimer;//timer for screen color averaging
        //private Thread fadeThread;//thread for color fades
        private Color color;//newest generated color for displaying
        private Color currentColor;//most recent successfully sent set command color
        private ColorPickerDialog picker;
        //private Thread driverThread;
        private bool fadeEnabled;
        private bool screenAvgEnabled;
        public bool gameMode { get; set; }
        private byte lastR, lastG, lastB;
        private int changeThreshold; //difference in colors needed to change
        //bool on;
        private SerialConnector serial;//serial connector
        private SettingsWindow settings;//settings window
        //public AntumbraScreenGrabber screenGrabber { get; set; }
        //public AntumbraDirectXScreenGrabber gameScreenGrabber { get; set; }
        //public AntumbraScreenProcessor screenProcessor { get; set; }
        public int pollingWidth { get; set; }
        public int pollingHeight { get; set; }
        public int pollingX { get; set; }
        public int pollingY { get; set; }
        public int colorFadeStepSleep { get; set; }
        public int manualStepSleep { get; set; }
        public int sinFadeStepSleep { get; set; }
        public double sinFadeStepSize { get; set; }//out of pi (for half a sin curve with the light on, half off)
        public int HSVstepSleep { get; set; }
        public int HSVstepSize { get; set; }
        public int colorFadeStepSize { get; set; }
        public int manualStepSize { get; set; }
        public int screenPollingWait { get; set; }
        public int screenAvgStepSleep { get; set; }
        public int screenAvgStepSize { get; set; }
        public int maxBrightness { get; set; }//0-100 scale
        public int minBrightness { get; set; }//0-100 scale
        public int warmth { get; set; }//0-100 scale

        private MEFHelper MEFHelper;

        private GlowDriver GlowDriver;
        private GlowScreenGrabber ScreenGrabber;
        private GlowScreenProcessor ScreenProcessor;
        private List<GlowDecorator> GlowDecorators;
        private List<GlowNotifier> GlowNotifiers;

        //private Thread MainDriverThread;//main driver thread for whole application
        private Thread DriverThread;//driver thread for driver extensions

        public AntumbraCore()
        {
            this.serial = new SerialConnector(0x03EB, 0x2040);
            Console.WriteLine(this.serial.setup());//sanity check that Glow connects correctly
            InitializeComponent();
            this.lastR = 0;
            this.lastG = 0;
            this.lastB = 0;
            this.currentColor = Color.Black;//depends on how the Glow starts up
            this.color = Color.Black;
            this.changeThreshold = 10; //see shouldChange(Color, Color) (lower is more sensitive)
            //this.continuous = false;
            this.fadeEnabled = false;
            this.gameMode = false;
            //this.fadeThread = new Thread(new ThreadStart(callColorFade));
            //this.screenTimer = new System.Timers.Timer();
            //this.pollingWidth = this.screen.width;
            //this.pollingHeight = this.screen.height;
            this.pollingWidth = Screen.PrimaryScreen.Bounds.Width;
            this.pollingHeight = Screen.PrimaryScreen.Bounds.Height;
            this.pollingX = 0;
            this.pollingY = 0;
            this.HSVstepSleep = 15;
            this.colorFadeStepSleep = 15;
            this.manualStepSleep = 1;
            this.sinFadeStepSleep = 3;
            this.sinFadeStepSize = .01;
            this.screenPollingWait = 33;//default is 33ms, ~30hz            TODO remove
            this.HSVstepSize = 1;
            this.manualStepSize = 1;
            this.colorFadeStepSize = 1; //default step sizes to 1
            this.screenAvgStepSleep = 0;
            this.screenAvgStepSize = 2;
            updateStatus(this.serial.state);
            //this.picker = new ColorPickerDialog(); //TODO investigate crash with color picker
            //this.screenGrabber = new AntumbraScreenGrabber(this);
            //this.gameScreenGrabber = new AntumbraDirectXScreenGrabber(this, this.pollingX, this.pollingY,
            //    this.pollingWidth, this.pollingHeight, 0);//TODO make timeOut a setting
            //this.screenProcessor = new AntumbraScreenProcessor(.45, true, 20, 20);
            //this.driverThread = new Thread(new ThreadStart(setToAvg));
            this.maxBrightness = 100;
            this.minBrightness = 0;
            this.warmth = 0;
            //this.settings = new SettingsWindow(this);
            this.DriverThread = new Thread(new ThreadStart(run));
            this.MEFHelper = new MEFHelper("./Extensions/");
            this.ScreenGrabber = this.MEFHelper.GetDefaultScreenDriver();
            this.ScreenProcessor = this.MEFHelper.GetDefaultScreenProcessor();
            this.GlowDriver = new GlowScreenDriverCoupler(this, this.ScreenGrabber, this.ScreenProcessor);
            //this.GlowDriver = this.MEFHelper.GetDefaultDriver();
            this.GlowDriver.AttachEvent(this);
            //this.GlowDriver.Subscribe(this);
        }

      /*  public void HandleNewColor(object sender, EventArgs args)
        {
            if (sender is Color)
                this.SetColorTo((Color)sender);
        }

        public void HandleNotification(object sender, EventArgs args)
        {
            if (sender is GlowNotifier)
                ((GlowNotifier)sender).Notify(this);//TODO change this to something less powerful
        }*/

 /*       public void OnCompleted()//extension has signaled it is done giving output
        {
            //do nothing
        }

        public void OnNext(Notification noti)
        {
            Console.WriteLine("Notification Recieved: " + noti.Name);
        }

        public void OnNext(Color newColor)
        {
            this.changeTo(newColor.R, newColor.G, newColor.B);
            //this.fade(newColor, 0, 2);
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Exception: " + error.ToString());
        }*/

        void AntumbraColorObserver.NewColorAvail(object sender, EventArgs args)
        {
            SetColorTo((Color)sender);
        }

        public void run()
        {
            while (true) {
                //do stuff
            }
        }

        public void SetColorTo(Color newColor)
        {
            //Console.WriteLine(newColor.ToString());
            //fade(newColor, 0, 2);
            byte r = newColor.R;
            byte g = newColor.G;
            byte b = newColor.B;
            changeTo(r, g, b);
        }

        public int getPollingWidth()
        {
            return this.pollingWidth;
        }

        public int getPollingHeight()
        {
            return this.pollingHeight;
        }

        /*private void setToAvg()
        {
            while (true) {
                if (this.screenGrabber.screen == null)//skip
                    continue;
                Color newColor = this.screenProcessor.Process(this.screenGrabber.screen);
                if (newColor.Equals(Color.Empty))//something went wrong
                    continue;
                //fade(newColor, this.screenAvgStepSleep, this.screenAvgStepSize);//fade
                Console.WriteLine(newColor.ToString());
            }
        }

        private void setToGameAvg()
        {
            while (true) {
                Color newColor = this.screenProcessor.Process(this.gameScreenGrabber.screen);
                if (newColor.Equals(Color.Empty))//something went wrong
                    return;
                fade(newColor, this.screenAvgStepSleep, this.screenAvgStepSize);//fade
            }
        }*/
        
        private int calcDiff(Color color, Color other)
        {
            int r1 = color.R;
            int g1 = color.G;
            int b1 = color.B;
            int r2 = other.R;
            int g2 = other.G;
            int b2 = other.B;
            int total = 0;//represents the total difference
            total += Math.Abs(r1 - r2);
            total += Math.Abs(g1 - g2);
            total += Math.Abs(b1 - b2);
            return total;
        }

        private bool shouldChange(Color color, Color other)
        {
            return calcDiff(color, other) > this.changeThreshold;
        }

        private void callColorFade()
        {
            while (true)
                colorFade(this.colorFadeStepSleep);
        }

        private void colorFade(int sleep)
        {
            Random rnd = new Random();
            while(true)
                fade(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), sleep, this.colorFadeStepSize);
        }

        private void callSinFade()
        {
            while (true)
                sinFade(this.sinFadeStepSleep, this.sinFadeStepSize);
        }

        private void sinFade(int sleepTime, double stepSize)
        {
            for (double i = 0; i < Math.PI*2; i += stepSize)
            {
                if (i <= Math.PI) {//in positive half
                    byte byte_i = (byte)(Math.Sin(i) * 255);
                    changeTo(byte_i, byte_i, byte_i);
                }
                Thread.Sleep(sleepTime);
            }
        }

        private void callHsvFade()
        {
            while (true)
                hsvFade(this.HSVstepSleep);
        }

        private void hsvFade(int stepSleep)
        {
            double s = 100;
            double v = 100;
            for (double h = 0; h <= 360; h++)
            {
                int[] rgb = HSVRGGConverter.HSVToRGB(h, s, v);
                fade(Color.FromArgb(rgb[0], rgb[1], rgb[2]), stepSleep, this.HSVstepSize);
            }
        }

        private Color decorate(Color newColor)//'decorate' color to conform to brightness and warmth settings
        {
            int minModifier = (int)(2.55 * this.minBrightness);//min directly adds percentage of color
            double maxBrightnessModifier = this.maxBrightness/100.0;//max reduces color proportional to its value
            int r = (int)(newColor.R * maxBrightnessModifier);
            int g = (int)(newColor.G * maxBrightnessModifier);
            int b = (int)(newColor.B * maxBrightnessModifier);
            r += minModifier;
            g += minModifier;
            b += minModifier;
            if (r > 255)
                r = 255;
            if (g > 255)
                g = 255;
            if (b > 255)
                b = 255;
            return Color.FromArgb(r, g, b);
        }

        private Color addWarmth(Color newColor)//'decorate' color to conform to warmth setting
        {
            int rWarm = 255;
            int gWarm = 190;
            int bWarm = 0;
            int r = newColor.R;
            int g = newColor.G;
            int b = newColor.B;
            r = ((100 - warmth) * r + warmth * rWarm) / (100);
            g = ((100 - warmth) * g + warmth * gWarm) / (100);
            b = ((100 - warmth) * b + warmth * bWarm) / (100);
            return Color.FromArgb(r, g, b);
        }

        private void fade(Color newColor, int sleepTime, int stepDivider)
        {
            if (!shouldChange(Color.FromArgb(this.lastR, this.lastG, this.lastB), newColor))
                return;//no update needed*/
            float r = this.lastR;
            float g = this.lastG;
            float b = this.lastB;
            int diff = calcDiff(Color.FromArgb((int)r,(int)g,(int)b), newColor);
            int steps = diff / 3 / stepDivider;
            if (steps <= 0)
                steps = 1;
            int stepSize = diff / steps;
            float rStep = (newColor.R - r) / steps;
            float gStep = (newColor.G - g) / steps;
            float bStep = (newColor.B - b) / steps;
            for (int i = 0; i < steps; i++) {
                r += rStep;
                g += gStep;
                b += bStep;
                changeTo((byte)r, (byte)g, (byte)b);
                if(sleepTime != 0)
                    Thread.Sleep(sleepTime);
            }
        }

        private void changeTo(byte r, byte g, byte b)
        {
            //Console.WriteLine(System.DateTime.Now.ToString());
            //Console.WriteLine(r + " " + g + " " + b);
            if (this.serial.send(r, g, b))//sucessful send
                updateLast(r, g, b);
            else {
                this.updateStatus(0);//send failed, device is probably dead
                Console.WriteLine("color send failed!");
            }
            //Console.WriteLine(System.DateTime.Now.ToString() + "        after");
        }

        private void updateStatus(int status)//0 - dead, 1 - idle, 2 - alive
        {
            switch(status) {
                case 0:
                    //dead
                    break;
                case 1:
                    //idle
                    break;
                case 2:
                    //good
                    break;
                default:
                    Console.WriteLine("This should not happen. updateStatus");
                    break;
            }
        }

        private void updateLast(byte r, byte g, byte b)
        {
            //this.antumbraLabel.ForeColor = Color.FromArgb(r, g, b);
            this.lastR = r;
            this.lastG = g;
            this.lastB = b;
        }

        private void Antumbra_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                //this.icon.Visible = true;
                //this.icon.ShowBalloonTip(3500);
                notifyIcon.ShowBalloonTip(3000);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                //this.icon.Visible = false;
            }
        }

        public void updatePollingBounds(int x, int y)
        {
            if (x <= 0 || y <= 0)
                return;//invalid
            this.pollingWidth = x;
            this.pollingHeight = y;
        }

        public void updatePollingBoundsToFull()//assumes primary monitor
        {
            updatePollingBounds(Screen.PrimaryScreen.Bounds.Width, 
                                Screen.PrimaryScreen.Bounds.Height);
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenu.Show(Cursor.Position);
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible) {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            this.settings.updateValues();
            this.settings.Visible = true;
        }

        private void HSVMenuItem_Click(object sender, EventArgs e)
        {
      /*      if (this.screenAvgEnabled)
                this.driverThread.Abort();
            this.screenAvgEnabled = false;
            //this.screenTimer.Enabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            this.fadeThread = new Thread(new ThreadStart(callHsvFade));
            this.fadeThread.Start();
            this.fadeEnabled = true;*/
        }

        private void randomColorFadeMenuItem_Click(object sender, EventArgs e)
        {
  /*          if (this.screenAvgEnabled)
                this.driverThread.Abort();
            this.screenAvgEnabled = false;
            //this.screenTimer.Enabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            this.fadeThread = new Thread(new ThreadStart(callColorFade));
            this.fadeThread.Start();
            this.fadeEnabled = true; */
        }

        private void screenResponsiveMenuItem_Click(object sender, EventArgs e)
        {
            //if (this.fadeEnabled)
            //    this.fadeThread.Abort();
            //this.fadeEnabled = false;
            //this.screenTimer = new System.Timers.Timer(this.screenPollingWait);//10 hz
            //this.screenTimer.Elapsed += new System.Timers.ElapsedEventHandler(callSetAvg);
            //this.screenTimer.Enabled = true;
            //this.screenAvgEnabled = true;
            //if (this.driverThread.IsAlive)
            //    this.driverThread.Abort();//kill any existing screenThread
           /* if (this.DriverThread != null && this.DriverThread.IsAlive)
                this.DriverThread.Abort();
            this.DriverThread = new Thread(new ThreadStart(this.GlowScreenDriver.captureTarget));
            this.DriverThread.Start();*/
            //if (gameMode) {
            //    this.driverThread = null;
            //    this.gameScreenGrabber.start(this.settings.getGameModeProcess());
            //}
            //else {
            //    this.screenGrabber.start();
            //    this.driverThread = new Thread(new ThreadStart(setToAvg));
            //    this.driverThread.Start();
            //}
            //this.DriverThread.Start();
            if (this.GlowDriver.ready())//ready?
                this.GlowDriver.start();//lets go!
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
          /*  if (this.fadeEnabled)
                this.fadeThread.Abort();
            if (this.screenAvgEnabled)
                this.driverThread.Abort();
            this.driverThread.Abort();*/
            if (this.DriverThread.IsAlive)
                this.DriverThread.Abort();
            //if (this.MainDriverThread.IsAlive)
            //    this.MainDriverThread.Abort();
            this.notifyIcon.Visible = false;
            this.contextMenu.Visible = false;
            //this.settings.Close();
            //this.settings.Dispose();
            Application.Exit();
        }

        private void sinWaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        /*    if (this.screenAvgEnabled)
                this.driverThread.Abort();
            this.screenAvgEnabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            //this.screenTimer.Enabled = false;
            this.fadeThread = new Thread(new ThreadStart(callSinFade));
            this.fadeThread.Start();
            this.fadeEnabled = true;*/
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
   /*         if (this.screenAvgEnabled)
                this.driverThread.Abort();
            this.screenAvgEnabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            //this.screenTimer.Enabled = false;
            this.fadeThread.Abort();
            this.fadeEnabled = false;
            changeTo(0, 0, 0);*/
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
   /*         if (this.fadeEnabled)
                this.fadeThread.Abort();
            if (this.screenAvgEnabled)
                this.driverThread.Abort();
            this.screenAvgEnabled = false;
            this.fadeEnabled = false;
            //this.screenTimer.Enabled = false;
            this.picker = new ColorPickerDialog();
            this.picker.Show();
            this.picker.previewPanel.BackColorChanged += new EventHandler(manualListener);
            fade(this.picker.previewPanel.BackColor, this.manualStepSleep, this.manualStepSize); */
        }

        private void manualListener(object sender, EventArgs e)
        {
            this.SetColorTo(this.picker.previewPanel.BackColor);
        }

        private void contextMenu_MouseLeave(object sender, EventArgs e)
        {
            contextMenu.Close();
        }
    }
}