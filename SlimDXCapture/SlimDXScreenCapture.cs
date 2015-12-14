using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ScreenInfo;
using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace SlimDXCapture {

    [Export(typeof(GlowExtension))]
    public class SlimDXScreenCapture : GlowScreenGrabber, AntumbraScreenInfoSource, Loggable, IDisposable {

        #region Private Fields

        private readonly object sync = new object();

        private int deviceId;

        private Thread driver;

        private Dictionary<Device, Rectangle> dxDevs;

        private bool running = false;

        private int screenCount;

        #endregion Private Fields

        #region Public Constructors

        public SlimDXScreenCapture() {
            dxDevs = new Dictionary<Device, Rectangle>();
            var screens = new List<Rectangle>();
            foreach(var screen in System.Windows.Forms.Screen.AllScreens) {
                screens.Add(screen.Bounds);
            }
            screens.Sort((x, y) => x.X.CompareTo(y.X));

            for(int i = 0; i < screens.Count; i += 1) {
                AttachObserver(LoggerHelper.GetInstance());
                PresentParameters present_params = new PresentParameters();
                present_params.Windowed = true;
                present_params.SwapEffect = SwapEffect.Discard;
                int index = screens.IndexOf(System.Windows.Forms.Screen.AllScreens[i].Bounds);
                Device dev = new Device(new Direct3D(), index, DeviceType.Hardware, IntPtr.Zero, CreateFlags.SoftwareVertexProcessing, present_params);
                dxDevs.Add(dev, screens[index]);
            }
            Console.WriteLine(dxDevs);
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewLogMsg(String source, String msg);

        public delegate void NewScreenAvail(List<int[, ,]> pixels, EventArgs args);

        #endregion Public Delegates

        #region Public Events

        public event NewLogMsg NewLogMsgEvent;

        public event NewScreenAvail NewScreenAvailEvent;

        #endregion Public Events

        #region Public Properties

        public override string Author {
            get { return "Team Antumbra"; }
        }

        public override string Description {
            get { return "An all-purpose screen capture plugin."; }//TODO test this statement!
        }

        public override int devId {
            get {
                return deviceId;
            }
            set {
                deviceId = value;
            }
        }

        public override Guid id {
            get { return Guid.Parse("ad1e6255-d9b4-4e6d-995f-a094e6ea5f7b"); }
        }

        public override bool IsDefault {
            get { return false; }
        }

        public override bool IsRunning {
            get { return this.running; }
        }

        public override string Name {
            get { return "DxScreenCapture"; }
        }

        public override Version Version {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website {
            get { throw new NotImplementedException(); }
        }

        #endregion Public Properties

        #region Public Methods

        public override void AttachObserver(AntumbraScreenInfoObserver observer) {
            this.NewScreenAvailEvent += observer.NewScreenInfoAvail;
        }

        public void AttachObserver(LogMsgObserver observer) {
            this.NewLogMsgEvent += observer.NewLogMsgAvail;
        }

        public override GlowScreenGrabber Create() {
            return new SlimDXScreenCapture();
        }

        public override void Dispose() {
            if(driver != null && driver.IsAlive) {
                driver.Abort();
            }

            foreach(Device dev in dxDevs.Keys) {
                dev.Dispose();
            }
        }

        public override bool Settings() {
            return false;
        }

        public override bool Start() {
            this.running = true;
            this.driver = new Thread(new ThreadStart(target));
            this.driver.Start();
            return true;
        }

        public override bool Stop() {
            if(this.driver != null && this.driver.IsAlive)
                this.driver.Abort();
            this.Dispose();
            this.running = false;
            return true;
        }

        #endregion Public Methods

        #region Private Methods

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
            }
            return result;
        }

        private void Log(String msg) {
            if(NewLogMsgEvent != null)
                NewLogMsgEvent("SlimDXCapture", msg);
        }

        private int[, ,] SurfaceToDataArray(Surface surface) {
            DataRectangle rect = surface.LockRectangle(LockFlags.None);
            DataStream data = rect.Data;
            int width = surface.Description.Width;
            int height = surface.Description.Height;
            int skipWidth = width / 25;
            int skipHeight = height / 25;
            int[, ,] result = new int[skipWidth, skipHeight, 3];
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
            data.Dispose();
            surface.Dispose();
            return result;
        }

        private void target() {
            while(this.IsRunning) {
                lock(sync) {
                    Thread.Sleep(captureThrottle);
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

                    surfaces.Clear();
                }
            }
        }

        #endregion Private Methods

        #region Internal Classes

        [System.Security.SuppressUnmanagedCodeSecurity()]
        internal static class NativeMethods {

            #region Internal Methods

            [DllImport("user32.dll")]
            internal static extern IntPtr GetForegroundWindow();

            internal static bool IsWindowInForeground(IntPtr hWnd) {
                return hWnd == GetForegroundWindow();
            }

            #endregion Internal Methods
        }

        #endregion Internal Classes
    }
}
