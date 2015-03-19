using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.View;
using Antumbra.Glow.Observer.Logging;
using System.Runtime.InteropServices;

namespace Antumbra.Glow.Controller
{
    public class MainWindowController : Loggable
    {
        public delegate void NewLogMsgAvail(String source, String msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        private MainWindow window;
        public MainWindowController()
        {
            this.window = new MainWindow();
            this.window.closeBtn_ClickEvent += new EventHandler(closeBtnClicked);
            this.window.colorWheel_ColorChangedEvent += new EventHandler(colorWheelColorChanged);
            this.window.brightnessTrackBar_ScrollEvent += new EventHandler(brightnessValueChanged);
            this.window.hsvBtn_ClickEvent += new EventHandler(hsvBtnClicked);
            this.window.sinBtn_ClickEvent += new EventHandler(sinBtnClicked);
            this.window.neonBtn_ClickEvent += new EventHandler(neonBtnClicked);
            this.window.mirrorBtn_ClickEvent += new EventHandler(mirrorBtnClicked);
            this.window.augmentBtn_ClickEvent += new EventHandler(augmentBtnClicked);
            this.window.smoothBtn_ClickEvent += new EventHandler(smoothBtnClicked);
            this.window.gameBtn_ClickEvent += new EventHandler(gameBtnClicked);
            this.window.mainWindow_MouseDownEvent += new System.Windows.Forms.MouseEventHandler(mouseDownEvent);
            this.window.customConfigBtn_ClickEvent += new EventHandler(customConfigBtnClicked);
            this.window.Show();
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            if (this.NewLogMsgAvailEvent != null)
                this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void closeBtnClicked(object sender, EventArgs args)
        {

        }

        public void colorWheelColorChanged(object sender, EventArgs args)
        {

        }

        public void brightnessValueChanged(object sender, EventArgs args)
        {

        }

        public void hsvBtnClicked(object sender, EventArgs args)
        {

        }

        public void sinBtnClicked(object sender, EventArgs args)
        {

        }

        public void neonBtnClicked(object sender, EventArgs args)
        {

        }

        public void mirrorBtnClicked(object sender, EventArgs args)
        {

        }

        public void augmentBtnClicked(object sender, EventArgs args)
        {

        }

        public void smoothBtnClicked(object sender, EventArgs args)
        {

        }

        public void gameBtnClicked(object sender, EventArgs args)
        {

        }

        public void customConfigBtnClicked(object sender, EventArgs args)
        {

        }

        public void mouseDownEvent(object sender, System.Windows.Forms.MouseEventArgs args)
        {
            //allows dragging of forms to move them (because of hidden menu bars and window frame)
            if (args.Button == System.Windows.Forms.MouseButtons.Left) {//drag with left mouse btn
                ReleaseCapture();
                SendMessage(this.window.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Move form dependencies
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
    }
}
