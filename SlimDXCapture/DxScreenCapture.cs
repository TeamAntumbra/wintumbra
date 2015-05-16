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
using System.Reflection;
using System.Diagnostics;

namespace SlimDXCapture
{
    [Export(typeof(GlowExtension))]
    public class DxScreenCapture : GlowScreenGrabber, AntumbraBitmapSource, Loggable, IDisposable
    {
        public delegate void NewScreenAvail(FastBitmap screen, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;
        private Thread driver;
        private bool running = false;
        private Device d;
        private Surface s;
        //private Direct3D d3;
        private AdapterInformation adapterInfo;
        private PresentParameters parameters;

        public Bitmap CaptureScreen(IntPtr hwnd)//Capture this at a reasonable rate (i.e. throttle this bitch so it doesn't destroy the experience of game in general)
        {//FIXME currently always captures full screen
            Bitmap bm = null;
            try {
                using (Direct3D d3 = new Direct3D()) {
                    AdapterInformation adapterInfo = d3.Adapters.DefaultAdapter;
                    PresentParameters parameters = new PresentParameters();
                    parameters.BackBufferFormat = adapterInfo.CurrentDisplayMode.Format;
                    parameters.BackBufferHeight = this.height;
                    parameters.BackBufferWidth = this.width;
                    parameters.Multisample = SlimDX.Direct3D9.MultisampleType.None;
                    parameters.SwapEffect = SlimDX.Direct3D9.SwapEffect.Discard;
                    parameters.DeviceWindowHandle = hwnd;
                    parameters.PresentationInterval = SlimDX.Direct3D9.PresentInterval.Default;
                    parameters.FullScreenRefreshRateInHertz = 0;
                    d = new Device(d3, adapterInfo.Adapter, DeviceType.Hardware, hwnd, CreateFlags.SoftwareVertexProcessing, parameters);
                        using (SlimDX.Direct3D9.Surface surface = SlimDX.Direct3D9.Surface.CreateOffscreenPlain(d, adapterInfo.CurrentDisplayMode.Width, adapterInfo.CurrentDisplayMode.Height, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Scratch)) {
                            d.GetFrontBufferData(0, surface);

                            // Update: thanks digitalutopia1 for pointing out that SlimDX have fixed a bug
                            // where they previously expected a RECT type structure for their Rectangle
                            bm = new Bitmap(SlimDX.Direct3D9.Surface.ToStream(surface, SlimDX.Direct3D9.ImageFileFormat.Bmp));
                            // Previous SlimDX bug workaround: new Rectangle(region.Left, region.Top, region.Right, region.Bottom)));

                        }
                        //s = Surface.CreateOffscreenPlain(d, this.width, this.height, Format.A8R8G8B8, Pool.Scratch);
                        //d.GetFrontBufferData(0, s);
                        //Bitmap bm = new Bitmap(SlimDX.Direct3D9.Surface.ToStream(s, SlimDX.Direct3D9.ImageFileFormat.Bmp));//as gross as it is temp file method is faster (approx 100ms according to others)
                        return bm;
                }
            }
            catch (Exception) {
                bm = null;
            }
            finally {
                if (d != null)
                    d.Dispose();
                if (s != null)
                    s.Dispose();
                //if (d3 != null)
               //     d3.Dispose();
            }
            return bm;
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

        public void Dispose()
        {
           // if (d != null)
            //    d.Dispose();
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
            get { throw new NotImplementedException(); }
        }

        public override string Description
        {
            get { throw new NotImplementedException(); }
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
                //if (NewScreenAvailEvent != null)
                //    NewScreenAvailEvent(new FastBitmap(CaptureScreen()), EventArgs.Empty);
                Bitmap result = CaptureScreen(FindForegroundPrcs());
                if (NewScreenAvailEvent != null && result != null) {
                    FastBitmap fast = new FastBitmap(result);
                    try {
                        fast.Lock();
                        NewScreenAvailEvent(fast, EventArgs.Empty);
                    }
                    finally {
                        fast.Dispose();
                    }
                }
                Thread.Sleep(150);
            }
        }

        public override bool Start()
        {
            this.driver = new Thread(new ThreadStart(target));
            this.running = true;
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
    }
}
