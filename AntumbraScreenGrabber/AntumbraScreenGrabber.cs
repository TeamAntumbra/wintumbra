using System.ComponentModel.Composition;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Bitmaps;
using System;
using Antumbra.Glow;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using Antumbra.Glow.Observer.Logging;

namespace AntumbraScreenDriver
{
    [Export(typeof(GlowExtension))]
    public class AntumbraScreenGrabber : GlowScreenGrabber, Loggable, AntumbraBitmapSource
    {
        public delegate void NewScreenAvail(FastBitmap image, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;
        private Thread driver;
        private bool running = false;
        public override bool IsDefault
        {
            get { return true; }
        }

        public override Guid id
        {
            get { return Guid.Parse("15115e91-ed5c-49e6-b7a8-4ebbd4dabb2e"); }
        }

        //DLL declaration
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
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
        public override Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }
        public override string Website
        {
            get { return "https://antumbra.io/"; }
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgEvent += new NewLogMsg(observer.NewLogMsgAvail);
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
            this.running = false;
            if (null != this.driver && this.driver.IsAlive) {
                this.driver.Abort();
            }
            this.driver = null;
            return true;
        }

        public override void AttachObserver(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += new NewScreenAvail(observer.NewBitmapAvail);
        }

        private void captureTarget()
        {
            int runX = x;
            int runY = y;
            int runW = width;
            int runH = height;
            while (this.running) {
                Bitmap screen = null;
                Graphics grphx = null;
                try {
                    screen = new Bitmap(runW, runH, PixelFormat.Format32bppArgb);
                    grphx = Graphics.FromImage(screen);
                    grphx.CopyFromScreen(runX, runY, 0, 0, new Size(runW, runH));
                    grphx.Save();
                    if (null != screen && NewScreenAvailEvent != null) {
                        FastBitmap fastBm = screen.FastLock();
                        NewScreenAvailEvent(fastBm, EventArgs.Empty);
                    }
                }
                catch (Exception e) {
                    NewLogMsgEvent(this.Name, e.ToString());
                }
                finally {
                    if (screen != null)
                        screen.Dispose();
                    if (grphx != null)
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