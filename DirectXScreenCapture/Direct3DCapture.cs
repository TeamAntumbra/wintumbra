using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using Capture;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Bitmaps;
using System.Reflection;
using System.Windows.Forms;

namespace DirectXScreenCapture
{
    [Export(typeof(GlowExtension))]
    public class Direct3DCapture : GlowScreenGrabber, Loggable, ToolbarNotificationSource, GlowCommandSender
    {
        public delegate void NewScreenAvail(Bitmap screen, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;
        public delegate void NewToolbarNotif(int time, String title, String msg, int icon);
        public event NewToolbarNotif NewToolbarNotifEvent;
        public delegate void NewGlowCommand(GlowCommand command);
        public event NewGlowCommand NewGlowCommandEvent;
        private DXSettingsWindow settings;
        private int devId;
        public override bool IsDefault
        {
            get { return false; }
        }

        public override Guid id
        {
            get { return Guid.Parse("ae53796b-ac50-4cef-a335-2d75dea9f1ea"); }
        }
        /// <summary>
        /// The name of the current plugin
        /// </summary>
        public override string Name
        {
            get { return "Direct3D Capture"; }
        }

        /// <summary>
        /// The author of this plugin
        /// </summary>
        public override string Author//built off of code from the AfterGlow project made by Jono C. and Justin S.
        {
            get { return "Team Antumbra"; }
        }
        
        /// <summary>
        /// A description of this plugin
        /// </summary>
        public override string Description
        {
            get { return "A screen capture plugin for Direct3D 9/10/11 applications"; }
        }

        public override string Website
        {
            get { return "https://github.com/FrozenPickle/Afterglow"; }//TODO change and include reference to afterglow for credit, as well as others
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override void AttachObserver(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += new NewScreenAvail(observer.NewBitmapAvail);
        }

        public override bool Settings()
        {
            this.settings = new DXSettingsWindow(this);
            this.settings.Show();
            return true;
        }

        public override bool IsRunning
        {
            get { return !this._stopped; }
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgEvent += new NewLogMsg(observer.NewLogMsgAvail);
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            this.NewGlowCommandEvent += observer.NewGlowCommandAvail;
        }

        public void RegisterDevice(int id)
        {
            this.devId = id;
        }

        private Capture.Interface.CaptureInterface _captureInterface;
        private CaptureProcess _capturedProcess;
        public Process TargetProcess { get; set; }
        private volatile bool _stopped = false;
        private global::Capture.Interface.Screenshot _currentResponse;
        private long _captures;
        private Task driver;
        
        public override bool Start()
        {
            _captureInterface = new Capture.Interface.CaptureInterface();
            _captureInterface.RemoteMessage += (message) => NewLogMsgEvent(this.Name, message.ToString());
            _stopped = false;
            this.driver = new Task(target);
            this.driver.Start();
            return true;
        }
        /// <summary>
        /// Find process that is in the foreground or return null if none found
        /// </summary>
        /// <returns>Process in foreground or null</returns>
        private Process FindForegroundPrcs()
        {
            foreach (Process prc in Process.GetProcesses()) {
                IntPtr handle = prc.MainWindowHandle;
                if (NativeMethods.IsWindowInForeground(handle))
                    return prc;
            }
            return null;
        }

        private void target()
        {
            try {
                NewToolbarNotifEvent(4000, "Open DX App",
                    "Please open your desired DX application."
                  + "In 10 seconds time the foreground application will attempt "
                  + "to be hooked for screen capture.", 0);
                Thread.Sleep(10000);
                this.TargetProcess = FindForegroundPrcs();
                Inject();
            }
            catch (Exception e) {
                NewToolbarNotifEvent(3000, "Exception Occured",
                    "A " + e.Message + " occured when attempting to find the foreground process"
                + "and hook into it for screen capture. Stopping Glow device.", 2);
                NewLogMsgEvent(this.ToString(), this.TargetProcess.ToString() + " - " + e.ToString());
                NewGlowCommandEvent(new StopCommand(this.devId));
            }
            while (this.IsRunning) {
                try {
                    Capture();
                    if (null != _capturedImage)
                        NewScreenAvailEvent(_capturedImage, EventArgs.Empty);
                }
                catch (Exception e) {
                    NewLogMsgEvent(this.ToString(), this.TargetProcess.ToString() + " - " + e.ToString());
                }
                finally {
                    ReleaseCapture();
                }
                Task.Delay(10);//throttle capture
            }
        }

        #region Injection methods
        int _processId = 0;
        Process _process;
        private void Inject()
        {
            bool newInstanceFound = false;

            while (!newInstanceFound)//looking
            {
                if (_stopped) break;
                // If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
                if (this.TargetProcess.MainWindowHandle == IntPtr.Zero)
                    continue;
                _processId = this.TargetProcess.Id;
                _process = this.TargetProcess;
                _capturedProcess = new CaptureProcess(this.TargetProcess, new Capture.Interface.CaptureConfig{
                    ShowOverlay = false, Direct3DVersion = global::Capture.Interface.Direct3DVersion.AutoDetect
                }, _captureInterface);
                newInstanceFound = true;
            }
        }
        
        #endregion

        public override bool Stop()
        {
            _stopped = true;
            if (this.settings != null)
                this.settings.Dispose();
            if (this.driver != null) {
                if (!this.driver.IsCompleted)
                    this.driver.Wait(3000);
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
            }
            if (_capturedProcess != null)
            {
                _capturedProcess.CaptureInterface.Disconnect();
                _capturedProcess.Dispose();
            }
            ReleaseCapture();
            return true;
        }

        private Bitmap _capturedImage = null;
        public Bitmap Capture() 
        {

            Capture.Interface.Screenshot response = null;

            if (_capturedProcess != null && _capturedProcess.CaptureInterface != null)
            {
                response = _capturedProcess.CaptureInterface.GetScreenshot();
            }

            if (response != null)
            {
                Interlocked.Increment(ref _captures);
            }

            Interlocked.Exchange(ref _currentResponse, response);
            if (_currentResponse != null) {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(_currentResponse.CapturedBitmap))
                    _capturedImage = new Bitmap(ms);
            }
            response.Dispose();
            return _capturedImage;
        }

        public void ReleaseCapture()
        {

            if (_capturedImage != null)
            {
                _capturedImage.Dispose();
                _capturedImage = null;
            }
        }
    }

