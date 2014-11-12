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
using Antumbra.Glow.ExtensionFramework.ScreenProcessors;


namespace Antumbra.Glow.ExtensionFramework.Drivers
{
    public class AntumbraScreenGrabber : GlowScreenGrabber //used to capture screen information in normal (default) use mode
    {
        public String Name { get { return "Antumbra Screen Grabber (Default)"; } }
        public String Author { get { return "Team Antumbra"; } }
        public String Version { get { return "V0.1.0"; } }
        public String Description { get { return "Default means of grabbing the screen."; } }
        public String Type { get { return "Driver"; } }
        //Being DLL declarations
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        //BitBlt - used to get screen info in an efficent manner

        public int width, height;//screen width and height
        private AntumbraCoreDriver antumbra;
        //int widthDivs, heightDivs;//width and height divisions for screen polling
        int x, y;//x and y bounds of screen
        //System.Drawing.Point[] points;//polling points
        private Thread captThread;
        System.Drawing.Size size;//polling size
        public Screen display { get; set; }//display for avging
        public double saturationAdditive { get; set; }
        public bool saturationEnabled { get; set; }
        public Bitmap screen { get; private set; }

        public AntumbraScreenGrabber(AntumbraCoreDriver antumbra)
        {
            this.antumbra = antumbra;
            this.captThread = new Thread(new ThreadStart(capture));
            this.size = new System.Drawing.Size(50, 30);
            this.display = Screen.PrimaryScreen;
            this.saturationAdditive = .45;//+45% saturation (caps @ 100%)
            this.saturationEnabled = true;
            updateBounds();
            //this.widthDivs = 4;
            //this.heightDivs = 4;
        }

        public System.Drawing.Color GetColor()//processes screen with selected processor
        {
            return antumbra.screenProcessor.Process(this.screen);
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
    }
}
