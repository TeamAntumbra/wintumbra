using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Bitmaps;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace SlimDXCapture
{
    [Export(typeof(GlowExtension))]
    public class SlimDXScreenCapture : GlowScreenGrabber, AntumbraBitmapSource, Loggable, IDisposable
    {
        public delegate void NewScreenAvail(int[,,] pixels, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;

        private int deviceId;
        private Thread driver;
        private bool running = false;
        private Device dxDev;
        private int screensWidth, screensHeight;

        public SlimDXScreenCapture()
        {
            foreach(var screen in System.Windows.Forms.Screen.AllScreens) {
                screensWidth += screen.Bounds.Width;
                screensHeight = height > screen.Bounds.Height ? height : screen.Bounds.Height;
            }
            AttachObserver(LoggerHelper.GetInstance());
            PresentParameters present_params = new PresentParameters();
            present_params.Windowed = true;
            present_params.SwapEffect = SwapEffect.Discard;
            dxDev = new Device(new Direct3D(), 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.SoftwareVertexProcessing, present_params);
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

        private Surface CaptureScreen() {
            Surface s = Surface.CreateOffscreenPlain(dxDev, screensWidth, screensHeight, Format.A8R8G8B8, Pool.Scratch);
            try {
                dxDev.GetFrontBufferData(0, s);
            } catch(SlimDXException ex) {
                Log(ex.Message + '\n' + ex.StackTrace);
            }
            return s;
        }

        private void Log(String msg)
        {
            if (NewLogMsgEvent != null)
                NewLogMsgEvent("SlimDXCapture", msg);
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
                Surface surface = CaptureScreen();
                if (NewScreenAvailEvent != null && surface != null) {
                    NewScreenAvailEvent(SurfaceToDataArray(surface), EventArgs.Empty);
                    surface.Dispose();
                }
            }
        }

        private int[,,] SurfaceToDataArray(Surface surface) {
            DataRectangle rect = surface.LockRectangle(LockFlags.None);
            DataStream data = rect.Data;
            int width = surface.Description.Width;
            int height = surface.Description.Height;
            int[,,] result = new int[width / 25, width / 25, 3];
            int resX = 0, resY = 0;

            for(int x = 25; x < width - 25; x += 25) {
                for(int y = 25; y < height - 25; y += 25) {
                    data.Position = (y * width + x) * 4;
                    byte[] pixel = new byte[4];
                    data.Read(pixel, 0, 4);
                    result[resX, resY, 0] = pixel[2];
                    result[resX, resY, 1] = pixel[1];
                    result[resX, resY, 2] = pixel[0];
                    resY += 1;
                }
                resY = 0;
                resX += 1;
            }
            surface.UnlockRectangle();
            return result;
        }

        public override bool Start()
        {
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
    }
}
