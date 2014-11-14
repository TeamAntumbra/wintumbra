using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using EasyHook;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.IO;
using Capture.Interface;
using Capture.Hook;
using Capture;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;

namespace AntumbraGlowDirectXScreenDriver
{
    [Export(typeof(GlowExtension))]
    public partial class AntumbraGlowDirectXScreenDriver : GlowScreenGrabber
    {
        public bool tryAuth { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public String exeName { get; set; }
        public delegate void NewScreenAvail(object sender, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        private int processId = 0;
        private Process _process;
        private CaptureProcess _captureProcess;
        private bool Ready;

        public AntumbraGlowDirectXScreenDriver()
        {
            this.exeName = "DecalTessellation11.exe";//TODO take from selection in settings
            this.tryAuth = false;
            this.width = 0;
            this.height = 0;
            this.x = 0;
            this.y = 0;
            this.Ready = false;
        }

        public override string Name
        {
            get { return "Antumbra Glow DirectX Screen Driver"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A DirectX compatiable screen-based driver "
                       + "for Antumbra|Glow. Meant mainly for gaming."; }
        }

        public override string Version
        {
            get { return "V0.0.1"; }
        }

        public override bool ready()
        {
            getReady();
            return this.Ready;
        }

        public override bool start()
        {
            DoRequest();
            return true;
        }

        public override void AttachEvent(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += new NewScreenAvail(observer.NewBitmapAvail);
        }

        private void getReady()
        {
            if (_captureProcess == null) {

                if (false) {
                    // NOTE: On some 64-bit setups this doesn't work so well.
                    //       Sometimes if using a 32-bit target, it will not find the GAC assembly
                    //       without a machine restart, so requires manual insertion into the GAC
                    // Alternatively if the required assemblies are in the target applications
                    // search path they will load correctly.

                    // Must be running as Administrator to allow dynamic registration in GAC
                    Config.Register("Capture",
                        "Capture.dll");
                }

                AttachProcess(this.exeName);
            }
            else {
                HookManager.RemoveHookedProcess(_captureProcess.Process.Id);
                _captureProcess.CaptureInterface.Disconnect();
                _captureProcess = null;
            }

            if (_captureProcess != null) {
                this.Ready = true;
            }
            else {
                this.Ready = false;
            }
        }

        private void inject()
        {
            if (_captureProcess == null) {

                if (tryAuth) {
                    // NOTE: On some 64-bit setups this doesn't work so well.
                    //       Sometimes if using a 32-bit target, it will not find the GAC assembly
                    //       without a machine restart, so requires manual insertion into the GAC
                    // Alternatively if the required assemblies are in the target applications
                    // search path they will load correctly.

                    // Must be running as Administrator to allow dynamic registration in GAC
                    Config.Register("Capture",
                        "Capture.dll");
                }

                AttachProcess(this.exeName);
            }
            else {
                HookManager.RemoveHookedProcess(_captureProcess.Process.Id);
                _captureProcess.CaptureInterface.Disconnect();
                _captureProcess = null;
            }
        }

        private void AttachProcess(String exe)
        {
            string exeName = Path.GetFileNameWithoutExtension(exe);

            Process[] processes = Process.GetProcessesByName(exeName);
            foreach (Process process in processes) {
                // Simply attach to the first one found.

                // If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
                if (process.MainWindowHandle == IntPtr.Zero) {
                    continue;
                }

                // Skip if the process is already hooked (and we want to hook multiple applications)
                if (HookManager.IsHooked(process.Id)) {
                    continue;
                }

                Direct3DVersion direct3DVersion = Direct3DVersion.AutoDetect;

                CaptureConfig cc = new CaptureConfig()
                {
                    Direct3DVersion = direct3DVersion,
                    ShowOverlay = false
                };

                processId = process.Id;
                _process = process;

                var captureInterface = new CaptureInterface();
                captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);
                _captureProcess = new CaptureProcess(process, cc, captureInterface);

                break;
            }
            //Thread.Sleep(10);

            if (_captureProcess == null) {
                Console.WriteLine("No executable found matching: '" + exeName + "'");
                this.Ready = false;
            }
            else {
                this.Ready = true;
            }
        }

        /// <summary>
        /// Display messages from the target process
        /// </summary>
        /// <param name="message"></param>
        void CaptureInterface_RemoteMessage(MessageReceivedEventArgs message)
        {
            Console.WriteLine(message.ToString());
        }

        /// <summary>
        /// Display debug messages from the target process
        /// </summary>
        /// <param name="clientPID"></param>
        /// <param name="message"></param>
        void ScreenshotManager_OnScreenshotDebugMessage(int clientPID, string message)
        {
            Console.WriteLine(clientPID.ToString() + "  " + message);
        }

        DateTime Start;
        DateTime end;

        private void btnCapture_Click(object sender, EventArgs e)
        {
            //Start = DateTime.Now;

            DoRequest();
        }

        private void btnLoadTest_Click(object sender, EventArgs e)
        {
            // Note: we bring the target application into the foreground because
            //       windowed Direct3D applications have a lower framerate 
            //       if not the currently focused window
            _captureProcess.BringProcessWindowToFront();
            //Start = DateTime.Now;
            DoRequest();
        }

        /// <summary>
        /// Create the screen shot request
        /// </summary>
        void DoRequest()
        {
            _captureProcess.BringProcessWindowToFront();
            // Initiate the screenshot of the CaptureInterface, the appropriate event handler within the target process will take care of the rest
            _captureProcess.CaptureInterface.BeginGetScreenshot(new Rectangle(this.x, this.y, this.width, this.height), new TimeSpan(0, 0, 2), Callback);
            end = DateTime.Now;
            Console.WriteLine(end.ToString());
        }

        /// <summary>
        /// The callback for when the screenshot has been taken
        /// </summary>
        /// <param name="clientPID"></param>
        /// <param name="status"></param>
        /// <param name="screenshotResponse"></param>
        void Callback(IAsyncResult result)
        {
            using (Screenshot screenshot = _captureProcess.CaptureInterface.EndGetScreenshot(result)) {
                try {
                    if (screenshot != null && screenshot.CapturedBitmap != null) {
                        Console.WriteLine("OMG OMG OMG OMG");
                        NewScreenAvailEvent(screenshot.CapturedBitmap.ToBitmap(), EventArgs.Empty);
                    }

                    Thread t = new Thread(new ThreadStart(DoRequest));//start next
                    t.Start();
                }
                catch {
                }
            }
        }
    }
}
