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

namespace Antumbra.Glow.Windows
{
    public partial class SettingsWindow : MetroFramework.Forms.MetroForm
    {
        private AntumbraCore antumbra;
        private ColorPickerDialog picker;
        private List<string> enabledDecorators, enabledNotifiers;
        private MetroFramework.Forms.MetroForm pollingAreaWindow;
        public SettingsWindow(AntumbraCore antumbra)
        {
            this.antumbra = antumbra;
            this.picker = new ColorPickerDialog();
            this.enabledDecorators = new List<string>();
            this.enabledNotifiers = new List<string>();
            InitializeComponent();
            updateValues();
            this.Focus();
        }

        public void updateValues()
        {
            stepSize.Text = this.antumbra.stepSize.ToString();
            sleepSize.Text = this.antumbra.stepSleep.ToString();
            pollingHeight.Text = this.antumbra.pollingHeight.ToString();
            pollingWidth.Text = this.antumbra.pollingWidth.ToString();
            pollingX.Text = this.antumbra.pollingX.ToString();
            pollingY.Text = this.antumbra.pollingY.ToString();
            driverExtensions.Items.Clear();
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailDrivers()) {
                driverExtensions.Items.Add(str);
            }
            if (driverExtensions.Items.Count > 0)
                if (null != this.antumbra.getCurrentDriverName())
               //     driverExtensions.SelectedIndex = 0;
               // else
                    driverExtensions.SelectedIndex = driverExtensions.Items.IndexOf(this.antumbra.getCurrentDriverName());
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailScreenGrabbers()) {
                if (!screenGrabbers.Items.Contains(str))
                    screenGrabbers.Items.Add(str);
            }
            if (screenGrabbers.Items.Count > 0)
                if (null != this.antumbra.getCurrentScreenGrabberName())
          //          screenGrabbers.SelectedIndex = 0;
           //     else
                    screenGrabbers.SelectedIndex = screenGrabbers.Items.IndexOf(this.antumbra.getCurrentScreenGrabberName());
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailScreenProcessors()) {
                if (!screenProcessors.Items.Contains(str))
                    screenProcessors.Items.Add(str);
            }
            if (screenProcessors.Items.Count > 0)
                if (null != this.antumbra.getCurrentScreenProcessorName())
           //         screenProcessors.SelectedIndex = 0;
           //     else
                    screenProcessors.SelectedIndex = screenProcessors.Items.IndexOf(this.antumbra.getCurrentScreenProcessorName());
            changeSensitivity.Text = this.antumbra.changeThreshold.ToString();
            updateDecorators();
            updateNotifiers();
            this.antumbra.checkStatus();
        }

        /*private void pollingY_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.updatePollingBounds(Convert.ToInt32(pollingX.Text), Convert.ToInt32(pollingY.Text));
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception from settings");
            }
        }

        private void pollingX_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.updatePollingBounds(Convert.ToInt32(pollingX.Text), Convert.ToInt32(pollingY.Text));
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception from settings");
            }
        }
        */
        private void stepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.stepSize = Convert.ToInt32(stepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        /*private void fullBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.updatePollingBoundsToFull();
            pollingX.Text = this.antumbra.getPollingWidth().ToString();
            pollingY.Text = this.antumbra.getPollingHeight().ToString();
        }*/

        private void sleepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.stepSleep = Convert.ToInt32(sleepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void displayIndex_ValueChanged(object sender, EventArgs e)
        {
            /*int value = (int)displayIndex.Value;
            if (value > Screen.AllScreens.Length - 1)//component handles min value so lets handle max
                displayIndex.Value = Screen.AllScreens.Length - 1;//max index allowed
            else
                //this.antumbra.screen.display = Screen.AllScreens[(int)displayIndex.Value];
                Console.WriteLine("TODO");*/
        }

        private void manualColorBtn_Click(object sender, EventArgs e)
        {
            this.picker = new ColorPickerDialog();
            this.picker.Show();
            picker.previewPanel.BackColorChanged += new EventHandler(manualListener);
        }

        private void manualListener(object sender, EventArgs e)
        {
            this.antumbra.SetColorTo(this.picker.previewPanel.BackColor);
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void UpdateSelections()
        {
            if (null == driverExtensions.SelectedItem)
                this.antumbra.setDriver(null);
            else
                this.antumbra.setDriver(this.antumbra.MEFHelper.GetDriver(driverExtensions.SelectedItem.ToString()));
            if (null == screenGrabbers.SelectedItem)
                this.antumbra.setScreenGrabber(null);
            else
                this.antumbra.setScreenGrabber(this.antumbra.MEFHelper.GetScreenGrabber(screenGrabbers.SelectedItem.ToString()));
            if (null == screenProcessors.SelectedItem)
                this.antumbra.setScreenProcessor(null);
            else
                this.antumbra.setScreenProcessor(this.antumbra.MEFHelper.GetScreenProcessor(screenProcessors.SelectedItem.ToString()));
            this.antumbra.setDecorators(this.antumbra.MEFHelper.GetDecorators(enabledDecorators));
            this.antumbra.setNotifiers(this.antumbra.MEFHelper.GetNotifiers(enabledNotifiers));
        }

        private void apply_Click(object sender, EventArgs e)
        {
            this.antumbra.Stop();
            UpdateSelections();
        }

        private void updateDecorators()
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
        }

        private void decoratorToggle_Click(object sender, EventArgs e)
        {
            if (null != decorators.SelectedItem) {
                var value = decorators.SelectedItem.ToString();
                if (value.EndsWith(" - Active")) {
                    value = value.Substring(0, value.Length - 9);
                    enabledDecorators.Remove(value);
                }
                else
                    enabledDecorators.Add(value);
            }
            updateDecorators();
        }

        private void updateNotifiers()
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
        }

        private void notifierToggle_Click(object sender, EventArgs e)
        {
            if (null != notifiers.SelectedItem) {
                var value = notifiers.SelectedItem.ToString();
                if (value.EndsWith(" - Active")) {
                    value = value.Substring(0, value.Length - 9);
                    enabledDecorators.Remove(value);
                }
                else
                    enabledNotifiers.Add(notifiers.SelectedItem.ToString());
            }
            updateNotifiers();
        }

        private void SettingsWindow_MouseEnter(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null || !this.pollingAreaWindow.Visible)
                this.Focus();
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null)
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
    }
}
