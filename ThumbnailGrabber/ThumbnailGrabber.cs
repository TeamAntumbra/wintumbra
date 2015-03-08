using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Bitmaps;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace ThumbnailGrabber
{
    [Export(typeof(GlowExtension))]
    public class ThumbnailGrabber : GlowScreenGrabber, AntumbraBitmapSource
    {
        public delegate void NewBitMapAvail(Bitmap bm, EventArgs args);
        public event NewBitMapAvail NewBitMapAvailEvent;
        private IntPtr window;
        private Task driver;
        private bool running;
        public override string Name
        {
            get { return "Thumbnail Grabber"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A screen grabber which uses the Desktop Window Manager's ability" +
                "to grab a thumbnail image of the foreground window"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override bool IsDefault
        {
            get { return false; }
        }

        public override void AttachBitmapObserver(Antumbra.Glow.Observer.Bitmaps.AntumbraBitmapObserver observer)
        {
            NewBitMapAvailEvent += observer.NewBitmapAvail;
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Settings()
        {
            return false;
        }

        public override bool Start()
        {
            this.running = true;
            this.driver = new Task(loopTarget);
            this.form = new Form();
            this.form.Visible = true;//false;
            this.box = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.box)).BeginInit();
            this.form.SuspendLayout();
            this.box.Location = new Point(0, 0);
            this.box.Size = new Size(50, 50);//TODO make configurable
            this.form.Controls.Add(this.box);
            ((System.ComponentModel.ISupportInitialize)(this.box)).EndInit();
            this.form.ResumeLayout(false);
            this.form.PerformLayout();
            this.driver.Start();
            return true;
        }

        public override bool Stop()
        {
            this.running = false;
            if (this.targetWindow != null)
                DwmUnregisterThumbnail(this.targetWindow);
            if (this.driver != null) {
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
                else {
                    this.driver.Wait(2000);
                    if (this.driver.IsCompleted)
                        this.driver.Dispose();
                    else
                        return false;
                }
            }
            return true;
        }

        private IntPtr targetWindow;
        private PictureBox box;
        private IntPtr thumb;
        private Form form;

        private void loopTarget()
        {
            Thread.Sleep(5000);
            this.targetWindow = NativeMethods.GetForegroundWindow();
            int resp = DwmRegisterThumbnail(this.form.Handle, this.targetWindow, out thumb);
            if (resp != 0) {
                Console.WriteLine("TODO report this");
                return;//TODO add sending of stop command
            }
            while (this.running) {
                UpdateThumb();
            }
        }

        #region Constants

        static readonly int GWL_STYLE = -16;

        static readonly int DWM_TNP_VISIBLE = 0x8;
        static readonly int DWM_TNP_OPACITY = 0x4;
        static readonly int DWM_TNP_RECTDESTINATION = 0x1;

        static readonly ulong WS_VISIBLE = 0x10000000L;
        static readonly ulong WS_BORDER = 0x00800000L;
        static readonly ulong TARGETWINDOW = WS_BORDER | WS_VISIBLE;

        #endregion

        #region DWM functions

        [DllImport("dwmapi.dll")]
        static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out PSIZE size);

        [DllImport("dwmapi.dll")]
        static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        #endregion

        #region Win32 helper functions

        [DllImport("user32.dll")]
        static extern ulong GetWindowLongA(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);
        delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        #endregion


        private void UpdateThumb()
        {
            if (thumb != IntPtr.Zero) {
                PSIZE size;
                DwmQueryThumbnailSourceSize(thumb, out size);
                DWM_THUMBNAIL_PROPERTIES props = new DWM_THUMBNAIL_PROPERTIES();

                props.fVisible = true;
                props.dwFlags = DWM_TNP_VISIBLE | DWM_TNP_RECTDESTINATION | DWM_TNP_OPACITY;
                props.opacity = 255;//full
                props.rcDestination = new Rect(box.Left, box.Top, box.Right, box.Bottom);

                if (size.x < box.Width)
                    props.rcDestination.Right = props.rcDestination.Left + size.x;

                if (size.y < box.Height)
                    props.rcDestination.Bottom = props.rcDestination.Top + size.y;

                DwmUpdateThumbnailProperties(thumb, ref props);
            }
        }
    }

    #region Interop structs

    [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_THUMBNAIL_PROPERTIES
        {
            public int dwFlags;
            public Rect rcDestination;
            public Rect rcSource;
            public byte opacity;
            public bool fVisible;
            public bool fSourceClientAreaOnly;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Rect
        {
            internal Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PSIZE
        {
            public int x;
            public int y;
        }

        #endregion
        internal class NativeMethods
        {
            /// <summary>
            /// The GetForegroundWindow function returns a handle to the foreground window.
            /// </summary>
            [DllImport("user32.dll")]
            internal static extern IntPtr GetForegroundWindow();
        }
}