    [System.Security.SuppressUnmanagedCodeSecurity()]
    internal static class NativeMethods
    {
        internal static bool IsWindowInForeground(IntPtr hWnd)
        {
            return hWnd == GetForegroundWindow();
        }

        #region user32

        #region ShowWindow
        /// <summary>Shows a Window</summary>
        /// <remarks>
        /// <para>To perform certain special effects when showing or hiding a
        /// window, use AnimateWindow.</para>
        ///<para>The first time an application calls ShowWindow, it should use
        ///the WinMain function's nCmdShow parameter as its nCmdShow parameter.
        ///Subsequent calls to ShowWindow must use one of the values in the
        ///given list, instead of the one specified by the WinMain function's
        ///nCmdShow parameter.</para>
        ///<para>As noted in the discussion of the nCmdShow parameter, the
        ///nCmdShow value is ignored in the first call to ShowWindow if the
        ///program that launched the application specifies startup information
        ///in the structure. In this case, ShowWindow uses the information
        ///specified in the STARTUPINFO structure to show the window. On
        ///subsequent calls, the application must call ShowWindow with nCmdShow
        ///set to SW_SHOWDEFAULT to use the startup information provided by the
        ///program that launched the application. This behavior is designed for
        ///the following situations: </para>
        ///<list type="">
        ///    <item>Applications create their main window by calling CreateWindow
        ///    with the WS_VISIBLE flag set. </item>
        ///    <item>Applications create their main window by calling CreateWindow
        ///    with the WS_VISIBLE flag cleared, and later call ShowWindow with the
        ///    SW_SHOW flag set to make it visible.</item>
        ///</list></remarks>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="nCmdShow">Specifies how the window is to be shown.
        /// This parameter is ignored the first time an application calls
        /// ShowWindow, if the program that launched the application provides a
        /// STARTUPINFO structure. Otherwise, the first time ShowWindow is called,
        /// the value should be the value obtained by the WinMain function in its
        /// nCmdShow parameter. In subsequent calls, this parameter can be one of
        /// the WindowShowStyle members.</param>
        /// <returns>
        /// If the window was previously visible, the return value is nonzero.
        /// If the window was previously hidden, the return value is zero.
        /// </returns>
        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        /// <summary>Enumeration of the different ways of showing a window using
        /// ShowWindow</summary>
        internal enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized
            /// or maximized, the system restores it to its original size and
            /// position. An application should specify this flag when displaying
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position.
            /// This value is similar to "ShowNormal", except the window is not
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is
            /// minimized or maximized, the system restores it to its original size
            /// and position. An application should specify this flag when restoring
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
            /// that owns the window is hung. This flag should only be used when
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }
        #endregion

        /// <summary>
        /// The GetForegroundWindow function returns a handle to the foreground window.
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsIconic(IntPtr hWnd);

        #endregion
    }
}
