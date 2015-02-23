using System.ComponentModel.Composition;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using System;
using Antumbra.Glow;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using Antumbra.Glow.Utility;

namespace AntumbraScreenDriver
{
    [Export(typeof(GlowExtension))]
    public class AntumbraScreenGrabber : GlowScreenGrabber
    {
        private Thread driver;
        public delegate void NewScreenAvail(Bitmap image, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        private bool running = false;
        public override int id { get; set; }
        public override bool IsDefault
        {
            get { return true; }
        }

        //DLL declaration
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        //BitBlt - used to get screen info

        public override String Name { get { return "Antumbra Screen Grabber (Default)"; } }
        public override String Description
        {
            get
            {
                return "Default means of grabbing the screen. "
                     + "Not DirectX compatible. Uses the GDI+ library.";
            }
        }
        public override bool IsRunning
        {
            get { return this.running; }
        }
        public override string Author { get { return "Team Antumbra"; } }
        public override Version Version { get { return new Version("0.0.1"); } }
        public override string Website
        {
            get { return "https://antumbra.io/"; }
        }

        public override bool Start()
        {
            this.driver = new Thread(new ThreadStart(captureTarget));
            this.driver.Start();
            this.running = true;
            return true;
        }

        public override bool Settings()
        {
            return false;
        }

        public override bool Stop()
        {
            this.NewScreenAvailEvent = null;
            if (null != this.driver && this.driver.IsAlive) {
                this.driver.Abort();
            }
            this.running = false;
            this.driver = null;
            return true;
        }

        public override void AttachEvent(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += new NewScreenAvail(observer.NewBitmapAvail);
        }

        public override void RecmmndCoreSettings()
        {
            this.x = 0;
            this.y = 0;
            var bounds = Screen.PrimaryScreen.Bounds;
            this.width = bounds.Width;
            this.height = bounds.Height;
        }

        private void captureTarget()
        {
            int runX = x;
            int runY = y;
            int runW = width;
            int runH = height;
            while (true) {
                //Bitmap screen = getPixelBitBlt(this.width, this.height);
                Bitmap screen = new Bitmap(runW, runH, PixelFormat.Format32bppArgb);
                Graphics grphx = Graphics.FromImage(screen);
                grphx.CopyFromScreen(runX, runY, 0, 0, new Size(runW, runH));
                grphx.Save();
                grphx.Dispose();
                if (null != screen) {
                    NewScreenAvailEvent(screen, EventArgs.Empty);
                    screen.Dispose();
                    grphx.Dispose();
                }
            }
        }

        private Bitmap getPixelBitBlt(int x, int y, int width, int height)
        {
            try {
                Bitmap screen = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                using (Graphics gdest = Graphics.FromImage(screen)) {
                    using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
                        IntPtr hSrcDC = gsrc.GetHdc();
                        IntPtr hDC = gdest.GetHdc();
                        int retval = BitBlt(hDC, x, y, width, height, hSrcDC, 0, 0, (int)0x00CC0020);
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