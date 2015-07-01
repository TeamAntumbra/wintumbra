using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Observer.Colors;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Forms;

namespace Saturator
{
    [Export(typeof(GlowExtension))]
    public class Saturator : GlowFilter
    {
        private bool running = false;
        private SaturatorSettings settingsWin;

        public override Guid id
        {
            get { return Guid.Parse("2acba4a6-af21-47a9-9551-964a750fea06"); }
        }

        public override bool IsDefault
        {
            get { return true; }
        }
        public override string Name
        {
            get { return "Saturator"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Description
        {
            get { return "A filter to saturate the output color for richer, more satisfying color output."; }
        }

        public override string Website
        {
            get { return "https://antumbra.io"; }
        }

        public override GlowExtension Create()
        {
            return new Saturator();
        }

        public override Color16Bit Filter(Color16Bit origColor)
        {
            // Too dark to saturate with good results
            if (origColor.red < 5000 && origColor.green < 5000 && origColor.blue < 5000)
                return origColor;
            HslColor boringHSL = new HslColor(origColor.ToRGBColor());
            double satAmnt = (double)Properties.Settings.Default.saturationAmount;
            double diff = Math.Abs(.5 - boringHSL.L);
            int dir = boringHSL.L > .5 ? -1 : 1;
            if (diff < satAmnt)
                boringHSL.L = .5;
            else
                boringHSL.L += dir * satAmnt;
            return new Color16Bit(boringHSL.ToRgbColor());
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override bool Settings()
        {
            this.settingsWin = new SaturatorSettings(this);
            this.settingsWin.Show();
            this.settingsWin.saturateAmtTxt.Text = Properties.Settings.Default.saturationAmount.ToString();
            this.settingsWin.saturateAmtTxt.TextChanged += new EventHandler(SaturationTxtChanged);
            this.settingsWin.saveBtn.Click += new EventHandler(ApplyBtnClick);
            return true;
        }

        private void ApplyBtnClick(object sender, EventArgs args)
        {
            Properties.Settings.Default.Save();
        }

        private void SaturationTxtChanged(object sender, EventArgs args)
        {
            TextBox bx = (TextBox)sender;
            try {
                Properties.Settings.Default.saturationAmount = double.Parse(bx.Text);
            }
            catch (Exception) { 
                //bad input, ignore
            }
        }
        
        public override bool Start()
        {
            this.running = true;
            return true;
        }

        public override bool Stop()
        {
            if (this.settingsWin != null)
                this.settingsWin.Dispose();
            this.running = false;
            return true;
        }
    }
}
