using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Utility;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Saturator
{
    [Export(typeof(GlowExtension))]
    public class Saturator : GlowDecorator
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
            get { return "A decorator to saturate the output color for richer, more satisfying color output."; }
        }

        public override string Website
        {
            get { return "https://antumbra.io"; }
        }

        public override Color Decorate(Color origColor)
        {
            HslColor boringHSL = new HslColor(origColor);
            double satAmnt = (double)Properties.Settings.Default.saturationAmount;
            if (boringHSL.S < satAmnt) { }//skip low saturation colors TODO make this its own value
            else if (boringHSL.S <= (1.0-satAmnt))
                boringHSL.S += satAmnt; //saturate
            else
                boringHSL.S = 1.0;
            return boringHSL.ToRgbColor();
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
