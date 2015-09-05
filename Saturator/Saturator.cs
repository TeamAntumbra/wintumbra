using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Forms;

namespace Saturator {

    [Export(typeof(GlowExtension))]
    public class Saturator : GlowFilter {

        #region Private Fields

        private int deviceId;
        private bool running = false;
        private SaturatorSettings settingsWin;

        #endregion Private Fields

        #region Public Properties

        public override string Author {
            get { return "Team Antumbra"; }
        }

        public override string Description {
            get { return "A filter to saturate the output color for richer, more satisfying color output."; }
        }

        public override int devId {
            get {
                return deviceId;
            }
            set {
                deviceId = value;
            }
        }

        public override Guid id {
            get { return Guid.Parse("2acba4a6-af21-47a9-9551-964a750fea06"); }
        }

        public override bool IsDefault {
            get { return true; }
        }

        public override bool IsRunning {
            get { return this.running; }
        }

        public override string Name {
            get { return "Saturator"; }
        }

        public override Version Version {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website {
            get { return "https://antumbra.io"; }
        }

        #endregion Public Properties

        #region Public Methods

        public override GlowFilter Create() {
            return new Saturator();
        }

        public override void Dispose() {
            if(settingsWin != null) {
                settingsWin.Dispose();
            }
        }

        public override Color16Bit Filter(Color16Bit origColor) {
            // Too dark to saturate with good results
            if(origColor.red < 5000 && origColor.green < 5000 && origColor.blue < 5000)
                return origColor;
            HslColor boringHSL = new HslColor(Color16BitUtil.ToRGBColor(origColor));
            double satAmnt = (double)Properties.Settings.Default.saturationAmount;
            double diff = Math.Abs(.5 - boringHSL.L);
            int dir = boringHSL.L > .5 ? -1 : 1;
            if(diff < satAmnt)
                boringHSL.L = .5;
            else
                boringHSL.L += dir * satAmnt;
            return Color16BitUtil.FromRGBColor(boringHSL.ToRgbColor());
        }

        public override bool Settings() {
            this.settingsWin = new SaturatorSettings(this);
            this.settingsWin.Show();
            this.settingsWin.saturateAmtTxt.Text = Properties.Settings.Default.saturationAmount.ToString();
            this.settingsWin.saturateAmtTxt.TextChanged += new EventHandler(SaturationTxtChanged);
            this.settingsWin.saveBtn.Click += new EventHandler(ApplyBtnClick);
            return true;
        }

        public override bool Start() {
            this.running = true;
            return true;
        }

        public override bool Stop() {
            if(this.settingsWin != null)
                this.settingsWin.Dispose();
            this.running = false;
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void ApplyBtnClick(object sender, EventArgs args) {
            Properties.Settings.Default.Save();
        }

        private void SaturationTxtChanged(object sender, EventArgs args) {
            TextBox bx = (TextBox)sender;
            try {
                Properties.Settings.Default.saturationAmount = double.Parse(bx.Text);
            } catch(Exception) {
                //bad input, ignore
            }
        }

        #endregion Private Methods
    }
}
