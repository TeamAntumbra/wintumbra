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
    public partial class Antumbra : Form
    {
        private System.Windows.Forms.NotifyIcon icon;
        private System.Timers.Timer screenTimer;//timer for screen color averaging
        private Thread fadeThread;//thread for color fades
        private Color color;//newest generated color for displaying
        private Color currentColor;//most recent successfully sent set command color
        bool continuous;//, serialEnabled;
        bool fadeEnabled;
        byte lastR, lastG, lastB;
        int changeThreshold; //difference in colors needed to change
        //bool on;
        private SerialConnector serial;
        private ScreenGrabber screen;
        private int pollingWidth, pollingHeight;

        public Antumbra()
        {
            //installDriver();
            this.serial = new SerialConnector(0x03EB, 0x2040);
            this.screen = new ScreenGrabber();
            Console.WriteLine(this.serial.setup());
            this.icon = new System.Windows.Forms.NotifyIcon();
            this.icon.Icon = Properties.Resources.favicon;
            this.icon.BalloonTipTitle = "Antumbra|Glow";
            this.icon.BalloonTipText = "Click the icon for a menu\nDouble click for to open";
            this.icon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            this.icon.Visible = true;
            InitializeComponent();
            this.lastR = 0;
            this.lastG = 0;
            this.lastB = 0;
            this.currentColor = Color.Black;//depends on how the Antumbra starts up
            this.color = Color.Black;
            this.changeThreshold = 3; //see shouldChange(Color, Color) (lower is more sensitive)
            this.continuous = false;
            this.fadeEnabled = false;
            this.fadeThread = new Thread(new ThreadStart(callColorFade));
            this.screenTimer = new System.Timers.Timer();
            this.modeComboBox.SelectedIndex = 0;
            this.pollingWidth = this.Width;
            this.pollingHeight = this.Height;
        }

        private void takeScreenshotBtn_Click(object sender, EventArgs e)
        {
            setToAvg();
        }

        private void setToAvg()
        {
            Color newColor = this.screen.getScreenAvgColor(this.pollingWidth, this.pollingHeight);
            //Color newColor = this.screen.getCenterScreenAvgColor();
            //Color newColor = this.screen.getScreenDomColor(); //holy shit 95% cpu usage
            //Color newColor = this.screen.getCenterScreenDomColor();
            if (newColor.Equals(Color.Empty))//something went wrong
                return;
            Console.WriteLine("r = " + newColor.R + " g = " + newColor.G + " b = " + newColor.B);
            //changeTo(newColor.R, newColor.G, newColor.B);
            fade(newColor, 0, 1);//fade using a 1-step
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

        private void continuousCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.continuous = !this.continuous;
            if (this.continuous)
            {
                screenTimer = new System.Timers.Timer(5);//200 hz
                screenTimer.Elapsed += new System.Timers.ElapsedEventHandler(callSetAvg);
                screenTimer.Enabled = true;
            }
            else
                screenTimer.Enabled = false;
        }       

        private void callSetAvg(object sender, System.Timers.ElapsedEventArgs e)
        {
            setToAvg();
        }

        private void colorFadeButton_Click(object sender, EventArgs e)
        {
            this.fadeEnabled = !this.fadeEnabled;
            if (this.fadeEnabled)
            {
                try
                {
                    fadeThread.Start();
                }
                catch (System.Threading.ThreadStateException)
                {
                    fadeThread = new Thread(new ThreadStart(callColorFade));
                    fadeThread.Start();
                }
            }
            else
                fadeThread.Abort();//stop fadeThread
        }

        private void callColorFade()
        {
            while (true)
                colorFade();
        }

        private void colorFade()
        {
            Color[] colors = { Color.Red, Color.Orange, Color.Yellow, Color.YellowGreen, Color.Green, Color.Blue, Color.Purple, };
            foreach (Color color in colors)
                fade(color, 0, 1);//color, step sleep time
        }

        private void callSinFade()
        {
            while (true)
                sinFade(20);//TODO make configurable
        }

        private void sinFade(int sleepTime)
        {
            for (double i = 0; i < Math.PI*2; i += .01)
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
                hsvFade();
        }

        private void hsvFade()
        {
            double s = 100;
            double v = 100;
            for (double h = 0; h <= 360; h++)
            {
                int[] rgb = this.HsvToRgb(h, s, v);
                fade(Color.FromArgb(rgb[0], rgb[1], rgb[2]), 10, 1);
            }
        }

        private void fade(Color newColor, int sleepTime, int stepDivider) //TODO: make this smarter 
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
                Thread.Sleep(sleepTime);
            }
        }

        private void changeTo(byte r, byte g, byte b)
        {
            if (this.serial.send(r, g, b))
                updateLast(r, g, b);
            else { }
        }

        private void updateLast(byte r, byte g, byte b)
        {
            this.lastR = r;
            this.lastG = g;
            this.lastB = b;
        }

        private void Antumbra_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.icon.Visible = true;
                this.icon.ShowBalloonTip(3500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                //this.icon.Visible = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            //this.icon.Visible = false;
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String mode = this.modeComboBox.Items[this.modeComboBox.SelectedIndex].ToString();
            if (mode.Equals("Off"))
            {
                this.screenTimer.Enabled = false;
                this.fadeThread.Abort();
                this.fadeEnabled = false;
                changeTo(0, 0, 0);
            }
            else if (mode.Equals("Color Fade"))
            {
                this.screenTimer.Enabled = false;
                if (this.fadeEnabled)
                    this.fadeThread.Abort();
                this.fadeThread = new Thread(new ThreadStart(callColorFade));
                this.fadeThread.Start();
                this.fadeEnabled = true;
            }
            else if (mode.Equals("HSV Sweep"))
            {
                this.screenTimer.Enabled = false;
                if (this.fadeEnabled)
                    this.fadeThread.Abort();
                this.fadeThread = new Thread(new ThreadStart(callHsvFade));
                this.fadeThread.Start();
                this.fadeEnabled = true;
            }
            else if (mode.Equals("Screen Responsive"))
            {
                if (this.fadeEnabled)
                    this.fadeThread.Abort();
                this.fadeEnabled = false;
                this.screenTimer = new System.Timers.Timer(50);//10 hz
                this.screenTimer.Elapsed += new System.Timers.ElapsedEventHandler(callSetAvg);
                this.screenTimer.Enabled = true;
            }
            else if (mode.Equals("Manual Selection"))
            {
                if (this.fadeEnabled)
                    this.fadeThread.Abort();
                this.fadeEnabled = false;
                this.screenTimer.Enabled = false;
                DialogResult result = colorChoose.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //this.BackColor = colorChoose.Color;
                    fade(colorChoose.Color, 0, 1);
                }
            }
            else if (mode.Equals("Sin Wave")) {
                if (this.fadeEnabled)
                    this.fadeThread.Abort();
                this.screenTimer.Enabled = false;
                this.fadeThread = new Thread(new ThreadStart(callSinFade));
                this.fadeThread.Start();
                this.fadeEnabled = true;
            }
            else { Console.WriteLine("This should never happen"); }//invalid choice
        }

        private int[] HsvToRgb(double h, double S, double V)//from here to... \/ \/ \/
        {
            int[] result = new int[3];
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            result[0] = Clamp((int)(R * 255.0));
            result[1] = Clamp((int)(G * 255.0));
            result[2] = Clamp((int)(B * 255.0));
            return result;
        }

        private int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }//here is taken from StackOverflow @ https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
    }
}
