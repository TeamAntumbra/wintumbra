using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Observer.ToolbarNotifications;
using System.Reflection;
using Microsoft.Win32;

namespace Antumbra.Glow
{
    public partial class ToolbarIcon : Form, ToolbarNotificationObserver
    {
        public event EventHandler notifyIcon_MouseClickEvent;
        /// <summary>
        /// ToolbarIcon Constructor
        /// </summary>
        public ToolbarIcon()
        {
            InitializeComponent();
        }
        public void NewToolbarNotifAvail(int time, String title, String msg, int icon)
        {
            ToolTipIcon notifIcon = ToolTipIcon.None;//default
            switch (icon) {
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
        /// <summary>
        /// Event handler for when the menubar icon is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                if (notifyIcon_MouseClickEvent != null)
                    notifyIcon_MouseClickEvent(sender, e);
        }

        /// <summary>
        /// Show the passed message as a balloon of the applications NotifyIcon
        /// </summary>
        /// <param name="time"></param>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="icon"></param>
        private void ShowMessage(int time, string title, string msg, ToolTipIcon icon)
        {
            //    this.logger.Log("Message shown to user in bubble. Message following.\n" + msg);
            this.notifyIcon.ShowBalloonTip(time, title, msg, icon);
        }
    }
}
