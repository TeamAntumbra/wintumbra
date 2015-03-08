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
using Antumbra.Glow.Observer.Extensions;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.ExtensionFramework.Management;
using FlatTabControl;

namespace Antumbra.Glow.Settings
{
    public partial class SettingsWindow : Form, ToolbarNotificationSource, GlowCommandSender, GlowExtCollectionObserver//TODO decouple out GlowDevice and ExtensionLibrary with observer pattern
    {
        public delegate void NewToolbarNotifAvail(int time, String title, String msg, int icon);
        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommandAvail(GlowCommand command);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private Color[] PollingWindowColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Pink, Color.Purple, Color.Orange, Color.White };
        private String antumbraVersion;
        private int devId;
        private BasicExtSettingsWinFactory settingsFactory;
        /// <summary>
        /// GlowDevice object for the device whose settings are being rendered currently
        /// </summary>
        public GlowDevice currentDevice { get; private set; }
        /// <summary>
        /// Form used to set the screen grabber polling area
        /// </summary>
        private Form pollingAreaWindow;
        /// <summary>
        /// Move form dependencies
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public SettingsWindow(GlowDevice device, String version, BasicExtSettingsWinFactory factory)//TODO move to views folder
        {
            this.antumbraVersion = version;
            this.currentDevice = device;
            this.settingsFactory = factory;
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

        public void LibraryUpdate(List<GlowExtension> exts)
        {
            List<GlowExtension> current = new List<GlowExtension>();
            foreach (GlowExtension ext in this.driverComboBox.Items)
                current.Add(ext);
            foreach (GlowExtension ext in this.grabberComboBx.Items)
                current.Add(ext);
            foreach (GlowExtension ext in this.processorComboBx.Items)
                current.Add(ext);
            foreach (GlowExtension ext in this.decoratorComboBx.Items)
                current.Add(ext);
            foreach (GlowExtension ext in exts) {
                if (CheckForExtInList(ext, current))//already have it
                    continue;
                //else add it
                if (ext is GlowDriver) {
                    this.driverComboBox.Items.Add(ext);
                    if (this.driverComboBox.SelectedItem == null)
                        this.driverComboBox.SelectedIndex = 0;
                }
                else if (ext is GlowScreenGrabber) {
                    this.grabberComboBx.Items.Add(ext);
                    if (this.grabberComboBx.SelectedItem == null)
                        this.grabberComboBx.SelectedIndex = 0;
                }
                else if (ext is GlowScreenProcessor) {
                    this.processorComboBx.Items.Add(ext);
                    if (this.processorComboBx.SelectedItem == null)
                        this.processorComboBx.SelectedIndex = 0;
                }
                else if (ext is GlowDecorator) {
                    this.decoratorComboBx.Items.Add(ext);
                    if (this.decoratorComboBx.SelectedItem == null)
                        this.decoratorComboBx.SelectedIndex = 0;
                }
                else if (ext is GlowNotifier) {
                    //TODO
                }
            }
        }

        private bool CheckForExtInList(GlowExtension ext, List<GlowExtension> list)
        {
            foreach (GlowExtension item in list)
                if (item.id.Equals(ext.id))
                    return true;
            return false;
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

        private void AttemptToOpenSettingsWindow(Guid id)
        {
            try {
                if (!this.currentDevice.GetExtSettingsWin(id)) {
                    var win = this.settingsFactory.GenerateWindow(id);
                    win.Show();
                }
            }
            catch (Exception) {
                NewToolbarNotifAvailEvent(3000, "No Selected Extension",
                    "There is no extension to open the settings of.", 1);
            }
        }

        private void driverComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            GlowDriver ext = (GlowDriver)box.SelectedItem;
            UpdateDriverChoice(ext);
        }

        private void UpdateDriverChoice(GlowDriver ext)
        {
            if (ext == null)
                return;
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(new StopCommand(this.devId));
            this.currentDevice.SetDvrGbbrOrPrcsrExt(ext.id);
            bool screenBased = (ext is GlowScreenDriverCoupler);
            this.grabberSettingsBtn.Enabled = screenBased;
            this.grabberComboBx.Enabled = screenBased;
            this.processorSettingsBtn.Enabled = screenBased;
            this.processorComboBx.Enabled = screenBased;
        }

        private GlowExtension GetExtFromSender(object sender)
        {
            ComboBox box = (ComboBox)sender;
            return (GlowExtension)box.SelectedItem;
        }

        private void SendStopCommand()
        {
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(new StopCommand(this.devId));
        }

        private void UpdateGrabberProcessorChoice(object sender, EventArgs args)
        {
            GlowExtension ext = GetExtFromSender(sender);
            if (ext == null)
                return;
            SendStopCommand();
            this.currentDevice.SetDvrGbbrOrPrcsrExt(ext.id);
        }

        private bool HandleToggle(ComboBox box)
        {
            GlowExtension ext = (GlowExtension)box.SelectedItem;
            if (ext == null)
                return false;
            SendStopCommand();
            return this.currentDevice.SetDecOrNotf(ext.id);
        }

        private void ToggleDecorator(object sender, System.EventArgs e)
        {
            if (HandleToggle(this.decoratorComboBx))
                currentDecStatus.Text = "True";
            else
                currentDecStatus.Text = "False";
        }

        private void decoratorComboBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlowDecorator dec = (GlowDecorator)this.decoratorComboBx.SelectedItem;
            if (dec != null)
                this.currentDecStatus.Text = this.currentDevice.GetDecOrNotfStatus(dec.id).ToString();
        }

        private void driverSettingsBtn_Click(object sender, EventArgs e)
        {
            GlowExtension ext = this.currentDevice.ActiveDriver;
            if (ext == null)
                return;
            AttemptToOpenSettingsWindow(ext.id);
        }

        private void grabberSettingsBtn_Click(object sender, EventArgs e)
        {
            GlowExtension ext = this.currentDevice.ActiveGrabber;
            if (ext == null)
                return;
            AttemptToOpenSettingsWindow(ext.id);
        }

        private void processorSettingsBtn_Click(object sender, EventArgs e)
        {
            GlowExtension ext = this.currentDevice.ActiveProcessor;
            if (ext == null)
                return;
            AttemptToOpenSettingsWindow(ext.id);
        }

        private void decoratorSettingsBtn_Click(object sender, EventArgs e)
        {
            GlowExtension ext = (GlowExtension)this.decoratorComboBx.SelectedItem;
            if (ext == null)
                return;
            AttemptToOpenSettingsWindow(ext.id);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            this.currentDevice.SaveSettings();
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            SendStopCommand();
            this.currentDevice.LoadSettings();
            updateValues();
        }
    }
}
