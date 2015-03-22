using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.View;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Antumbra.Glow.Utility;

namespace Antumbra.Glow.Controller
{
    public class MainWindowController : Loggable, ToolbarNotificationSource, GlowCommandSender
    {
        public delegate void NewLogMsgAvail(String source, String msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public delegate void NewToolbarNotifAvail(int time, string title, string msg, int icon);
        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;
        public delegate void NewGlowCmdAvail(GlowCommand cmd);
        public event NewGlowCmdAvail NewGlowCmdAvailEvent;
        public event EventHandler quitEventHandler;
        public bool goodStart { get; private set; }
        private const string extPath = "./Extensions/";
        private MainWindow window;
        private int id;
        public MainWindowController()
        {
            this.AttachObserver((LogMsgObserver)(LoggerHelper.GetInstance()));//attach logger
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerModeChanged);
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
            this.window.quitBtn_ClickEvent += new EventHandler(quitBtnClicked);
        }

        public void RegisterDevice(int id)
        {
            this.id = id;
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            this.NewGlowCmdAvailEvent += observer.NewGlowCommandAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        private void ShowMessage(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
        }

        private void Log(string msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("MainWindowController", msg);
        }

        public void showWindowEventHandler(object sender, EventArgs args)
        {
            this.showWindow();
        }

        private void showWindow()
        {
            this.window.Show();
        }

        public void closeBtnClicked(object sender, EventArgs args)
        {
            this.window.Hide();
        }

        public void colorWheelColorChanged(object sender, EventArgs args)
        {
            if (sender is HslColor) {
                NewGlowCmdAvailEvent(new StopCommand(this.id));//stop device if running (dev mgr will make check)
                HslColor col = (HslColor)sender;
                NewGlowCmdAvailEvent(new SendColorCommand(this.id, col.ToRgbColor()));
            }
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

        public void quitBtnClicked(object sender, EventArgs args)
        {
            this.window.Close();
            NewGlowCmdAvailEvent(new PowerOffCommand(-1));//turn all devices off
            if (quitEventHandler != null)
                quitEventHandler(sender, args);
        }

        /// <summary>
        /// Event handler for session switching. Used for handling locking and unlocking of the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason) {
                case SessionSwitchReason.SessionLogoff:
                case SessionSwitchReason.SessionLock:
                    //this.Off();
                    Console.WriteLine("locked/logged off");
                    break;
                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                    Console.WriteLine("unlocked/logged on");
                   // StartAllAfterDelay();
                    break;
            }
        }
        /// <summary>
        /// Event handler for PowerModeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            // User is putting the system into standby 
            if (e.Mode == PowerModes.Suspend) {
                //this.Off();
                Console.WriteLine("Suspended.");
            }
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
