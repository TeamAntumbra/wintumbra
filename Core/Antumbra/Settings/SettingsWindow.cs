using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow;
using Antumbra.Glow.ExtensionFramework;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Settings
{
    public partial class SettingsWindow : Form
    {
        private Color[] PollingWindowColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Pink, Color.Purple, Color.Orange, Color.White };
        private AntumbraCore antumbra;
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
        public SettingsWindow(GlowDevice device, ExtensionLibrary library, AntumbraCore core)
        {
            this.antumbra = core;
            this.library = library;
            this.currentDevice = device;
            InitializeComponent();
            //updateValues();
            this.Focus();
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
            compoundDecorationCheck.Checked = this.currentDevice.settings.compoundDecoration;
            newColorWeight.Text = (this.currentDevice.settings.newColorWeight * 100).ToString();
            weightingEnabled.Checked = this.currentDevice.settings.weightingEnabled;
            sleepSize.Text = this.currentDevice.settings.stepSleep.ToString();
            pollingHeight.Text = this.currentDevice.settings.height.ToString();
            pollingWidth.Text = this.currentDevice.settings.width.ToString();
            pollingX.Text = this.currentDevice.settings.x.ToString();
            pollingY.Text = this.currentDevice.settings.y.ToString();
            if (this.library.ready) {
                foreach (var dvr in this.library.AvailDrivers)
                    if (!driverExtensions.Items.Contains(dvr))
                        driverExtensions.Items.Add(dvr);
                foreach (var gbbr in this.library.AvailGrabbers)
                    if (!screenGrabbers.Items.Contains(gbbr))
                        screenGrabbers.Items.Add(gbbr);
                foreach (var pcsr in this.library.AvailProcessors)
                    if (!screenProcessors.Items.Contains(pcsr))
                        screenProcessors.Items.Add(pcsr);
                foreach (var dctr in this.library.AvailDecorators)
                    if (!decorators.Items.Contains(dctr))
                        decorators.Items.Add(dctr);
                foreach (var notf in this.library.AvailNotifiers)
                    if (!notifiers.Items.Contains(notf))
                        notifiers.Items.Add(notf);
            }
            if (this.currentDevice.ActiveDriver != null) {
                if (this.currentDevice.ActiveDriver is GlowScreenDriverCoupler)
                    for (int i = 0; i < driverExtensions.Items.Count; i += 1) {
                        if (driverExtensions.Items[i] is GlowScreenDriverCoupler) {
                            driverExtensions.SelectedIndex = i;
                            break;
                        }
                    }
                else
                    driverExtensions.SelectedIndex = GetIndexOfExtInComboBox(this.currentDevice.ActiveDriver, driverExtensions);
            }
            if (this.currentDevice.ActiveGrabber != null)
                screenGrabbers.SelectedIndex = GetIndexOfExtInComboBox(this.currentDevice.ActiveGrabber, screenGrabbers);
            if (this.currentDevice.ActiveProcessor != null)
                screenProcessors.SelectedIndex = GetIndexOfExtInComboBox(this.currentDevice.ActiveProcessor, screenProcessors);
            if (decorators.SelectedItem == null && decorators.Items.Count > 0)//no item selected & there are items
                decorators.SelectedIndex = 0;
            if (notifiers.SelectedItem == null && notifiers.Items.Count > 0)//no item selected & there are items
                notifiers.SelectedIndex = 0;
            glowStatus.Text = GetStatusString(this.currentDevice.status);
            deviceName.Text = this.currentDevice.id.ToString();
            currentSetup.Text = this.currentDevice.GetSetupDesc();
        }

        private int GetIndexOfExtInComboBox(GlowExtension ext, ComboBox combo)
        {
            var items = combo.Items;
            for (var i = 0; i < items.Count; i += 1) {
                if (((GlowExtension)items[i]).id == ext.id)
                    return i;
            }
            return -1;//not found
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

        private void apply_Click(object sender, EventArgs e)
        {
            this.antumbra.StopCurrent();
            this.currentDevice.ActiveDriver = (GlowDriver)this.driverExtensions.SelectedItem;
            this.currentDevice.ActiveGrabber = (GlowScreenGrabber)this.screenGrabbers.SelectedItem;
            this.currentDevice.ActiveProcessor = (GlowScreenProcessor)this.screenProcessors.SelectedItem;
            //decorators and notifiers are handled through their toggle button and active list in the ExtensionManager
            this.antumbra.AnnounceConfig();
        }

        private void decoratorToggle_Click(object sender, EventArgs e)
        {
            if (null != decorators.SelectedItem) {
                this.antumbra.StopCurrent();
                GlowDecorator value = (GlowDecorator)decorators.SelectedItem;
                if (this.currentDevice.ActiveDecorators.Contains(value)) {
                    this.currentDevice.ActiveDecorators.Remove(value);
                    this.antumbra.ShowMessage(3000, "Decorator Disabled",
                        "The decorator, " + value.ToString() + ", has been disabled.", ToolTipIcon.Info);
                }
                else {
                    this.currentDevice.ActiveDecorators.Add(value);
                    this.antumbra.ShowMessage(3000, "Decorator Enabled",
                        "The decorator, " + value.ToString() + ", has been enabled.", ToolTipIcon.Info);
                }
            }
        }

        private void notifierToggle_Click(object sender, EventArgs e)
        {
            if (null != notifiers.SelectedItem) {
                this.antumbra.StopCurrent();
                GlowNotifier notf = (GlowNotifier)notifiers.SelectedItem;
                if (this.currentDevice.ActiveNotifiers.Contains(notf)) {
                    this.currentDevice.ActiveNotifiers.Remove(notf);
                    this.antumbra.ShowMessage(3000, "Notifier Disabled",
                        "The notifier, " + notf.ToString() + ", has been disabled.", ToolTipIcon.Info);
                }
                else {
                    this.currentDevice.ActiveNotifiers.Add(notf);
                    this.antumbra.ShowMessage(3000, "Notifier Enabled",
                        "The notifier, " + notf.ToString() + ", has been enabled.", ToolTipIcon.Info);
                }
            }
        }

        private void SettingsWindow_MouseEnter(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null || !this.pollingAreaWindow.Visible)
                this.Focus();
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null || this.pollingAreaWindow.IsDisposed) {
                var current = this.currentDevice.id;
                var back = PollingWindowColors[current % 8];
                this.pollingAreaWindow = new pollingAreaSetter(this.currentDevice.settings, back);
                this.antumbra.Stop(current);
                this.antumbra.SendColor(current, back);
                this.pollingAreaWindow.FormClosing += new FormClosingEventHandler(UpdateSelectionsEvent);
            }
            this.pollingAreaWindow.Show();
        }

        private void UpdateSelectionsEvent(object sender, EventArgs args)
        {
            this.updateValues();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Start(this.currentDevice.id);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Stop(this.currentDevice.id);
        }

        private void offBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Off();
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

        private void grabberRecBtn_Click(object sender, EventArgs e)
        {
            var grabber = this.currentDevice.ActiveGrabber;
            grabber.RecmmndCoreSettings();
            this.currentDevice.settings.x = grabber.x;
            this.currentDevice.settings.y = grabber.y;
            this.currentDevice.settings.height = grabber.height;
            this.currentDevice.settings.width = grabber.width;
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
                this.antumbra.ShowMessage(3000, "No Selected Extension",
                    "There is no extension to open the settings of.",
                    ToolTipIcon.Warning);
                return;
            }
            ext.Settings();
        }

        private void driverSettingsBtn_Click(object sender, EventArgs e)
        {
            AttemptToOpenSettingsWindow((GlowDriver)this.driverExtensions.SelectedItem);
        }

        private void grabberSettingsBtn_Click(object sender, EventArgs e)
        {
            AttemptToOpenSettingsWindow((GlowScreenGrabber)this.screenGrabbers.SelectedItem);
        }

        private void processorSettingsBtn_Click(object sender, EventArgs e)
        {
            AttemptToOpenSettingsWindow((GlowScreenProcessor)this.screenProcessors.SelectedItem);
        }

        private void currentDecSettingsBtn_Click(object sender, EventArgs e)
        {
            AttemptToOpenSettingsWindow((GlowDecorator)this.decorators.SelectedItem);
        }

        private void currentNotfSettingsBtn_Click(object sender, EventArgs e)
        {
            AttemptToOpenSettingsWindow((GlowNotifier)this.notifiers.SelectedItem);
        }
    }
}
