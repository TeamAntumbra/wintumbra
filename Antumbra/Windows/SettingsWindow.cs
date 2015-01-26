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

namespace Antumbra.Glow.Windows
{
    public partial class SettingsWindow : MetroFramework.Forms.MetroForm
    {
        private AntumbraCore antumbra;
        private ColorPickerDialog picker;
        public SettingsWindow(AntumbraCore antumbra)
        {
            this.antumbra = antumbra;
            this.picker = new ColorPickerDialog();
            InitializeComponent();
        }

        public void updateValues()
        {
            pollingX.Text = this.antumbra.getPollingWidth().ToString();
            pollingY.Text = this.antumbra.getPollingHeight().ToString();
            stepSize.Text = this.antumbra.stepSize.ToString();
            sleepSize.Text = this.antumbra.stepSleep.ToString();
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailIndependentDrivers()) {
                driverExtensions.Items.Add(str);
            }
            if (driverExtensions.Items.Count > 0)
                driverExtensions.SelectedIndex = 0;
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailScreenGrabbers()) {
                screenGrabbers.Items.Add(str);
            }
            if (screenGrabbers.Items.Count > 0)
                screenGrabbers.SelectedIndex = 0;
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailScreenProcessors()) {
                screenProcessors.Items.Add(str);
            }
            if (screenProcessors.Items.Count > 0)
                screenProcessors.SelectedIndex = 0;
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailDecorators()) {
                decorators.Items.Add(str);
            }
            if (decorators.Items.Count > 0)
                decorators.SelectedIndex = 0;
            foreach (var str in this.antumbra.MEFHelper.GetNamesOfAvailNotifiers()) {
                notifiers.Items.Add(str);
            }
            if (notifiers.Items.Count > 0)
                notifiers.SelectedIndex = 0;
        }

        private void pollingY_TextChanged(object sender, EventArgs e)
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

        private void stepSize_TextChanged(object sender, EventArgs e)
        {
           //TODO
        }

        private void fullBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.updatePollingBoundsToFull();
            pollingX.Text = this.antumbra.getPollingWidth().ToString();
            pollingY.Text = this.antumbra.getPollingHeight().ToString();
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

        private void displayIndex_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)displayIndex.Value;
            if (value > Screen.AllScreens.Length - 1)//component handles min value so lets handle max
                displayIndex.Value = Screen.AllScreens.Length - 1;//max index allowed
            else
                //this.antumbra.screen.display = Screen.AllScreens[(int)displayIndex.Value];
                Console.WriteLine("TODO");
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

        private void driverExtensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.antumbra.setDriver(this.antumbra.MEFHelper.GetDriver(driverExtensions.SelectedText));
        }

        private void screenGrabbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.antumbra.setScreenGrabber((GlowScreenGrabber)screenGrabbers.SelectedItem);
        }

        private void screenProcessors_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.antumbra.setScreenProcessor((GlowScreenProcessor)screenProcessors.SelectedItem);
        }
    }
}
