using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        int offThreshold; //level at which all (RGB) must be under to turn off
        int fadeThreshold;
        int sleepTime;//default time to sleep between color steps when changing
        int changeThreshold; //difference in colors needed to change
        Size pollingRectSize = new Size(50, 50);
        bool on;
        //OpenNETCF.IO.Ports.SerialPort serial;
        //SerialPort serial;
        SerialConnector serial;
        int width, height, x, y;

        public Antumbra()
        {
            this.icon =  new System.Windows.Forms.NotifyIcon();
            this.icon.BalloonTipTitle = "Antumbra|Glow";
            this.icon.BalloonTipText = "Click the icon for a menu\nDouble click for to open";
            InitializeComponent();
            this.width = Screen.PrimaryScreen.Bounds.Width;
            this.height = Screen.PrimaryScreen.Bounds.Height;
            this.x = Screen.PrimaryScreen.Bounds.X;
            this.y = Screen.PrimaryScreen.Bounds.Y;
            this.serial = new SerialConnector("COM4");
            this.on = true;;//depends on how the Antumbra starts up
            this.lastR = 0;
            this.lastG = 0;
            this.lastB = 0;
            this.currentColor = Color.Black;//depends on how the Antumbra starts up
            this.color = Color.Black;
            this.offThreshold = 10;//TODO test how low this should be
            this.changeThreshold = 6; //see shouldChange(Color, Color) (lower is more sensitive)
            this.fadeThreshold = 6;//diff before taking smaller steps to destination color
            this.sleepTime = 0;//time to sleep after taking each step
            this.continuous = false;
            this.fadeEnabled = false;
            this.fadeThread = new Thread(new ThreadStart(callColorFade));
            this.screenTimer = new System.Timers.Timer();
            turnOff();
            this.modeComboBox.SelectedIndex = 0;
        }

        private void takeScreenshotBtn_Click(object sender, EventArgs e)
        {
            //this.Hide();
            setBackToAvg();
        }

        private void setBackToAvg()
        {
            int avgR = 0, avgG = 0, avgB = 0;
            var points = getPollingPoints(this.width, this.height, 0);
            Bitmap screen = getScreen();//Shot();
            foreach (Point point in points)
            {
                Bitmap section = getSectionOf(screen, point, pollingRectSize);
                Color areaAvg = getAvgFromBitmap(section);
                avgR += areaAvg.R;
                avgG += areaAvg.G;
                avgB += areaAvg.B;
            }
            int divisor = points.Length;//divsor to avg values (num of colors)
            avgR /= divisor;
            avgG /= divisor;
            avgB /= divisor;
            Color avgColor = Color.FromArgb(avgR, avgG, avgB);
            fade(avgColor, this.fadeThreshold, this.sleepTime);
            screen.Dispose();//clean up for next screenshot
            lastR = (byte)avgR;
            lastG = (byte)avgG;
            lastB = (byte)avgB;
        }

        private Bitmap getScreen()//return bitmap of entire screen
        {
            Bitmap result = new Bitmap(this.width, this.height);//, PixelFormat.Format16bppRgb555);
            using (Graphics gfxScreenshot = Graphics.FromImage(result))
                gfxScreenshot.CopyFromScreen(this.x, this.y, 0, 0, new Size(this.width, this.height));//, CopyPixelOperation.SourceCopy)
            return result;
        }

        private Bitmap getSectionOf(Bitmap screen, Point topLeft, Size size)
        {
            Rectangle wanted = new Rectangle(topLeft, size);
            return screen.Clone(wanted, PixelFormat.Format16bppRgb555);
        }

        private Point[] getPollingPoints(int width, int height, int pad)//screen width and height TODO make padding actually smart
        {
            Point leftTop = new Point(width / 8 + pad, height / 4 + pad);
            Point midTop = new Point(width / 2, height / 4 + pad);
            Point topRight = new Point(7 * width / 8 - pad, height / 4 + pad);
            Point leftMid = new Point(width / 8 + pad, height / 2);
            Point rightMid = new Point(7 * width / 8 - pad, height / 2);
            Point leftBot = new Point(width / 8 + pad, 3 * height / 4 - pad);
            Point midBot = new Point(width / 2, 3 * height / 4 - pad);
            Point rightBot = new Point(7 * width / 8 - pad, 3* height / 4 + pad);
            Point[] result = {leftTop, midTop, topRight, leftMid, rightMid, leftBot, midBot, rightBot};
            return result;
        }

       /* private Point[] getPollingPoints(Bitmap screen, int widthDivs, int heightDivs)
        {
            List<Point> points = new List<Point>();
            int height = screen.Height;
            int width = screen.Width;
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
            return calcDiff(color, other)  > this.changeThreshold;
        }

        private Color getAvgFromBitmap(Bitmap bm)
        {
            int red = 0, blue = 0, green = 0;
            int total = bm.Width * bm.Height;
            for (int r = 0; r < bm.Width; r++)
            {
                for (int c = 0; c < bm.Height; c++)
                {
                    Color current = bm.GetPixel(r, c);
                    red += current.R;
                    blue += current.B;
                    green += current.G;
                }
            }
            return Color.FromArgb(red / total, green / total, blue / total);
        }

        private Bitmap getScreenAvgAt(Point point, Size size)
        {
            Bitmap result;
            result = new Bitmap(size.Width, size.Height);
            Graphics gfxScreenshot = Graphics.FromImage(result);
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            Size s = new Size(width/64, height/64);
            gfxScreenshot.CopyFromScreen(point.X, point.Y, 0, 0, s, CopyPixelOperation.SourceCopy);
            return result;
        }

        private void continuousCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.continuous = !this.continuous; 
            if (this.continuous)
            {
                screenTimer = new System.Timers.Timer(50);//20 hz
                screenTimer.Elapsed += new System.Timers.ElapsedEventHandler(callSetAvg);
                screenTimer.Enabled = true;
            }
            else
                screenTimer.Enabled = false;
        }

        private void callSetAvg(object sender, System.Timers.ElapsedEventArgs e)
        {
            setBackToAvg();
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
                fade(color, 1, 0);//color, fade percision, sleep time
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
                fade(Color.FromArgb(rgb[0], rgb[1], rgb[2]), 1, 0);
            }
        }

        private void turnOff() 
        {
            if (!this.on)
                return;//do nothing
            byte[] command = { 0x04, 0 };
            byte[] stuffed = readyToSend(command);
            if (this.serial.send(stuffed))
                this.on = false;//update
        }

        private void turnOn()
        {
            if (this.on)
                return; //do nothing
            byte[] command = { 0x04, 15 };
            byte[] stuffed = readyToSend(command);
            if (this.serial.send(stuffed))
                this.on = true;//update
        }

        private void sendColorToSerial(Color newColor)
        {
            Color color = Color.FromArgb(this.lastR, this.lastG, this.lastB);
            if (!shouldChange(color, newColor))//dont change leds
                return;
            int diff = calcDiff(color, newColor);
            fade(newColor, this.fadeThreshold, this.sleepTime);
        }

        private void fade(Color newColor, int threshold, int sleepTime) //TODO: perfect this
        {
            if (!this.serial.isReady())
            {//not ready
                Console.WriteLine("Serial not ready");
                return; //dont bother trying
            }
            if (!shouldChange(Color.FromArgb(this.lastR, this.lastG, this.lastB), newColor))
                return;//no update needed
            int r = this.lastR;
            int g = this.lastG;
            int b = this.lastB;
            bool rDone = false, gDone = false, bDone = false;
            while(true)
            {
                if (newColor.R - r >= threshold)
                    r += threshold;
                else if (r - newColor.R >= threshold)
                    r -= threshold;
                else
                    rDone = true;
                if (newColor.G - g >= threshold)
                    g += threshold;
                else if (g - newColor.G >= threshold)
                    g -= threshold;
                else
                    gDone = true;
                if (newColor.B - b >= threshold)
                    b += threshold;
                else if (b - newColor.B >= threshold)
                    b -= threshold;
                else
                    bDone = true;
                Color step = Color.FromArgb(r, g, b);
                changeTo(step);//update
                if (rDone && gDone && bDone)
                    return;//end this madness
                if (!shouldChange(newColor, step))//close enough
                    return;
                Thread.Sleep(sleepTime);
            }
        }

        private void changeTo(Color color)
        {
            if (color.R < this.offThreshold && color.G < this.offThreshold && color.B < this.offThreshold)
            {
                turnOff();
                updateLast(color);
                return;
            }
            turnOn();
            byte[] command = convertColorToSerialCommand(color);
            byte[] stuffed = readyToSend(command);
            if (this.serial.send(stuffed))
                updateLast(color);
        }

        private void updateLast(Color color)
        {
            this.lastR = color.R;
            this.lastG = color.G;
            this.lastB = color.B;
        }

        private byte[] readyToSend(byte[] command)//will add start, stop, escape, and checksum
        {
            byte escape = 0x7D;
            byte start = 0x7E;
            byte end = 0x7F;
            List<byte> result = new List<byte>();
            foreach (byte b in command)
            {
                byte current;
                if (b == escape)
                {
                    result.Add(escape);
                    current = (byte)(b ^ 0x20);
                }
                else if (b == start)
                {
                    result.Add(escape);
                    current = (byte)(b ^ 0x20);
                }
                else if (b == end)
                {
                    result.Add(escape);
                    current = (byte)(b ^ 0x20);
                }
                else//no transformation needed
                {
                    current = (byte)b;
                }
                result.Add(current);
            }
            result.Add(generateChecksum(result.ToArray<byte>()));
            result.Insert(0, start);
            result.Add(end);
            return result.ToArray<byte>();
        }

        private byte[] convertColorToSerialCommand(Color color) //needs to follow the protocol in docs repo
        {
            byte command = 0x02;//command code for setting color
            List<byte> bytes =  new List<byte>();
            bytes.Add(command);
            byte red = color.R;
            bytes.Add(red);
            byte green = color.G;
            bytes.Add(green);
            byte blue = color.B;
            bytes.Add(blue);
            return bytes.ToArray<byte>();
        }

        private byte generateChecksum(byte[] bytes)
        {
            uint sum = 0;
            foreach (byte b in bytes)
            {
                sum += b;
            }
            return (byte)(sum % 0x100);
        }

        /*private Bitmap getScreenShot()
        {
            Size sz = Screen.PrimaryScreen.Bounds.Size;
            IntPtr hDesk = GetDesktopWindow();
            IntPtr hSrce = GetWindowDC(hDesk);
            IntPtr hDest = CreateCompatibleDC(hSrce);
            IntPtr hBmp = CreateCompatibleBitmap(hSrce, sz.Width, sz.Height);
            IntPtr hOldBmp = SelectObject(hDest, hBmp);
            bool b = BitBlt(hDest, 0, 0, sz.Width, sz.Height, hSrce, 0, 0, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
            Bitmap bmp = Bitmap.FromHbitmap(hBmp);
            SelectObject(hDest, hOldBmp);
            DeleteObject(hBmp);
            DeleteDC(hDest);
            ReleaseDC(hDesk, hSrce);
            return bmp;
        }
        // P/Invoke declarations
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int
        wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr ptr);*/

        private void powerToggleBtn_Click(object sender, EventArgs e)
        {
            if (this.on)
                turnOff();
            else
                turnOn();
        }

        private void Antumbra_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.icon.Visible = true;
                this.icon.ShowBalloonTip(2500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.icon.Visible = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.icon.Visible = false;
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String mode = this.modeComboBox.Items[this.modeComboBox.SelectedIndex].ToString();
            Console.WriteLine(mode);
            if (mode.Equals("Off"))
            {
                this.screenTimer.Enabled = false;
                this.fadeThread.Abort();
                this.fadeEnabled = false;
                turnOff();
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
                this.screenTimer = new System.Timers.Timer(50);//20 hz
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
                    fade(colorChoose.Color, 1, 0);
                }
            }
            else { Console.WriteLine("This should never happen"); }//invalid choice?
        }

        /*public void Dispose() //clean up
        {
            this.serial.close();
            Dispose(true);
            GC.SuppressFinalize(this);
        }*/

        int[] HsvToRgb(double h, double S, double V)//from here to... \/ \/ \/
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

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }//here is taken from StackOverflow @ https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
    }
}
