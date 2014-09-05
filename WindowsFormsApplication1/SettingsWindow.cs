using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antumbra
{
    public partial class SettingsWindow : Form
    {
        Antumbra antumbra;
        public SettingsWindow(Antumbra antumbra)
        {
            this.antumbra = antumbra;
            InitializeComponent();
            updateValues();
        }

        public void updateValues()
        {
            pollingX.Text = this.antumbra.getPollingWidth().ToString();
            pollingY.Text = this.antumbra.getPollingHeight().ToString();
            HSVstepSize.Text = this.antumbra.HSVstepSize.ToString();
            screenPollingWait.Text = this.antumbra.screenPollingWait.ToString();
            colorFadeStepSize.Text = this.antumbra.colorFadeStepSize.ToString();
            ManualStepSize.Text = this.antumbra.manualStepSize.ToString();
            HSVsleepSize.Text = this.antumbra.HSVstepSleep.ToString();
            ColorFadeSleepSize.Text = this.antumbra.colorFadeStepSleep.ToString();
            ManualSleepSize.Text = this.antumbra.manualStepSleep.ToString();
            screenStepSleep.Text = this.antumbra.screenAvgStepSleep.ToString();
            screenStepSize.Text = this.antumbra.screenAvgStepSize.ToString();
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

        private void screenPollingFq_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.screenPollingWait = (Convert.ToInt32(screenPollingWait.Text));
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception from settings");
            }
        }

        private void HSVstepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.HSVstepSize = (Convert.ToInt32(HSVstepSize.Text));
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception from settings");
            }
        }

        private void colorFadeStepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.colorFadeStepSize = (Convert.ToInt32(colorFadeStepSize.Text));
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception from settings");
            }
        }

        private void ManualStepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.manualStepSize = (Convert.ToInt32(ManualStepSize.Text));
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception from settings");
            }
        }

        private void fullBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.updatePollingBoundsToFull();
            pollingX.Text = this.antumbra.getPollingWidth().ToString();
            pollingY.Text = this.antumbra.getPollingHeight().ToString();
        }

        private void HSVsleepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.HSVstepSleep = Convert.ToInt32(HSVsleepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void ColorFadeSleepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.colorFadeStepSleep = Convert.ToInt32(ColorFadeSleepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void ManualSleepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.manualStepSleep = Convert.ToInt32(ManualSleepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");//todo make these specific
            }
        }

        private void screenStepSleep_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.screenAvgStepSleep = Convert.ToInt32(screenStepSleep.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");//todo make these specific
            }
        }

        private void screenStepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.antumbra.screenAvgStepSize = Convert.ToInt32(screenStepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");//todo make these specific
            }
        }
    }
}
