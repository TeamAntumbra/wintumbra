using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.Direct3D9;
using System.Drawing;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;
using System.Threading;
using Antumbra.Glow.Observer.Bitmaps;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using System.Reflection;
using System.Diagnostics;

namespace SlimDXCapture
{
    [Export(typeof(GlowExtension))]
    public class SlimDXScreenCapture : GlowScreenGrabber, AntumbraBitmapSource, Loggable, IDisposable, ToolbarNotificationSource
    {
        public delegate void NewScreenAvail(int[,] pixels, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifEvent;

        private int deviceId;
        private Thread driver;
        private bool running = false;

        public SlimDXScreenCapture()
        {
            AttachObserver(LoggerHelper.GetInstance());
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

        public override GlowScreenGrabber Create()
        {
            return new SlimDXScreenCapture();
        }

        public Bitmap CaptureScreen(IntPtr hwnd)
        {
            Bitmap result = null;
            List<Bitmap> screens = new List<Bitmap>();
            try {
                using (Direct3D d3 = new Direct3D()) {
                    foreach (AdapterInformation adapterInfo in d3.Adapters) {
                        PresentParameters parameters = new PresentParameters();
                        parameters.BackBufferFormat = adapterInfo.CurrentDisplayMode.Format;
                        parameters.BackBufferHeight = adapterInfo.CurrentDisplayMode.Height;
                        parameters.BackBufferWidth = adapterInfo.CurrentDisplayMode.Width;
                        parameters.Multisample = SlimDX.Direct3D9.MultisampleType.None;
                        parameters.SwapEffect = SlimDX.Direct3D9.SwapEffect.Discard;
                        parameters.DeviceWindowHandle = hwnd;
                        parameters.PresentationInterval = SlimDX.Direct3D9.PresentInterval.Default;
                        parameters.FullScreenRefreshRateInHertz = 0;
                        using (Device d = new Device(d3, adapterInfo.Adapter, DeviceType.Hardware, hwnd, CreateFlags.HardwareVertexProcessing, parameters)) {
                            using (SlimDX.Direct3D9.Surface surface = SlimDX.Direct3D9.Surface.CreateOffscreenPlain(d, adapterInfo.CurrentDisplayMode.Width, adapterInfo.CurrentDisplayMode.Height, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.SystemMemory)) {
                                d.GetFrontBufferData(0, surface);
                                screens.Add(new Bitmap(SlimDX.Direct3D9.Surface.ToStream(surface, SlimDX.Direct3D9.ImageFileFormat.Bmp)));
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                result = null;
                this.Log(e.StackTrace + '\n' + e.Message);
            }
            int newWidth = 0;
            int newHeight = 0;
            float xDpi = 0;
            float yDpi = 0;
            foreach (Bitmap screen in screens) {
                newWidth += screen.Width;
                newHeight = newHeight > screen.Height ? newHeight : screen.Height;
                xDpi = screen.HorizontalResolution;
                yDpi = screen.VerticalResolution;
            }
            newHeight += 1;
            result = new Bitmap(newWidth, newHeight);
            result.SetResolution(xDpi, yDpi);
            using (Graphics g = Graphics.FromImage(result)) {
                int drawX = 0;
                foreach(Bitmap screen in screens) {
                    g.DrawImage(screen, new Point(drawX, 0));
                    drawX += screen.Width;
                }
            }
            foreach (Bitmap screen in screens) {
                screen.Dispose();
            }
            return result;
        }

        private void Log(String msg)
        {
            if (NewLogMsgEvent != null)
                NewLogMsgEvent("SlimDXCapture", msg);
        }

        private IntPtr FindForegroundPrcs()
        {
            foreach (Process prc in Process.GetProcesses()) {
                IntPtr handle = prc.MainWindowHandle;
                if (NativeMethods.IsWindowInForeground(handle))
                    return handle;
            }
            return IntPtr.Zero;
        }

        public override void Dispose()
        {
            if (driver != null && driver.IsAlive) {
                driver.Abort();
            }
        }

        public override Guid id
        {
            get { return Guid.Parse("ad1e6255-d9b4-4e6d-995f-a094e6ea5f7b"); }
        }

        public override string Name
        {
            get { return "DxScreenCapture"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A plugin that will capture DirectX content as well as normal content, however at a lower rate due to its process."; }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override bool IsDefault
        {
            get { return false; }
        }

        private void target()
        {
            while (this.IsRunning) {
                Bitmap result = CaptureScreen(FindForegroundPrcs());
                if (NewScreenAvailEvent != null && result != null) {
                    using(FastBitmap fb = result.FastLock()) {
                        NewScreenAvailEvent(fb.DataArray, EventArgs.Empty);
                    }
                    result.Dispose();
                }
                Thread.Sleep(captureThrottle);
            }
        }

        public override bool Start()
        {
            AnnounceMessage(3000, "The capture plugin is now grabbing content.", 1);
            this.running = true;
            this.driver = new Thread(new ThreadStart(target));
            this.driver.Start();
            return true;
        }

        public override bool Stop()
        {
            if (this.driver != null && this.driver.IsAlive)
                this.driver.Abort();
            this.running = false;
            return true;
        }

        public override bool Settings()
        {
            return false;
        }

        public override void AttachObserver(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += observer.NewBitmapAvail;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgEvent += observer.NewLogMsgAvail;
        }

        [System.Security.SuppressUnmanagedCodeSecurity()]
        internal static class NativeMethods
        {
            internal static bool IsWindowInForeground(IntPtr hWnd)
            {
                return hWnd == GetForegroundWindow();
            }

            [DllImport("user32.dll")]
            internal static extern IntPtr GetForegroundWindow();
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifEvent += observer.NewToolbarNotifAvail;
        }

        private void AnnounceMessage(int time, string msg, int icon)
        {
            if (this.NewToolbarNotifEvent != null)
                this.NewToolbarNotifEvent(time, "DxScreenCapture", msg, icon);
        }
    }
}
