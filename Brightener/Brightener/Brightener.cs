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
        private AntumbraExtSettingsWindow settings;
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
            if (hsl.L > (1.0 - .15))//TODO make the .15 (or amount lightness is upped) be configurable
                hsl.L = 1.0;
            else
                hsl.L += .15;
            return hsl.ToRgbColor();
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override bool Start()
        {
            if (this.settings != null)
                this.settings.Dispose();
            this.running = true;
            return true;
        }

        public override bool Stop()
        {
            this.running = false;
            return true;
        }

        public override Version Version
        {
            get { return new Version("0.0.1"); }
        }

        public override void Settings()
        {
            this.settings = new AntumbraExtSettingsWindow(this);//TODO make this include the custom settings
            this.settings.Show();
        }
        public override string Website
        {
            get { throw new NotImplementedException(); }
        }
    }
}
