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

namespace Antumbra.Glow.Controller
{
    public class ToolbarIconController : Loggable, ToolbarNotificationObserver, ToolbarNotificationSource, IDisposable
    {
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewLogMsgAvail(string source, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        private const string extPath = "./Extensions/";
        private Antumbra.Glow.View.ToolbarIcon toolbarIcon;
        public bool failed { get; private set; }
        public ToolbarIconController()
        {
            this.failed = false;
            this.toolbarIcon = new Antumbra.Glow.View.ToolbarIcon();
            this.toolbarIcon.Hide();
            this.AttachObserver(this.toolbarIcon);
            MainWindowController mainController = new MainWindowController();
            mainController.AttachObserver(this);
            this.failed = !mainController.Setup(this.toolbarIcon.ProductVersion, new EventHandler(Quit));
            if (!this.failed) {//continue if so far so good
                this.toolbarIcon.notifyIcon_MouseClickEvent += new EventHandler(mainController.showWindowEventHandler);
                this.AttachObserver(LoggerHelper.GetInstance());
                this.LogMsg("Wintumbra starting @ " + DateTime.Now.ToString());
                NewToolbarNotifAvail(1000, "Click to Open", "Click the Antumbra logo to open the main " +
                    "application window.", 0);
            }
        }

        private void Quit(object sender, EventArgs args)
        {
            this.toolbarIcon.Dispose();
            System.Windows.Forms.Application.Exit();
        }

        private void LogMsg(String msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("ToolbarIconController", msg);
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        private void Log(string msg)
        {
            NewLogMsgAvailEvent("ToolbarIconController", msg);
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
        }

        public void Dispose()
        {
            this.toolbarIcon.Dispose();
        }
    }
}
