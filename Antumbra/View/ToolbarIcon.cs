using Antumbra.Glow.Observer.ToolbarNotifications;
using System;
using System.Windows.Forms;

namespace Antumbra.Glow.View {

    public partial class ToolbarIcon : Form, ToolbarNotificationObserver {

        #region Public Constructors

        /// <summary>
        /// ToolbarIcon Constructor
        /// </summary>
        public ToolbarIcon() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler notifyIcon_DoubleClickEvent;

        public event EventHandler notifyIcon_MouseClickEvent;

        #endregion Public Events

        #region Public Methods

        public void NewToolbarNotifAvail(int time, String title, String msg, int icon) {
            ToolTipIcon notifIcon = ToolTipIcon.None;//default
            switch(icon) {
                case 0:
                    notifIcon = ToolTipIcon.Info;
                    break;

                case 1:
                    notifIcon = ToolTipIcon.Warning;
                    break;

                case 2:
                    notifIcon = ToolTipIcon.Error;
                    break;
            }
            this.ShowMessage(time, title, msg, notifIcon);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Event handler for when the menubar icon is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
            if(notifyIcon_MouseClickEvent != null)
                notifyIcon_MouseClickEvent(sender, e);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(notifyIcon_DoubleClickEvent != null)
                notifyIcon_DoubleClickEvent(sender, e);
        }

        /// <summary>
        /// Show the passed message as a balloon of the applications NotifyIcon
        /// </summary>
        /// <param name="time"></param>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="icon"></param>
        private void ShowMessage(int time, string title, string msg, ToolTipIcon icon) {
            if(!msg.Trim().Equals("")) {
                this.notifyIcon.ShowBalloonTip(time, title, msg, icon);
            }
        }

        #endregion Private Methods
    }
}
