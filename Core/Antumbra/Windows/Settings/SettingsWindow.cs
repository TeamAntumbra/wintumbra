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
            stepSize.Text = this.antumbra.stepSize.ToString();
            sleepSize.Text = this.antumbra.stepSleep.ToString();
            maxFadeSteps.Text = this.antumbra.fadeSteps.ToString();
            fadeEnabledCheck.Checked = this.antumbra.fadeEnabled;
            pollingHeight.Text = this.antumbra.pollingHeight.ToString();
            pollingWidth.Text = this.antumbra.pollingWidth.ToString();
            pollingX.Text = this.antumbra.pollingX.ToString();
            pollingY.Text = this.antumbra.pollingY.ToString();
            foreach (var dvr in this.antumbra.ExtensionManager.AvailDrivers)
                if (!driverExtensions.Items.Contains(dvr))
                    driverExtensions.Items.Add(dvr);
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
            changeSensitivity.Text = this.antumbra.changeThreshold.ToString();
            //updateDecorators();
            //updateNotifiers();
            //this.antumbra.checkStatus();
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

        public void UpdateSelections()
        {
            this.antumbra.ExtensionManager.ActiveDriver = (GlowDriver)this.driverExtensions.SelectedItem;
            /*if (null == screenGrabbers.SelectedItem)
                this.antumbra.setScreenGrabber(null);
            else
                this.antumbra.setScreenGrabber(this.antumbra.MEFHelper.GetScreenGrabber(screenGrabbers.SelectedItem.ToString()));
            if (null == screenProcessors.SelectedItem)
                this.antumbra.setScreenProcessor(null);
            else
                this.antumbra.setScreenProcessor(this.antumbra.MEFHelper.GetScreenProcessor(screenProcessors.SelectedItem.ToString()));
            this.antumbra.setDecorators(this.antumbra.MEFHelper.GetDecorators(enabledDecorators));
            this.antumbra.setNotifiers(this.antumbra.MEFHelper.GetNotifiers(enabledNotifiers));*/
        }

        private void apply_Click(object sender, EventArgs e)
        {
            this.antumbra.Stop();
            UpdateSelections();
        }

        /*private void updateDecorators()
        {
            decorators.Items.Clear();
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailDecorators()) {
                if (!decorators.Items.Contains(str)) {
                    if (enabledDecorators.Contains(str)) {
                        decorators.Items.Add(str + " - Active");
                    }
                    else
                        decorators.Items.Add(str);
                }
            }
            if (decorators.Items.Count > 0)
                decorators.SelectedIndex = 0;
        }*/

        private void decoratorToggle_Click(object sender, EventArgs e)
        {
            if (null != decorators.SelectedItem) {
              /*  var value = decorators.SelectedItem.ToString();
                if (value.EndsWith(" - Active")) {
                    value = value.Substring(0, value.Length - 9);
                    enabledDecorators.Remove(value);
                }
                else
                    enabledDecorators.Add(value);*/
                GlowDecorator value = (GlowDecorator)decorators.SelectedItem;
                if (this.antumbra.ExtensionManager.ActiveDecorators.Contains(value))
                    this.antumbra.ExtensionManager.ActiveDecorators.Remove(value);
                else
                    this.antumbra.ExtensionManager.ActiveDecorators.Add(value);
            }
            //updateDecorators();
        }

       /* private void updateNotifiers()
        {
            notifiers.Items.Clear();
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailNotifiers()) {
                if (!notifiers.Items.Contains(str)) {
                    if (enabledNotifiers.Contains(str)) {
                        notifiers.Items.Add(str + " - Active");
                    }
                    else
                        notifiers.Items.Add(str);
                }
            }
            if (notifiers.Items.Count > 0)
                notifiers.SelectedIndex = 0;
        }*/

        private void notifierToggle_Click(object sender, EventArgs e)
        {
            if (null != notifiers.SelectedItem) {
            /*    var value = notifiers.SelectedItem.ToString();
                if (value.EndsWith(" - Active")) {
                    value = value.Substring(0, value.Length - 9);
                    enabledDecorators.Remove(value);
                }
                else
                    enabledNotifiers.Add(notifiers.SelectedItem.ToString());*/
                GlowNotifier notf = (GlowNotifier)notifiers.SelectedItem;
                if (this.antumbra.ExtensionManager.ActiveNotifiers.Contains(notf))
                    this.antumbra.ExtensionManager.ActiveNotifiers.Remove(notf);
                else
                    this.antumbra.ExtensionManager.ActiveNotifiers.Add(notf);
            }
            //updateNotifiers();
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

        private void changeSensitivity_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (Int32.TryParse(changeSensitivity.Text.ToString(), out value)) {
                this.antumbra.changeThreshold = value;
            }
            else
                Console.WriteLine("Input value, '" + changeSensitivity.Text + "' is not parsable to an int.");
        }

        private void maxFadeSteps_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (Int32.TryParse(maxFadeSteps.Text.ToString(), out value))
                this.antumbra.fadeSteps = value;
            else
                Console.WriteLine("Input value, '" + maxFadeSteps.Text + "' is not parsable to an int.");
        }

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

        private void fadeEnabledCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.antumbra.fadeEnabled = fadeEnabledCheck.Checked;
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
    }
}
