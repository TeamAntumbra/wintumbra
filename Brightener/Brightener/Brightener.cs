using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Utility;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace Brightener
{
    [Export(typeof(GlowExtension))]
    public class Brightener : GlowDecorator
    {
        public override int id { get; set; }
        private bool running;
        private BrightenerSettings settingsWin;
        public override string Name
        {
            get { return "Brightener"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "Brightens colors using the HSL color model."; }
        }

        public override bool IsDefault
        {
            get { return true; }
        }

        public override Color Decorate(Color origColor)
        {
            HslColor hsl = new HslColor(origColor);
            if (hsl.L > (1.0 - (double)Properties.Settings.Default["amountToLighten"]))
                hsl.L = 1.0;
            else
                hsl.L += (double)Properties.Settings.Default["amountToLighten"];
            return hsl.ToRgbColor();
        }

        public override bool IsRunning
        {
            get { return this.running; }
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

        public override Version Version
        {
            get { return new Version("0.0.1"); }
        }

        public override bool Settings()
        {
            this.settingsWin = new BrightenerSettings(this);
            this.settingsWin.Show();
            this.settingsWin.percBrightenTxt.Text = Properties.Settings.Default["amountToLighten"].ToString();
            this.settingsWin.percBrightenTxt.TextChanged += new EventHandler(PercentChanged);
            return true;
        }

        private void PercentChanged(object sender, EventArgs args)
        {
            TextBox bx = (TextBox)sender;
            try {
                Properties.Settings.Default["amountToLighten"] = double.Parse(bx.Text);
            }
            catch (Exception e) {
                //invalid input
            }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }
    }
}
