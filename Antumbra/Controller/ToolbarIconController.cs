using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using System;
using System.Threading;

namespace Antumbra.Glow.Controller {

    public class ToolbarIconController : Loggable, ToolbarNotificationObserver, ToolbarNotificationSource, IDisposable {

        #region Public Fields

        public bool failed;

        #endregion Public Fields

        #region Private Fields

        private const string extPath = "./Extensions/";

        private Antumbra.Glow.View.ToolbarIcon toolbarIcon;

        #endregion Private Fields

        #region Public Constructors

        public ToolbarIconController() {
            AttachObserver(LoggerHelper.GetInstance());
            toolbarIcon = new Antumbra.Glow.View.ToolbarIcon();
            toolbarIcon.Hide();
            AttachObserver(toolbarIcon);
            try {
                MainWindowController mainController = new MainWindowController(toolbarIcon.ProductVersion, new EventHandler(Quit));
                mainController.AttachObserver((ToolbarNotificationObserver)this);

                toolbarIcon.notifyIcon_MouseClickEvent += new EventHandler(mainController.showWindowEventHandler);
                toolbarIcon.notifyIcon_DoubleClickEvent += new EventHandler(mainController.restartEventHandler);
                LogMsg("ToolbarIconController", "Wintumbra starting @ " + DateTime.Now.ToString());
                NewToolbarNotifAvail(1000, "Click to Open", "Click the Antumbra logo to open the main " +
                    "application window.", 0);
                failed = false;
            } catch(Exception ex) {
                NewToolbarNotifAvailEvent(3000, "Exception while starting.", ex.Message, 2);
                Thread.Sleep(3500);
                failed = true;
            }
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewLogMsgAvail(string source, string msg);

        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);

        #endregion Public Delegates

        #region Public Events

        public event NewLogMsgAvail NewLogMsgAvailEvent;

        public event NewToolbarNotif NewToolbarNotifAvailEvent;

        #endregion Public Events

        #region Public Methods

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer) {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void Dispose() {
            toolbarIcon.Dispose();
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon) {
            if(NewToolbarNotifAvailEvent != null) {
                NewToolbarNotifAvailEvent(time, title, msg, icon);
                LogMsg(title, msg);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void LogMsg(string source, string msg) {
            if(NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent(source, msg);
            }
        }

        private void Quit(object sender, EventArgs args) {
            Dispose();
            System.Windows.Forms.Application.Exit();
        }

        #endregion Private Methods
    }
}
