using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FlatTabControl {

    internal class Win32 {
        /*
         * GetWindow() Constants
         */

        #region Public Fields

        public const int GW_CHILD = 5;
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GWL_WNDPROC = -4;
        public const int HC_ACTION = 0;
        public const int WH_CALLWNDPROC = 4;
        public const int WM_CREATE = 0x1;
        public const int WM_DESTROY = 0x2;
        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_NCCREATE = 0x81;
        public const int WM_NCPAINT = 0x85;
        public const int WM_PAINT = 0xF;
        public const int WM_PRINT = 0x317;
        public const int WM_SHARED_MENU = 0x1E2;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_WINDOWPOSCHANGING = 0x46;

        #endregion Public Fields

        #region Public Constructors

        public Win32() {
            // TODO: Add constructor logic here
        }

        #endregion Public Constructors

        #region Public Methods

        [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hwnd, [In, Out] ref Rectangle rect);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindow(IntPtr hwnd, int uCmd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr handle);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hwnd, ref Rectangle rect, bool bErase);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDC);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool UpdateWindow(IntPtr hwnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool ValidateRect(IntPtr hwnd, ref Rectangle rect);

        #endregion Public Methods

        #region Internal Methods

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

        #endregion Internal Methods

        #region Public Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS {
            public RECT rgc;
            public WINDOWPOS wndpos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS {
            public IntPtr hwnd;
            public IntPtr hwndAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        #endregion Public Structs
    }

    #region SubClass Classing Handler Class

    internal class SubClass : System.Windows.Forms.NativeWindow {

        #region Private Fields

        private bool IsSubClassed = false;

        #endregion Private Fields

        #region Public Constructors

        public SubClass(IntPtr Handle, bool _SubClass) {
            base.AssignHandle(Handle);
            this.IsSubClassed = _SubClass;
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate int SubClassWndProcEventHandler(ref System.Windows.Forms.Message m);

        #endregion Public Delegates

        #region Public Events

        public event SubClassWndProcEventHandler SubClassedWndProc;

        #endregion Public Events

        #region Public Properties

        public bool SubClassed {
            get { return this.IsSubClassed; }
            set { this.IsSubClassed = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public void CallDefaultWndProc(ref Message m) {
            base.WndProc(ref m);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void WndProc(ref Message m) {
            if(this.IsSubClassed) {
                if(OnSubClassedWndProc(ref m) != 0)
                    return;
            }
            base.WndProc(ref m);
        }

        #endregion Protected Methods

        #region HiWord Message Cracker

        public int HiWord(int Number) {
            return ((Number >> 16) & 0xffff);
        }

        #endregion HiWord Message Cracker

        #region LoWord Message Cracker

        public int LoWord(int Number) {
            return (Number & 0xffff);
        }

        #endregion LoWord Message Cracker

        #region MakeLong Message Cracker

        public int MakeLong(int LoWord, int HiWord) {
            return (HiWord << 16) | (LoWord & 0xffff);
        }

        #endregion MakeLong Message Cracker

        #region MakeLParam Message Cracker

        public IntPtr MakeLParam(int LoWord, int HiWord) {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        #endregion MakeLParam Message Cracker

        #region Private Methods

        private int OnSubClassedWndProc(ref Message m) {
            if(SubClassedWndProc != null) {
                return this.SubClassedWndProc(ref m);
            }

            return 0;
        }

        #endregion Private Methods
    }

    #endregion SubClass Classing Handler Class
}
