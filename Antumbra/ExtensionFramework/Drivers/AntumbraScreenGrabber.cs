using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Interop;
using System.Threading;


namespace Antumbra.Glow.ExtensionFramework.Drivers
{
    public class AntumbraScreenGrabber : Driver //used to capture screen information in normal (default) use mode
    {
        //Being DLL declarations
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        //BitBlt - used to get screen info in an efficent manner

        public int width, height;//screen width and height
        //int widthDivs, heightDivs;//width and height divisions for screen polling
        int x, y;//x and y bounds of screen
        //System.Drawing.Point[] points;//polling points
        private Thread captThread;
        System.Drawing.Size size;//polling size
        public Screen display { get; set; }//display for avging
        public double saturationAdditive { get; set; }
        public bool saturationEnabled { get; set; }
        public Bitmap screen { get; private set; }

        public AntumbraScreenGrabber()
        {
            this.captThread = new Thread(new ThreadStart(capture));
            this.size = new System.Drawing.Size(50, 30);
            this.display = Screen.PrimaryScreen;
            this.saturationAdditive = .45;//+45% saturation (caps @ 100%)
            this.saturationEnabled = true;
            updateBounds();
            //this.widthDivs = 4;
            //this.heightDivs = 4;
        }

        public void start()
        {
            this.captThread = new Thread(new ThreadStart(capture));
            this.captThread.Start();
        }

        public void stop()
        {
            this.captThread.Abort();
        }

        private void capture()
        {
            while (true) {
                this.screen = getPixelBitBlt(this.width, this.height);
            }
        }

        public void updateBounds()
        {
            this.width = this.display.Bounds.Width;
            this.height = this.display.Bounds.Height;
            this.x = this.display.Bounds.X;
            this.y = this.display.Bounds.Y;
            //this.points = getPollingPoints(this.width, this.height, this.widthDivs, this.heightDivs);
            //this.points = getPollingRectPoints(this.width, this.height);
        }

        private Bitmap getPixelBitBlt(int width, int height)
        {
            try {
                Bitmap screen = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                using (Graphics gdest = Graphics.FromImage(screen)) {
                    using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
                        IntPtr hSrcDC = gsrc.GetHdc();
                        IntPtr hDC = gdest.GetHdc();
                        int retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, 0, 0, (int)0x00CC0020);
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();
                    }
                }
                return screen;
            }
            catch (System.ArgumentException) {
                return null;
            }
        }

        /*private System.Drawing.Point[] getPollingRectPoints(float width, float height)
        {
            System.Drawing.Point topLeft = new System.Drawing.Point((int)(width * .125), (int)(height * .125));
            System.Drawing.Point topRight = new System.Drawing.Point((int)(width * .875), (int)(height * .125));
            System.Drawing.Point botLeft = new System.Drawing.Point((int)(width * .125), (int)(height * .875));
            System.Drawing.Point botRight = new System.Drawing.Point((int)(width * .875), (int)(height * .875));
            System.Drawing.Point[] result = { topLeft, topRight, botLeft, botRight };
            return result;
        }

        private System.Drawing.Point[] getPollingPoints(float width, float height, int widthDivs, int heightDivs)
        {           //TODO make this only called when changed in settings (if we even use this in the future)
            List<System.Drawing.Point> points = new List<System.Drawing.Point>();
            float hStep = height / heightDivs;
            float wStep = width / widthDivs;
            for (float y = hStep; y < height; y += hStep) {
                for (float x = wStep; x < width; x += wStep) {
                    //Console.WriteLine(x.ToString() + " " + y.ToString());//debug print
                    points.Add(new System.Drawing.Point((int)x, (int)y));
                }
            }
            return points.ToArray();
        }*/
    }
}
