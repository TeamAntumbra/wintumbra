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

namespace Antumbra
{
    public partial class Antumbra : MetroFramework.Forms.MetroForm
    {
        private System.Timers.Timer screenTimer;//timer for screen color averaging
        private Thread fadeThread;//thread for color fades
        private Color color;//newest generated color for displaying
        private Color currentColor;//most recent successfully sent set command color
        private ColorPickerDialog picker;
        private Thread screenThread;
        //bool continuous;//, serialEnabled;
        bool fadeEnabled;
        bool screenAvgEnabled;
        public bool gameMode { get; set; }
        byte lastR, lastG, lastB;
        int changeThreshold; //difference in colors needed to change
        //bool on;
        private SerialConnector serial;
        public ScreenGrabber screenGrabber { get; set; }
        public ScreenGrabberHelper gameScreenGrabber { get; set; }
        public ScreenProcessor screenProcessor { get; set; }
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

        public Antumbra()
        {
            this.serial = new SerialConnector(0x03EB, 0x2040);
            Console.WriteLine(this.serial.setup());
            InitializeComponent();
            this.lastR = 0;
            this.lastG = 0;
            this.lastB = 0;
            this.currentColor = Color.Black;//depends on how the Antumbra starts up
            this.color = Color.Black;
            this.changeThreshold = 10; //see shouldChange(Color, Color) (lower is more sensitive)
            //this.continuous = false;
            this.fadeEnabled = false;
            this.gameMode = false;
            this.fadeThread = new Thread(new ThreadStart(callColorFade));
            this.screenTimer = new System.Timers.Timer();
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
            this.screenPollingWait = 33;//default is 33ms, ~30hz
            this.HSVstepSize = 1;
            this.manualStepSize = 1;
            this.colorFadeStepSize = 1; //default step sizes to 1
            this.screenAvgStepSleep = 0;
            this.screenAvgStepSize = 2;
            updateStatus(this.serial.state);
            this.picker = new ColorPickerDialog();
            this.screenGrabber = new ScreenGrabber();
            this.gameScreenGrabber = new ScreenGrabberHelper(this.pollingX, this.pollingY,
                this.pollingWidth, this.pollingHeight, 0);//todo make timeOut a setting
            this.screenProcessor = new ScreenProcessor(.45, true, 20, 20);
            this.screenThread = new Thread(new ThreadStart(setToAvg));
        }

        public void setColorTo(Color newColor)
        {
            fade(newColor, this.manualStepSleep, this.manualStepSize);
        }

        private void takeScreenshotBtn_Click(object sender, EventArgs e)
        {
            setToAvg();
        }

        public int getPollingWidth()
        {
            return this.pollingWidth;
        }

        public int getPollingHeight()
        {
            return this.pollingHeight;
        }

        private void setToAvg()
        {
            while (true) {
                if (this.screenGrabber.screen == null)
                    continue;
                Color newColor = this.screenProcessor.process(this.screenGrabber.screen);
                if (newColor.Equals(Color.Empty))//something went wrong
                    continue;
                fade(newColor, this.screenAvgStepSleep, this.screenAvgStepSize);//fade
            }
        }

