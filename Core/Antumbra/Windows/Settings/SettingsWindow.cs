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

namespace Antumbra.Glow.Windows
{
    public partial class SettingsWindow : Form//TODO split this into window event handlers and another settings / setup class
    {
        /// <summary>
        /// AntumbraCore object that created this form
        /// </summary>
        private AntumbraCore antumbra;
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
        public SettingsWindow(AntumbraCore antumbra)
        {
            this.antumbra = antumbra;
            InitializeComponent();
            updateValues();
            this.Focus();
        }

        public void updateValues()
        {
            newColorWeight.Text = (this.antumbra.newColorWeight * 100).ToString();
            weightingEnabled.Checked = this.antumbra.weightingEnabled;
            stepSize.Text = this.antumbra.stepSize.ToString();
            sleepSize.Text = this.antumbra.stepSleep.ToString();
            pollingHeight.Text = this.antumbra.pollingHeight.ToString();
            pollingWidth.Text = this.antumbra.pollingWidth.ToString();
            pollingX.Text = this.antumbra.pollingX.ToString();
            pollingY.Text = this.antumbra.pollingY.ToString();
            foreach (var dvr in this.antumbra.ExtensionManager.AvailDrivers)
                if (!driverExtensions.Items.Contains(dvr))
                    driverExtensions.Items.Add(dvr);
            if (this.antumbra.ExtensionManager.ActiveDriver is GlowScreenDriverCoupler)
                for (int i = 0; i < driverExtensions.Items.Count; i += 1) {
                    if (driverExtensions.Items[i] is GlowScreenDriverCoupler) {
                        driverExtensions.SelectedIndex = i;
                        break;
                    }
                }
            else
                driverExtensions.SelectedIndex = driverExtensions.Items.IndexOf(this.antumbra.ExtensionManager.ActiveDriver);
            foreach (var gbbr in this.antumbra.ExtensionManager.AvailScreenGrabbers)
                if (!screenGrabbers.Items.Contains(gbbr))
                    screenGrabbers.Items.Add(gbbr);
            screenGrabbers.SelectedIndex = screenGrabbers.Items.IndexOf(this.antumbra.ExtensionManager.ActiveGrabber);
            foreach (var pcsr in this.antumbra.ExtensionManager.AvailScreenProcessors)
                if (!screenProcessors.Items.Contains(pcsr))
                    screenProcessors.Items.Add(pcsr);
            screenProcessors.SelectedIndex = screenProcessors.Items.IndexOf(this.antumbra.ExtensionManager.ActiveProcessor);
            foreach (var dctr in this.antumbra.ExtensionManager.AvailDecorators)
                if (!decorators.Items.Contains(dctr))
                    decorators.Items.Add(dctr);
            //TODO add some way to differentiate the active decorators and notifiers (and maybe the others too)
            foreach (var notf in this.antumbra.ExtensionManager.AvailNotifiers)
                if (!notifiers.Items.Contains(notf))
                    notifiers.Items.Add(notf);
            //changeSensitivity.Text = this.antumbra.changeThreshold.ToString();
        }

        public void updateSwatch(Color newColor)
        {
            if (this.Visible)
                this.colorSwatch.BackColor = newColor;
        }

        private void stepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.stepSize = Convert.ToInt32(stepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void sleepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.stepSleep = Convert.ToInt32(sleepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void apply_Click(object sender, EventArgs e)
        {
            this.antumbra.Stop();
            this.antumbra.ExtensionManager.ActiveDriver = (GlowDriver)this.driverExtensions.SelectedItem;
            this.antumbra.ExtensionManager.ActiveGrabber = (GlowScreenGrabber)this.screenGrabbers.SelectedItem;
            this.antumbra.ExtensionManager.ActiveProcessor = (GlowScreenProcessor)this.screenProcessors.SelectedItem;
            //decorators and notifiers are handled through their toggle button and active list in the ExtensionManager
            this.antumbra.AnnounceConfig();
        }

        private void decoratorToggle_Click(object sender, EventArgs e)
        {
            if (null != decorators.SelectedItem) {
                this.antumbra.Stop();
                GlowDecorator value = (GlowDecorator)decorators.SelectedItem;
                if (this.antumbra.ExtensionManager.ActiveDecorators.Contains(value)) {
                    this.antumbra.ExtensionManager.ActiveDecorators.Remove(value);
                    this.antumbra.ShowMessage(3000, "Decorator Disabled",
                        "The decorator, " + value.ToString() + ", has been disabled.", ToolTipIcon.Info);
                }
                else {
                    this.antumbra.ExtensionManager.ActiveDecorators.Add(value);
                    this.antumbra.ShowMessage(3000, "Decorator Enabled",
                        "The decorator, " + value.ToString() + ", has been enabled.", ToolTipIcon.Info);
                }
            }
        }

        private void notifierToggle_Click(object sender, EventArgs e)
        {
            if (null != notifiers.SelectedItem) {
                this.antumbra.Stop();
                GlowNotifier notf = (GlowNotifier)notifiers.SelectedItem;
                if (this.antumbra.ExtensionManager.ActiveNotifiers.Contains(notf)) {
                    this.antumbra.ExtensionManager.ActiveNotifiers.Remove(notf);
                    this.antumbra.ShowMessage(3000, "Notifier Disabled",
                        "The notifier, " + notf.ToString() + ", has been disabled.", ToolTipIcon.Info);
                }
                else {
                    this.antumbra.ExtensionManager.ActiveNotifiers.Add(notf);
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
            if (this.pollingAreaWindow == null || this.pollingAreaWindow.IsDisposed)
                this.pollingAreaWindow = new pollingAreaSetter(this.antumbra, this);
            this.pollingAreaWindow.Show();
        }

        /*private void changeSensitivity_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (Int32.TryParse(changeSensitivity.Text.ToString(), out value)) {
                this.antumbra.changeThreshold = value;
            }
            else
                Console.WriteLine("Input value, '" + changeSensitivity.Text + "' is not parsable to an int.");
        }*/

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Start();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Stop();
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
                    this.antumbra.newColorWeight = Convert.ToDouble(percent / 100.0);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void weightingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.antumbra.weightingEnabled = weightingEnabled.Checked;
        }
    }
}
