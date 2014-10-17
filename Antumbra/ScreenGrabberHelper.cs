using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Capture;
using Capture.Hook;
using Capture.Interface;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Drawing;
using EasyHook;

namespace Antumbra
{
    public class ScreenGrabberHelper
    {
        CaptureProcess captPrcss;
        Process prcss;
        int processId;
        int x, y, width, height, timeOut;
        Thread captThread;//thread to make & send screenshot requests
        public Bitmap screen { get; private set; }//where the current capture will live
        public ScreenGrabberHelper(int x, int y, int width, int height, int timeOut)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.timeOut = timeOut;
            this.screen = null;//start as null (no screenies taken)
            getReady();
        }

        public void start()//start capturing
        {
            AttachProcess("blah_blah");
            captThread.Start();
        }

        public void stop()//stop capturing
        {
            captThread.Abort();
            getReady();
        }

        private void takeScreenie()
        {
            MakeRequest();
        }

        private void getReady()
        {
            captThread = new Thread(new ThreadStart(takeScreenie));
        }

        private void AttachProcess(String exe)
        {
            String exeName = Path.GetFileNameWithoutExtension(exe);
            //Process[] processes = Process.GetProcessesByName(exeName);
            Process[] processes = Process.GetProcesses();//get all running processes
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
                };

                processId = process.Id;
                prcss = process;

                var captureInterface = new CaptureInterface();
                //captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);//TODO do we need this?
                captPrcss = new CaptureProcess(process, cc, captureInterface);

                break;
            }
            if (captPrcss == null) {
                Console.WriteLine("No executable found matching: '" + exeName + "'");
            }
        }

        private void MakeScreenShotRequest(int x, int y, int width, int height, int secTimeOut)
        {//a width of zero means capture the whole window
            captPrcss.BringProcessWindowToFront();
            // Initiate the screenshot of the CaptureInterface, the appropriate event handler within the target process will take care of the rest
            captPrcss.CaptureInterface.BeginGetScreenshot(new Rectangle(x, y, width, height), new TimeSpan(0, 0, secTimeOut), Callback);
        }

        private void MakeRequest()
        {
            MakeScreenShotRequest(this.x, this.y, this.width, this.height, this.timeOut);
        }

        private void inject(bool autoRegGAC, String exe)
        {
            if (captPrcss == null) {

                if (autoRegGAC) {
                    // NOTE: On some 64-bit setups this doesn't work so well.
                    //       Sometimes if using a 32-bit target, it will not find the GAC assembly
                    //       without a machine restart, so requires manual insertion into the GAC
                    // Alternatively if the required assemblies are in the target applications
                    // search path they will load correctly.

                    // Must be running as Administrator to allow dynamic registration in GAC
                    Config.Register("Capture", "Capture.dll");
                }

                AttachProcess(exe);
            }
            else {
                HookManager.RemoveHookedProcess(captPrcss.Process.Id);
                captPrcss.CaptureInterface.Disconnect();
                captPrcss = null;
            }
        }

        /// <summary>
        /// The callback for when the screenshot has been taken
        /// </summary>
        /// <param name="clientPID"></param>
        /// <param name="status"></param>
        /// <param name="screenshotResponse"></param>
        void Callback(IAsyncResult result)
        {
            using (Screenshot screenshot = captPrcss.CaptureInterface.EndGetScreenshot(result))
                try {
                    if (screenshot != null && screenshot.CapturedBitmap != null) {
                        this.screen = screenshot.CapturedBitmap.ToBitmap();//update current screen
                    }

                    Thread t = new Thread(new ThreadStart(MakeRequest));
                    t.Start();
                }
                catch { }
        }
    }
}
