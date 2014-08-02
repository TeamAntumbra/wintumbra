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
        private System.Timers.Timer screenTimer;//timer for screen color averaging
        bool continuous;//, serialEnabled;
        bool fadeEnabled;
        byte lastR, lastG, lastB;
        int offThreshold; //level at which all (RGB) must be under to turn off
        int fadeThreshold;
        int changeThreshold; //difference in colors needed to change
        Size pollingRectSize = new Size(50, 50);
        bool on;
        //OpenNETCF.IO.Ports.SerialPort serial;
        //SerialPort serial;
        SerialConnector serial;
        int width, height, x, y;

        public Antumbra()
        {
            InitializeComponent();
            this.width = Screen.PrimaryScreen.Bounds.Width;
            this.height = Screen.PrimaryScreen.Bounds.Height;
            this.x = Screen.PrimaryScreen.Bounds.X;
            this.y = Screen.PrimaryScreen.Bounds.Y;
            this.serial = new SerialConnector("COM4");
            this.on = true;;//depends on how the Antumbra starts up
            this.lastR = 255;
            this.lastG = 255;
            this.lastB = 255;
            this.offThreshold = 20;//TODO test how low this should be
            this.changeThreshold = 6; //see shouldChange(Color, Color) (lower is more sensitive)
            this.fadeThreshold = 10;//diff before taking smaller steps to destination color
            this.continuous = false;
            this.fadeEnabled = false;
        }

        private void takeScreenshotBtn_Click(object sender, EventArgs e)
        {
            //this.Hide();
            setBackToAvg();
        }

        private void callSetBack(object sender, System.Timers.ElapsedEventArgs e)
        {
            setBackToAvg();
            //Console.WriteLine("polling");
        }

        private void setBackToAvg()
        {
            turnOn();//and reset brightness to full
            int avgR = 0, avgG = 0, avgB = 0;
            var points = getPollingPoints(this.width, this.height, 0);
            Bitmap screen = getScreen();//Shot();
            foreach (Point point in points)
            {
                //Console.WriteLine(point.X.ToString() + ',' + point.Y.ToString());
                //Console.WriteLine((point.X + pollingRectSize.Width).ToString() + ',' + (point.Y + pollingRectSize.Height).ToString());
                /*Bitmap currentScreen = getScreenAvgAt(point, pollingRectSize);
                for (int r = 0; r < currentScreen.Height; r++)//for each pixel
                {
                    for (int c = 0; c < currentScreen.Width; c++)
                    {
                        Color color = currentScreen.GetPixel(c, r);//yes this is weird to have switched...I did it backwards
                        avgR += color.R;
                        avgG += color.G;
                        avgB += color.B;
                        //red += color.R;
                        //green += color.G;
                        //blue += color.B;
                    }
                }*/
                //red /= 900;
                //green /= 900;//avg
                //blue /= 900;
                //Console.WriteLine("R: " + red + " G: " + green + " B: " + blue);

                Bitmap section = getSectionOf(screen, point, pollingRectSize);
                Color areaAvg = getAvgFromBitmap(section);
                avgR += areaAvg.R;
                avgG += areaAvg.G;
                avgB += areaAvg.B;
            }
            /*int totalPixels = pollingRectSize.Height * pollingRectSize.Width * points.Length;
            avgR /= totalPixels;
            avgG /= totalPixels;
            avgB /= totalPixels;*/
            int divisor = points.Length;//divsor to avg values (num of colors)
            avgR /= divisor;
            avgG /= divisor;
            avgB /= divisor;
            Color avgColor = Color.FromArgb(avgR, avgG, avgB);
            fade(avgColor);
            //sendColorToSerial(avgColor);
            //this.BackColor = avgColor;//this has issues with text fields in the same window (needs thread safety)
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
                screenTimer.Elapsed += new System.Timers.ElapsedEventHandler(callSetBack);
                screenTimer.Enabled = true;
            }
            else
            {
                screenTimer.Enabled = false;
            }
        }

        private void turnOff() 
        {
            if (!this.on)
                return;//do nothing
            byte[] command = { 0x04, 0 };
            byte[] stuffed = readyToSend(command);
            this.serial.send(stuffed);
            this.on = false;//update
        }

        private void turnOn()
        {
            if (this.on)
                return; //do nothing
            byte[] command = { 0x04, 15 };
            byte[] stuffed = readyToSend(command);
            this.serial.send(stuffed);
            this.on = true;//update
        }

        private void sendColorToSerial(Color newColor)
        {
            Color color = Color.FromArgb(this.lastR, this.lastG, this.lastB);
            if (!shouldChange(color, newColor))//dont change leds
                return;
            int diff = calcDiff(color, newColor);
            //Console.WriteLine("diff - " + diff.ToString());
            fade(newColor);
        }

        private void fade(Color newColor) //TODO: perfect this
        {
            if (!this.serial.isReady())
            {//not ready
                Console.WriteLine("Serial not ready");
                return; //dont bother trying
            }
            int r = this.lastR;
            int g = this.lastG;
            int b = this.lastB;
            bool rDone = false, gDone = false, bDone = false;
            while(true)
            {
                Console.WriteLine("r - " + r.ToString() + " g - " + g.ToString() + " b - " + b.ToString());
                if (newColor.R - r >= this.fadeThreshold)
                    r += this.fadeThreshold;
                else if (r - newColor.R >= this.fadeThreshold)
                    r -= this.fadeThreshold;
                else
                    rDone = true;
                if (newColor.G - g >= this.fadeThreshold)
                    g += this.fadeThreshold;
                else if (g - newColor.G >= this.fadeThreshold)
                    g -= this.fadeThreshold;
                else
                    gDone = true;
                if (newColor.B - b >= this.fadeThreshold)
                    b += this.fadeThreshold;
                else if (b - newColor.B >= this.fadeThreshold)
                    b -= this.fadeThreshold;
                else
                    bDone = true;
                Color step = Color.FromArgb(r, g, b);
                changeTo(step);//update
                if (rDone && gDone && bDone)
                    return;//end this madness
                if (!shouldChange(newColor, step))//close enough
                    return;
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
            byte[] command = convertColorToSerialCommand(color);
            byte[] stuffed = readyToSend(command);
            this.serial.send(stuffed);
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
            //byte start = 0x7E;
            //byte end = 0x7F;
            //byte escape = 0x7D;
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

        private Bitmap getScreenShot()
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
        public static extern IntPtr GetWindowDC(IntPtr ptr);

        private void button1_Click(object sender, EventArgs e)//choose color button
        {
            DialogResult result = colorChoose.ShowDialog();
            if (result == DialogResult.OK)
            {
                //this.BackColor = colorChoose.Color;
                fade(colorChoose.Color);
            }
        }

        private void colorFadeButton_Click(object sender, EventArgs e)
        {
            fade(Color.Red);
            fade(Color.Purple);
            fade(Color.Blue);
            fade(Color.Green);
            fade(Color.Yellow);
            fade(Color.Orange);
        }

        private void powerToggleBtn_Click(object sender, EventArgs e)
        {
            if (this.on)
                turnOff();
            else
                turnOn();
        }

        /*public void Dispose() //clean up
        {
            this.serial.close();
            Dispose(true);
            GC.SuppressFinalize(this);
        }*/
    }
}
