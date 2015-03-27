using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antumbra.Glow.View
{
    public partial class MainWindow : Form
    {
        public event EventHandler closeBtn_ClickEvent;
        public event EventHandler colorWheel_ColorChangedEvent;
        public event EventHandler brightnessTrackBar_ScrollEvent;
        public event EventHandler hsvBtn_ClickEvent;
        public event EventHandler sinBtn_ClickEvent;
        public event EventHandler neonBtn_ClickEvent;
        public event EventHandler mirrorBtn_ClickEvent;
        public event EventHandler augmentBtn_ClickEvent;
        public event EventHandler smoothBtn_ClickEvent;
        public event EventHandler gameBtn_ClickEvent;
        public event MouseEventHandler mainWindow_MouseDownEvent;
        public event EventHandler customConfigBtn_ClickEvent;
        public event EventHandler quitBtn_ClickEvent;
        public event EventHandler setPollingBtn_ClickEvent;
        public event EventHandler onOffValueChanged;
        public event EventHandler advancedDevSelectionChanged;
        public event EventHandler whiteBalanceBtn_ClickEvent;

        public MainWindow()
        {
            InitializeComponent();
            this.brightnessTrackBar.Value = this.brightnessTrackBar.Maximum;
            this.versionLabel.Text = "v" + this.ProductVersion.ToString();
        }

        public void AddDeviceId(int id)
        {
            this.devIDList.Items.Add(id);
            if (this.devIDList.SelectedItem == null)
                this.devIDList.SelectedIndex = 0;
        }

        public void SetOnSelection(bool value)
        {
            this.offBtn.Checked = !value;
            this.onBtn.Checked = value;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (closeBtn_ClickEvent != null)
                closeBtn_ClickEvent(sender, e);
        }

        private void colorWheel_ColorChanged(object sender, EventArgs e)
        {
            if (colorWheel_ColorChangedEvent != null)
                colorWheel_ColorChangedEvent(this.colorWheel.HslColor, e);
        }

        private void brightnessTrackBar_Scroll(object sender, EventArgs e)
        {
            if (brightnessTrackBar_ScrollEvent != null) {
                int max = this.brightnessTrackBar.Maximum;
                int current = this.brightnessTrackBar.Value;
                int[] values = { current, max };
                brightnessTrackBar_ScrollEvent(values, e);
            }
        }

        private void hsvBtn_Click(object sender, EventArgs e)
        {
            if (hsvBtn_ClickEvent != null)
                hsvBtn_ClickEvent(sender, e);
        }

        private void sinBtn_Click(object sender, EventArgs e)
        {
            if (sinBtn_ClickEvent != null)
                sinBtn_ClickEvent(sender, e);
        }

        private void neonBtn_Click(object sender, EventArgs e)
        {
            if (neonBtn_ClickEvent != null)
                neonBtn_ClickEvent(sender, e);
        }

        private void mirrorBtn_Click(object sender, EventArgs e)
        {
            if (mirrorBtn_ClickEvent != null)
                mirrorBtn_ClickEvent(sender, e);
        }

        private void augmentBtn_Click(object sender, EventArgs e)
        {
            if (augmentBtn_ClickEvent != null)
                augmentBtn_ClickEvent(sender, e);
        }

        private void smoothBtn_Click(object sender, EventArgs e)
        {
            if (smoothBtn_ClickEvent != null)
                smoothBtn_ClickEvent(sender, e);
        }

        private void gameBtn_Click(object sender, EventArgs e)
        {
            if (gameBtn_ClickEvent != null)
                gameBtn_ClickEvent(sender, e);
        }

        private void MainWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (mainWindow_MouseDownEvent != null)
                mainWindow_MouseDownEvent(sender, e);
        }

        private void customConfigBtn_Click(object sender, EventArgs e)
        {
            if (customConfigBtn_ClickEvent != null)
                customConfigBtn_ClickEvent(sender, e);
        }

        private void quitBtn_Click(object sender, EventArgs e)
        {
            if (quitBtn_ClickEvent != null)
                quitBtn_ClickEvent(sender, e);
        }

        private void setPollingSizeBtn_Click(object sender, EventArgs e)
        {
            if (setPollingBtn_ClickEvent != null)
                setPollingBtn_ClickEvent(sender, e);
        }

        private void onBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (onOffValueChanged != null)
                onOffValueChanged(onBtn.Checked, e);
        }

        private void devIDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (advancedDevSelectionChanged != null)
                advancedDevSelectionChanged(this.devIDList.SelectedItem, e);
        }

        private void whiteBalanceBtn_Click(object sender, EventArgs e)
        {
            if (whiteBalanceBtn_ClickEvent != null)
                whiteBalanceBtn_ClickEvent(sender, e);
        }
    }
}
