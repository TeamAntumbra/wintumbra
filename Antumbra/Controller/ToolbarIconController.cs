using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Settings;
using System.Drawing;

namespace Antumbra.Glow.Controller {
    public class ToolbarIconController : Loggable, ToolbarNotificationObserver, ToolbarNotificationSource, IDisposable {
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewLogMsgAvail(string source, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        private const string extPath = "./Extensions/";
        private Antumbra.Glow.View.ToolbarIcon toolbarIcon;
        public ToolbarIconController() {
            AttachObserver(LoggerHelper.GetInstance());
            toolbarIcon = new Antumbra.Glow.View.ToolbarIcon();
            toolbarIcon.Hide();
            AttachObserver(toolbarIcon);
            MainWindowController mainController = new MainWindowController(toolbarIcon.ProductVersion, new EventHandler(Quit));
            mainController.AttachObserver((ToolbarNotificationObserver)this);

            toolbarIcon.notifyIcon_MouseClickEvent += new EventHandler(mainController.showWindowEventHandler);
            toolbarIcon.notifyIcon_DoubleClickEvent += new EventHandler(mainController.restartEventHandler);
            LogMsg("ToolbarIconController", "Wintumbra starting @ " + DateTime.Now.ToString());
            NewToolbarNotifAvail(1000, "Click to Open", "Click the Antumbra logo to open the main " +
                "application window.", 0);
        }

        private void Quit(object sender, EventArgs args) {
            Dispose();
            System.Windows.Forms.Application.Exit();
        }

        private void LogMsg(string source, string msg) {
            if(NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent(source, msg);
            }
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer) {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon) {
            if(NewToolbarNotifAvailEvent != null) {
                NewToolbarNotifAvailEvent(time, title, msg, icon);
                LogMsg(title, msg);
            }
        }

        public void Dispose() {
            toolbarIcon.Dispose();
        }
    }
}
