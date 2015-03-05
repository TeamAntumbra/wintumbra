using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.ExtensionFramework.Types;
using FlatTabControl;

namespace Antumbra.Glow.Settings
{
    public partial class SettingsWindow : Form, ToolbarNotificationSource, GlowCommandSender//TODO decouple out GlowDevice and ExtensionLibrary with observer pattern
    {
        public delegate void NewToolbarNotifAvail(int time, String title, String msg, int icon);
        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommandAvail(GlowCommand command);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private Color[] PollingWindowColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Pink, Color.Purple, Color.Orange, Color.White };
        private String antumbraVersion;
        private int devId;
        /// <summary>
        /// GlowDevice object for the device whose settings are being rendered currently
        /// </summary>
        public GlowDevice currentDevice { get; private set; }
        /// <summary>
        /// Form used to set the screen grabber polling area
        /// </summary>
        private Form pollingAreaWindow;
        private ExtensionLibrary library;
        /// <summary>
        /// Move form dependencies
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public SettingsWindow(GlowDevice device, ExtensionLibrary library, String version)//TODO move to views folder
        {
            this.antumbraVersion = version;
            this.library = library;
            this.currentDevice = device;
            InitializeComponent();
            this.Focus();
        }

        public void AttachToolbarNotifObserver(ToolbarNotificationObserver observer)
        {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachGlowCommandObserver(GlowCommandObserver observer)
        {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public void RegisterDevice(int id)
        {
            this.devId = id;
        }

        public void CleanUp()
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// Update the settings window form to reflect the settings found in the GlowDevice settings object
        /// </summary>
        public void updateValues()
        {
            this.versionLabel.Text = this.antumbraVersion;
            compoundDecorationCheck.Checked = this.currentDevice.settings.compoundDecoration;
            newColorWeight.Text = (this.currentDevice.settings.newColorWeight * 100).ToString();
            weightingEnabled.Checked = this.currentDevice.settings.weightingEnabled;
            sleepSize.Text = this.currentDevice.settings.stepSleep.ToString();
            pollingHeight.Text = this.currentDevice.settings.height.ToString();
            pollingWidth.Text = this.currentDevice.settings.width.ToString();
            pollingX.Text = this.currentDevice.settings.x.ToString();
            pollingY.Text = this.currentDevice.settings.y.ToString();
            glowStatus.Text = GetStatusString(this.currentDevice.status);
            deviceName.Text = this.devId.ToString();
            //currentSetup.Text = this.currentDevice.GetSetupDesc();
        }

        /// <summary>
        /// Return the string representation of the given status value
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetStatusString(int status)
        {
            switch (status) {
                case 0:
                    return "Sending/Recieving Successfully";
                case 1:
                    return "Glow Device Disconnected";
                case 2:
                    return "LibAntumbra Memory Allocation Failed";
                case 3:
                    return "LibUSB Exception";
                case 4:
                    return "Device in Invalid State for Operation";
                case 5:
                    return "Index or Size Out of Range";
                case 6:
                    return "Protocol Command Not Supported";
                case 7:
                    return "Protocol Command Failure";
                case 8:
                    return "Unspecified Protocol Error";
                default:
                    return "Invalid Status";
            }
        }
        /// <summary>
        /// Update stepSleepSize for current device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sleepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.currentDevice.settings.stepSleep = Convert.ToInt32(sleepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null || this.pollingAreaWindow.IsDisposed) {
                var current = this.devId;
                var back = PollingWindowColors[current % 8];
                this.pollingAreaWindow = new pollingAreaSetter(this.currentDevice.settings, back);
                NewGlowCommandAvailEvent(new StopCommand(current));
                NewGlowCommandAvailEvent(new SendColorCommand(current, back));
                this.pollingAreaWindow.FormClosing += new FormClosingEventHandler(UpdatePollingSelectionsEvent);
            }
            this.pollingAreaWindow.Show();
        }

        private void UpdatePollingSelectionsEvent(object sender, EventArgs args)
        {
            this.updateValues();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            NewGlowCommandAvailEvent(new StartCommand(this.devId));
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            NewGlowCommandAvailEvent(new StopCommand(this.devId));
        }

        private void offBtn_Click(object sender, EventArgs e)
        {
            NewGlowCommandAvailEvent(new PowerOffCommand(-1));
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsWindow_MouseDown(object sender, MouseEventArgs e)
        {
            // Drag form to move
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void newColorWeight_TextChanged(object sender, EventArgs e)
        {
            try {
                int percent = Convert.ToInt16(newColorWeight.Text);
                if (percent >= 0 && percent <= 100)//valid
                    this.currentDevice.settings.newColorWeight = Convert.ToDouble(percent / 100.0);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void weightingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.currentDevice.settings.weightingEnabled = weightingEnabled.Checked;
        }

        private void driverRecBtn_Click(object sender, EventArgs e)
        {
            this.currentDevice.ActiveDriver.RecmmndCoreSettings();
            this.currentDevice.settings.stepSleep = this.currentDevice.ActiveDriver.stepSleep;
            updateValues();
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void compoundDecorationCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.currentDevice.settings.compoundDecoration = compoundDecorationCheck.Checked;
        }

        private void AttemptToOpenSettingsWindow(GlowExtension ext)
        {
            if (ext == null) {
                NewToolbarNotifAvailEvent(3000, "No Selected Extension",
                    "There is no extension to open the settings of.", 1);
                return;
            }
            if (!ext.Settings()) {
                var win = new AntumbraExtSettingsWindow(ext);
                win.Show();
            }
                
        }
    }
}
