using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.ScreenInfo;
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
    public class SlimDXScreenCapture : GlowScreenGrabber, AntumbraScreenInfoSource, Loggable, IDisposable
    {
        public delegate void NewScreenAvail(List<int[,,]> pixels, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;

        private int deviceId;
        private Thread driver;
        private bool running = false;
        private Dictionary<Device, Rectangle> dxDevs;
        private int screenCount;
        private readonly object sync = new object();

        public SlimDXScreenCapture()
        {
            dxDevs = new Dictionary<Device, Rectangle>();
            screenCount = System.Windows.Forms.Screen.AllScreens.Length;
            for(int i = 0; i < screenCount; i += 1) {//Note: this order is used throughout this class
                AttachObserver(LoggerHelper.GetInstance());
                PresentParameters present_params = new PresentParameters();
                present_params.Windowed = true;
                present_params.SwapEffect = SwapEffect.Discard;
                Rectangle rect = System.Windows.Forms.Screen.AllScreens[i].Bounds;
                Device dev = new Device(new Direct3D(), i, DeviceType.Hardware, IntPtr.Zero, CreateFlags.SoftwareVertexProcessing, present_params);
                dxDevs.Add(dev, rect);
            }
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

        private List<Surface> CaptureScreen() {
            List<Surface> result = new List<Surface>();
            foreach(KeyValuePair<Device, Rectangle> keyValuePair in dxDevs) {
                Device dxDev = keyValuePair.Key;
                Rectangle rect = keyValuePair.Value;
                Surface s = Surface.CreateOffscreenPlain(dxDev, rect.Width, rect.Height, Format.A8R8G8B8, Pool.Scratch);
                try {
                    dxDev.GetFrontBufferData(0, s);
                } catch(SlimDXException ex) {
                    Log(ex.Message + '\n' + ex.StackTrace);
                }
                result.Add(s);
                //Bitmap bm = new Bitmap(Surface.ToStream(s, ImageFileFormat.Bmp));
            }
            return result;
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
            get { return "An all-purpose screen capture plugin."; }//TODO test this statement!
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

        private void target() {
            while(this.IsRunning) {
                lock(sync) {
                    List<Surface> surfaces = CaptureScreen();
                    List<int[, ,]> dataArrays = new List<int[, ,]>();

                    foreach(Surface surface in surfaces) {
                        if(surface != null) {
                            int[, ,] data = SurfaceToDataArray(surface);
                            dataArrays.Add(data);
                        }
                    }

                    if(NewScreenAvailEvent != null) {
                        NewScreenAvailEvent(dataArrays, EventArgs.Empty);
                    }

                    foreach(Surface surface in surfaces) {
                        surface.Dispose();
                    }

                    surfaces.Clear();
                }
            }
        }

        private int[,,] SurfaceToDataArray(Surface surface) {
            DataRectangle rect = surface.LockRectangle(LockFlags.None);
            DataStream data = rect.Data;
            int width = surface.Description.Width;
            int height = surface.Description.Height;
            int skipWidth = width / 25;
            int skipHeight = height / 25;
            int[,,] result = new int[skipWidth, skipHeight, 3];
            int resX = 0, resY = 0;

            for(int x = 25; x < width; x += 25) {
                for(int y = 25; y < height; y += 25) {
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

        public override void AttachObserver(AntumbraScreenInfoObserver observer)
        {
            this.NewScreenAvailEvent += observer.NewScreenInfoAvail;
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
