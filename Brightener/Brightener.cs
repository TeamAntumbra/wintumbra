using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Observer.Colors;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;

namespace Brightener
{
    [Export(typeof(GlowExtension))]
    public class Brightener : GlowDecorator
    {
        private bool running;
        private BrightenerSettings settingsWin;
        public override string Name
        {
            get { return "Brightener"; }
        }
        public override Guid id
        {
            get { return Guid.Parse("1a271e63-5f7e-43c0-bbb1-7d80d23d8db7"); }
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

        public override Color16Bit Decorate(Color16Bit origColor)
        {
            HslColor hsl = new HslColor(origColor.ToRGBColor());
            if (hsl.L > (1.0 - (double)Properties.Settings.Default.amountToLighten))
                hsl.L = 1.0;
            else
                hsl.L += (double)Properties.Settings.Default.amountToLighten;
            return new Color16Bit(hsl.ToRgbColor());
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
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override bool Settings()
        {
            this.settingsWin = new BrightenerSettings(this);
            this.settingsWin.Show();
            this.settingsWin.percBrightenTxt.Text = Properties.Settings.Default.amountToLighten.ToString();
            this.settingsWin.percBrightenTxt.TextChanged += new EventHandler(PercentChanged);
            this.settingsWin.saveBtn.Click += new EventHandler(ApplyBtnClick);
            return true;
        }

        private void ApplyBtnClick(object sender, EventArgs args)
        {
            Properties.Settings.Default.Save();
        }

        private void PercentChanged(object sender, EventArgs args)
        {
            TextBox bx = (TextBox)sender;
            try {
                Properties.Settings.Default.amountToLighten = double.Parse(bx.Text);
            }
            catch (Exception) {
                //invalid input
            }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }
    }
}
