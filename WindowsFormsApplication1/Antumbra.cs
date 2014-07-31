using System;
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
using System.IO.Ports;

namespace Antumbra
{
    public partial class Antumbra : Form
    {
        private System.Timers.Timer timer;
        bool continuous = false;
        public Antumbra()
        {
            InitializeComponent();
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
            int size = 10;//size X size rectangles used for pollings
            Size pollingRectSize = new Size(size, size);
            int avgR = 0, avgG = 0, avgB = 0;
            var points = getPollingPoints(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, 0);
            //Bitmap screen = getScreen();
            foreach (Point point in points)
            {
                //Console.WriteLine(point.X.ToString() + ',' + point.Y.ToString());
                //Console.WriteLine((point.X + pollingRectSize.Width).ToString() + ',' + (point.Y + pollingRectSize.Height).ToString());
                Bitmap currentScreen = getScreenAvgAt(point, pollingRectSize);
                //int red = 0, green = 0, blue = 0;
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
                }
                //red /= 900;
                //green /= 900;//avg
                //blue /= 900;
                //Console.WriteLine("R: " + red + " G: " + green + " B: " + blue);
            }
            int totalPixels = pollingRectSize.Height * pollingRectSize.Width * points.Length;
            avgR /= totalPixels;
            avgG /= totalPixels;
            avgB /= totalPixels;
            Color avgColor = Color.FromArgb(avgR, avgG, avgB);
            this.BackColor = avgColor;//this has issues with text fields in the same window (needs thread safety)
        }

        private Bitmap getScreen()
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            Bitmap result = new Bitmap(height, width);
            Graphics gfxScreenshot = Graphics.FromImage(result);
            Size s = new Size(height, width);
            gfxScreenshot.CopyFromScreen(0, 0, 0, 0, s, CopyPixelOperation.SourceCopy);
            return result;
        }

        private Bitmap getSectionOf(Bitmap screen, Point topLeft, Size size)
        {
            Rectangle wanted = new Rectangle(topLeft, size);
            return screen.Clone(wanted, PixelFormat.Format16bppRgb555);
        }

        private Point[] getPollingPoints(int width, int height, int pad)//screen width and height
        {
            Point leftTop = new Point(width / 8 + pad, height / 5 + pad);
            Point midTop = new Point(width / 2, height / 5 + pad);
            Point topRight = new Point(7 * width / 8 - pad, height / 5 + pad);
            Point leftMid = new Point(width / 5 + pad, height / 2);
            Point rightMid = new Point(7 * width / 8 - pad, height / 2);
            Point leftBot = new Point(width / 8 + pad, 4 * height / 5 - pad);
            Point midBot = new Point(width / 2, 4 * height / 5 - pad);
            Point rightBot = new Point(7 * width / 8 - pad, height / 5 + pad);
            Point[] result = {leftTop, midTop, topRight, leftMid, rightMid, leftBot, midBot, rightBot};
            return result;
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
            return Color.FromArgb(red / total, blue / total, green / total);
        }

        private Bitmap getScreenAvgAt(Point point, Size size)
        {
            Bitmap result;
            result = new Bitmap(size.Width, size.Height);
            Graphics gfxScreenshot = Graphics.FromImage(result);
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            Size s = new Size(width/64, height/64);
            try
            {
                gfxScreenshot.CopyFromScreen(point.X, point.Y, 0, 0, s, CopyPixelOperation.SourceCopy);
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine(point.X.ToString() + ' ' + point.Y.ToString());
            }
            return result;
        }

        private void continuousCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.continuous = !this.continuous; 
            if (this.continuous)
            {
                timer = new System.Timers.Timer(200);
                timer.Elapsed += new System.Timers.ElapsedEventHandler(callSetBack);
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        private void sendViaSerial_Click(object sender, EventArgs e)
        {
            Console.WriteLine(SerialPort.GetPortNames().Length);
            foreach (String port in SerialPort.GetPortNames())
            {
                Console.WriteLine(port);
            }
            /*SerialPort serial = new SerialPort();
            serial.PortName = "";//TODO fill in settings here
            serial.BaudRate = 115200;
            serial.Parity = Parity.None;
            serial.DataBits = 8;
            serial.StopBits = StopBits.One;
            serial.Handshake = Handshake.None;
            serial.ReadTimeout = 250;
            serial.WriteTimeout = 250;
            serial.Open();
            serial.WriteLine("test");
            serial.Close();*/
        }
    }
}
