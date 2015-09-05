using System;
using System.Windows.Forms;

namespace Antumbra.Glow.View {

    public partial class MainWindow : Form {

        #region Public Constructors

        public MainWindow() {
            InitializeComponent();
            this.brightnessTrackBar.Value = this.brightnessTrackBar.Maximum;
            this.versionLabel.Text = "v" + this.ProductVersion.ToString();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler augmentBtn_ClickEvent;

        public event EventHandler brightnessTrackBar_ScrollEvent;

        public event EventHandler closeBtn_ClickEvent;

        public event EventHandler colorWheel_ColorChangedEvent;

        public event EventHandler customConfigBtn_ClickEvent;

        public event EventHandler hsvBtn_ClickEvent;

        public event MouseEventHandler mainWindow_MouseDownEvent;

        public event EventHandler mirrorBtn_ClickEvent;

        public event EventHandler neonBtn_ClickEvent;

        public event EventHandler outputRateBtn_ClickEvent;

        public event EventHandler pwrBtn_ClickEvent;

        public event EventHandler quitBtn_ClickEvent;

        public event EventHandler setPollingBtn_ClickEvent;

        public event EventHandler sinBtn_ClickEvent;

        public event EventHandler smoothBtn_ClickEvent;

        public event EventHandler throttleBar_ValueChange;

        public event EventHandler whiteBalanceBtn_ClickEvent;

        #endregion Public Events

        #region Public Methods

        public void SetBrightnessValue(int value) {
            if(brightnessTrackBar.Value != value) {
                brightnessTrackBar.Value = value;
            }
        }

        public void SetCaptureThrottleValue(int value) {
            if(throttleBar.Value != value) {
                throttleBar.Value = value;
            }
        }

        public void SetPollingBtnText(String newText) {
            this.setPollingSizeBtn.Text = newText;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void offBtn_Click(object sender, EventArgs e) {
            if(pwrBtn_ClickEvent != null)
                pwrBtn_ClickEvent(false, EventArgs.Empty);
        }

        protected void onBtn_Click(object sender, EventArgs e) {
            if(pwrBtn_ClickEvent != null)
                pwrBtn_ClickEvent(true, EventArgs.Empty);
        }

        #endregion Protected Methods

        #region Private Methods

        private void augmentBtn_Click(object sender, EventArgs e) {
            if(augmentBtn_ClickEvent != null)
                augmentBtn_ClickEvent(sender, e);
        }

        private void brightnessTrackBar_Scroll(object sender, EventArgs e) {
            if(brightnessTrackBar_ScrollEvent != null) {
                int max = this.brightnessTrackBar.Maximum;
                double current = this.brightnessTrackBar.Value;
                brightnessTrackBar_ScrollEvent(current / max, e);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e) {
            if(closeBtn_ClickEvent != null)
                closeBtn_ClickEvent(sender, e);
        }

        private void colorWheel_ColorChanged(object sender, EventArgs e) {
            if(colorWheel_ColorChangedEvent != null)
                colorWheel_ColorChangedEvent(this.colorWheel.HslColor, e);
        }

        private void customConfigBtn_Click(object sender, EventArgs e) {
            if(customConfigBtn_ClickEvent != null)
                customConfigBtn_ClickEvent(sender, e);
        }

        private void hsvBtn_Click(object sender, EventArgs e) {
            if(hsvBtn_ClickEvent != null)
                hsvBtn_ClickEvent(sender, e);
        }

        private void MainWindow_MouseDown(object sender, MouseEventArgs e) {
            if(mainWindow_MouseDownEvent != null)
                mainWindow_MouseDownEvent(sender, e);
        }

        private void mirrorBtn_Click(object sender, EventArgs e) {
            if(mirrorBtn_ClickEvent != null)
                mirrorBtn_ClickEvent(sender, e);
        }

        private void neonBtn_Click(object sender, EventArgs e) {
            if(neonBtn_ClickEvent != null)
                neonBtn_ClickEvent(sender, e);
        }

        private void outputRateBtn_Click(object sender, EventArgs e) {
            if(outputRateBtn_ClickEvent != null)
                outputRateBtn_ClickEvent(sender, e);
        }

        private void quitBtn_Click(object sender, EventArgs e) {
            if(quitBtn_ClickEvent != null)
                quitBtn_ClickEvent(sender, e);
        }

        private void setPollingSizeBtn_Click(object sender, EventArgs e) {
            if(setPollingBtn_ClickEvent != null)
                setPollingBtn_ClickEvent(sender, e);
        }

        private void sinBtn_Click(object sender, EventArgs e) {
            if(sinBtn_ClickEvent != null)
                sinBtn_ClickEvent(sender, e);
        }

        private void smoothBtn_Click(object sender, EventArgs e) {
            if(smoothBtn_ClickEvent != null)
                smoothBtn_ClickEvent(sender, e);
        }

        private void throttleBar_ValueChanged(object sender, EventArgs e) {
            if(throttleBar_ValueChange != null && sender is TrackBar) {
                TrackBar bar = (TrackBar)sender;
                throttleBar_ValueChange(bar.Value, e);
            }
        }

        private void whiteBalanceBtn_Click(object sender, EventArgs e) {
            if(whiteBalanceBtn_ClickEvent != null)
                whiteBalanceBtn_ClickEvent(sender, e);
        }

        #endregion Private Methods
    }
}