        private void setToGameAvg()
        {
            while (true) {
                Color newColor = this.screenProcessor.process(this.gameScreenGrabber.screen);
                if (newColor.Equals(Color.Empty))//something went wrong
                    return;
                fade(newColor, this.screenAvgStepSleep, this.screenAvgStepSize);//fade
            }
        }
        
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
                //Console.WriteLine(r + " " + g + ' ' + b + ' ' + steps + ' ' + rStep + ' ' + gStep + ' ' + bStep);
                changeTo((byte)r, (byte)g, (byte)b);
                if(sleepTime != 0)
                    Thread.Sleep(sleepTime);
            }
        }

        private void changeTo(byte r, byte g, byte b)
        {
            if (this.serial.send(r, g, b))//sucessful send
                updateLast(r, g, b);
            else {
                this.updateStatus(0);//send failed, device is probably dead
            }
        }

        private void updateStatus(int status)//0 - dead, 1 - idle, 2 - alive
        {
            switch(status) {
                case 0:
                    this.statusBtn.BackColor = Color.Red;
                    break;
                case 1:
                    this.statusBtn.BackColor = Color.Yellow;
                    break;
                case 2:
                    this.statusBtn.BackColor = Color.Green;
                    break;
                default:
                    Console.WriteLine("This should not happen. updateStatus");
                    break;
            }
        }

        private void updateLast(byte r, byte g, byte b)
        {
            this.antumbraLabel.ForeColor = Color.FromArgb(r, g, b);
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
            Form settings = new SettingsWindow(this);
            settings.Visible = true;
        }

        private void HSVMenuItem_Click(object sender, EventArgs e)
        {
            if (this.screenAvgEnabled)
                this.screenThread.Abort();
            this.screenAvgEnabled = false;
            //this.screenTimer.Enabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            this.fadeThread = new Thread(new ThreadStart(callHsvFade));
            this.fadeThread.Start();
            this.fadeEnabled = true;
        }

        private void randomColorFadeMenuItem_Click(object sender, EventArgs e)
        {
            if (this.screenAvgEnabled)
                this.screenThread.Abort();
            this.screenAvgEnabled = false;
            //this.screenTimer.Enabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            this.fadeThread = new Thread(new ThreadStart(callColorFade));
            this.fadeThread.Start();
            this.fadeEnabled = true;
        }

        private void screenResponsiveMenuItem_Click(object sender, EventArgs e)
        {
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            this.fadeEnabled = false;
            //this.screenTimer = new System.Timers.Timer(this.screenPollingWait);//10 hz
            //this.screenTimer.Elapsed += new System.Timers.ElapsedEventHandler(callSetAvg);
            //this.screenTimer.Enabled = true;
            this.screenAvgEnabled = true;
            if (this.screenThread.IsAlive)
                this.screenThread.Abort();//kill any existing screenThread
            if (gameMode) {
                this.screenThread = null;
                this.gameScreenGrabber.start();
            }
            else {
                this.screenGrabber.start();
                this.screenThread = new Thread(new ThreadStart(setToAvg));
                this.screenThread.Start();
            }
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            if (this.screenAvgEnabled)
                this.screenThread.Abort();
            this.screenThread.Abort();
            this.notifyIcon.Visible = false;
            this.contextMenu.Visible = false;
            Application.Exit();
        }

        private void sinWaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.screenAvgEnabled)
                this.screenThread.Abort();
            this.screenAvgEnabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            //this.screenTimer.Enabled = false;
            this.fadeThread = new Thread(new ThreadStart(callSinFade));
            this.fadeThread.Start();
            this.fadeEnabled = true;
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.screenAvgEnabled)
                this.screenThread.Abort();
            this.screenAvgEnabled = false;
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            //this.screenTimer.Enabled = false;
            this.fadeThread.Abort();
            this.fadeEnabled = false;
            changeTo(0, 0, 0);
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.fadeEnabled)
                this.fadeThread.Abort();
            if (this.screenAvgEnabled)
                this.screenThread.Abort();
            this.screenAvgEnabled = false;
            this.fadeEnabled = false;
            //this.screenTimer.Enabled = false;
            this.picker = new ColorPickerDialog();
            this.picker.Show();
            this.picker.previewPanel.BackColorChanged += new EventHandler(manualListener);
            fade(this.picker.previewPanel.BackColor, this.manualStepSleep, this.manualStepSize);
        }

        private void manualListener(object sender, EventArgs e)
        {
            this.setColorTo(this.picker.previewPanel.BackColor);
        }

        private void contextMenu_MouseLeave(object sender, EventArgs e)
        {
            contextMenu.Close();
        }
    }
}
