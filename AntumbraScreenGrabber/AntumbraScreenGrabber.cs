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
        public delegate void NewScreenAvail(int[,] pixels, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;

        private int deviceId;
        private Thread driver;
        private bool running;
        public override bool IsDefault
        {
            get { return true; }
        }

        public override Guid id
        {
            get { return Guid.Parse("15115e91-ed5c-49e6-b7a8-4ebbd4dabb2e"); }
        }

        public override int devId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
            }
        }

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
            get { return "https://wintumbra.rtfd.org"; }
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgEvent += new NewLogMsg(observer.NewLogMsgAvail);
        }

        public override GlowScreenGrabber Create()
        {
            return new AntumbraScreenGrabber();
        }

        public override bool Start()
        {
            driver = new Thread(new ThreadStart(captureTarget));
            driver.Start();
            running = true;
            return true;
        }

        public override bool Settings()
        {
            return false;
        }

        public override bool Stop()
        {
            NewScreenAvailEvent = null;
            running = false;
            if (null != driver && driver.IsAlive) {
                driver.Abort();
            }
            driver = null;
            return true;
        }

        public override void AttachObserver(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += new NewScreenAvail(observer.NewBitmapAvail);
        }

        public override void Dispose()
        {
            if (driver != null && driver.IsAlive) {
                driver.Abort();
            }
        }

        private void Log(string msg)
        {
            if (NewLogMsgEvent != null) {
                NewLogMsgEvent("AntumbraScreenGrabber", msg);
            }
        }

        private void captureTarget()
        {
            int runX = x;
            int runY = y;
            Size runSize = new Size(width, height);
            while (this.running) {
                try {
                    using (Bitmap screen = new Bitmap(runSize.Width, runSize.Height, PixelFormat.Format32bppArgb)) {
                        using (Graphics grphx = Graphics.FromImage(screen)) {
                            try {
                                grphx.CopyFromScreen(runX, runY, 0, 0, runSize, CopyPixelOperation.SourceCopy);
                                grphx.Save();
                                if (null != screen && NewScreenAvailEvent != null) {
                                    NewScreenAvailEvent(screen.FastLock().DataArray, EventArgs.Empty);
                                }
                            }
                            catch (Exception e) {
                                Log(e.Message + '\t' + e.StackTrace);
                            }
                            Thread.Sleep(captureThrottle);
                        }
                    }
                }
                catch (Exception ex) {
                    Log(ex.Message + '\t' + ex.StackTrace);
                }
            }
        }
    }
}