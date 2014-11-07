﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antumbra.Glow.Windows
{
    public partial class SettingsWindow : MetroFramework.Forms.MetroForm
    {
        AntumbraCoreDriver antumbra;
        ColorPickerDialog picker;
        private String selectedExeName;
        public SettingsWindow(AntumbraCoreDriver antumbra)
        {
            this.antumbra = antumbra;
            this.picker = new ColorPickerDialog();
            this.selectedExeName = null;
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
            saturationEnabledCheck.Checked = this.antumbra.screenProcessor.saturationEnabled;
            saturationAdditiveTxt.Text = this.antumbra.screenProcessor.saturationAdditive.ToString();
            sinSleepSize.Text = this.antumbra.sinFadeStepSleep.ToString();
            sinStepSize.Text = this.antumbra.sinFadeStepSize.ToString();
            processMenu.DataSource = this.antumbra.gameScreenGrabber.getProcesses();
            if (this.selectedExeName == null)
                this.selectedExeName = processMenu.SelectedText;
            else
                processMenu.SelectedText = this.selectedExeName;
            gameModeCheck.Checked = this.antumbra.gameMode;
            maxBright.Value = this.antumbra.maxBrightness;
            minBright.Value = this.antumbra.minBrightness;
            warmthTrackBar.Value = this.antumbra.warmth;
        }

        public String getGameModeProcess()
        {
            return this.selectedExeName;
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
            this.antumbra.setColorTo(this.picker.previewPanel.BackColor);
        }

        private void maxBright_Scroll(object sender, ScrollEventArgs e)
        {
            this.antumbra.maxBrightness = maxBright.Value;
        }

        private void minBright_Scroll(object sender, ScrollEventArgs e)
        {
            this.antumbra.minBrightness = minBright.Value;
        }

        private void saturationAdditiveTxt_TextChanged(object sender, EventArgs e)
        {
            try {
                //this.antumbra.screen.saturationAdditive = Convert.ToDouble(saturationAdditiveTxt.Text);
                this.antumbra.screenProcessor.saturationAdditive = Convert.ToDouble(saturationAdditiveTxt.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");//todo make these specific
            }
            
        }

        private void saturationEnabledCheck_CheckedChanged(object sender, EventArgs e)
        {
           // this.antumbra.screen.saturationEnabled = saturationEnabledCheck.Checked;
            this.antumbra.screenProcessor.saturationEnabled = saturationEnabledCheck.Checked;
        }

        private void sinStepSize_TextChanged(object sender, EventArgs e)
        {
            this.antumbra.sinFadeStepSize = Convert.ToDouble(sinStepSize.Text);
        }

        private void sinSleepSize_TextChanged(object sender, EventArgs e)
        {
            this.antumbra.sinFadeStepSleep = Convert.ToInt32(sinSleepSize.Text);
        }

        private void gameModeCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.antumbra.gameMode = gameModeCheck.Checked;
        }

        private void processMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedExeName = processMenu.SelectedText;
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void warmthTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.antumbra.warmth = warmthTrackBar.Value;
        }
    }
}
