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
using Antumbra.Glow.Exceptions;
using FlatTabControl;

namespace Antumbra.Glow.Settings
{
    public partial class SettingsWindow : Form
    {
        //Begin UI events
        public event EventHandler driverRecomBtn_ClickEvent;
        public event EventHandler sleepSize_TextChangedEvent;
        public event EventHandler pollingArea_ClickEvent;
        public event EventHandler startBtn_ClickEvent;
        public event EventHandler stopBtn_ClickEvent;
        public event EventHandler offBtn_ClickEvent;
        public event EventHandler closeBtn_ClickEvent;
        public event MouseEventHandler settingsWindow_MouseDownEvent;
        public event EventHandler newColorWeight_TextChangedEvent;
        public event EventHandler weightingEnabled_CheckedChangedEvent;
        public event FormClosingEventHandler settingsWindow_FormClosingEvent;
        public event EventHandler compoundDecorationCheck_CheckedChangedEvent;
        public event EventHandler driverComboBox_SelectedIndexChangedEvent;
        public event EventHandler loadBtn_ClickEvent;
        public event EventHandler saveBtn_ClickEvent;
        public event EventHandler decoratorSettingsBtn_ClickEvent;
        public event EventHandler processorSettingsBtn_ClickEvent;
        public event EventHandler driverSettingsBtn_ClickEvent;
        public event EventHandler grabberSettingsBtn_ClickEvent;
        public event EventHandler decoratorComboBx_SelectedIndexChangedEvent;
        public event EventHandler updateGrabberProcessorChoiceEvent;
        public event EventHandler toggleDecoratorEvent;
        //end UI events
        private int devId;
        public SettingsWindow(String version)
        {
            InitializeComponent();
            this.versionLabel.Text = version;
            this.Focus();
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (pollingArea_ClickEvent != null)
                pollingArea_ClickEvent(sender, e);
        }

        public void UpdateConfiguration(DeviceSettings settings, String status)//TODO move to controller
        {
            compoundDecorationCheck.Checked = settings.compoundDecoration;
            newColorWeight.Text = (settings.newColorWeight * 100).ToString();
            weightingEnabled.Checked = settings.weightingEnabled;
            sleepSize.Text = settings.stepSleep.ToString();
            pollingHeight.Text = settings.height.ToString();
            pollingWidth.Text = settings.width.ToString();
            pollingX.Text = settings.x.ToString();
            pollingY.Text = settings.y.ToString();
            glowStatus.Text = status;
            deviceName.Text = this.devId.ToString();
        }

        public void UpdatedComboBoxSelectedExt(Guid id, ComboBox box)
        {
            foreach (var obj in box.Items) {
                GlowExtension current = (GlowExtension)obj;
                if (current.id.Equals(id)) {//it's a match
                    box.SelectedItem = current;
                    return;
                }
            }
            throw new ExtensionNotFoundException(id);
        }

        public void RegisterDevice(int id)
        {
            this.devId = id;
        }

        public void UpdateIfDriverScreenBased(bool screenBased)
        {
            this.grabberSettingsBtn.Enabled = screenBased;
            this.grabberComboBx.Enabled = screenBased;
            this.processorSettingsBtn.Enabled = screenBased;
            this.processorComboBx.Enabled = screenBased;
        }

        public void SetCurrentDecStatus(String status)
        {
            this.currentDecStatus.Text = status;
        }

        public void CleanUp()
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// Update stepSleepSize for current device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sleepSize_TextChanged(object sender, EventArgs e)
        {
            if (sleepSize_TextChangedEvent != null)
                sleepSize_TextChangedEvent(sender, e);
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (startBtn_ClickEvent != null)
                startBtn_ClickEvent(sender, e);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (stopBtn_ClickEvent != null)
                stopBtn_ClickEvent(sender, e);
        }

        private void offBtn_Click(object sender, EventArgs e)
        {
            if (this.offBtn_ClickEvent != null)
                offBtn_ClickEvent(sender, e);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (this.closeBtn_ClickEvent != null)
                closeBtn_ClickEvent(sender, e);
        }

        private void SettingsWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (settingsWindow_MouseDownEvent != null)
                settingsWindow_MouseDownEvent(sender, e);
        }

        private void newColorWeight_TextChanged(object sender, EventArgs e)
        {
            if (newColorWeight_TextChangedEvent != null)
                newColorWeight_TextChangedEvent(sender, e);
        }

        private void weightingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (weightingEnabled_CheckedChangedEvent != null)
                weightingEnabled_CheckedChangedEvent(sender, e);
        }

        private void driverRecBtn_Click(object sender, EventArgs e)
        {
            if (driverRecomBtn_ClickEvent != null)
                driverRecomBtn_ClickEvent(sender, e);
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (settingsWindow_FormClosingEvent != null)
                settingsWindow_FormClosingEvent(sender, e);
        }

        private void compoundDecorationCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (compoundDecorationCheck_CheckedChangedEvent != null)
                compoundDecorationCheck_CheckedChangedEvent(sender, e);
        }

        private void driverComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (driverComboBox_SelectedIndexChangedEvent != null)
                driverComboBox_SelectedIndexChangedEvent(sender, e);
        }

        private void UpdateGrabberProcessorChoice(object sender, EventArgs args)
        {
            if (updateGrabberProcessorChoiceEvent != null)
                updateGrabberProcessorChoiceEvent(sender, args);
        }

        private void ToggleDecorator(object sender, EventArgs e)
        {
            if (toggleDecoratorEvent != null)
                toggleDecoratorEvent(this.decoratorComboBx.SelectedItem, e);
        }

        private void decoratorComboBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (decoratorComboBx_SelectedIndexChangedEvent != null)
                decoratorComboBx_SelectedIndexChangedEvent(sender, e);
        }

        private void driverSettingsBtn_Click(object sender, EventArgs e)
        {
            if (driverSettingsBtn_ClickEvent != null)
                driverSettingsBtn_ClickEvent(this.driverComboBox.SelectedItem, e);
        }

        private void grabberSettingsBtn_Click(object sender, EventArgs e)
        {
            if (grabberSettingsBtn_ClickEvent != null)
                grabberSettingsBtn_ClickEvent(this.grabberComboBx.SelectedItem, e);
        }

        private void processorSettingsBtn_Click(object sender, EventArgs e)
        {
            if (processorSettingsBtn_ClickEvent != null)
                processorSettingsBtn_ClickEvent(this.processorComboBx.SelectedItem, e);
        }

        private void decoratorSettingsBtn_Click(object sender, EventArgs e)
        {
            if (decoratorSettingsBtn_ClickEvent != null)
                decoratorSettingsBtn_ClickEvent(this.decoratorComboBx.SelectedItem, e);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (saveBtn_ClickEvent != null)
                saveBtn_ClickEvent(sender, e);
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            if (loadBtn_ClickEvent != null)
                loadBtn_ClickEvent(sender, e);
            //SendStopCommand();
           // this.currentDevice.LoadSettings();
            //updateValues();
        }
    }
}
