using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Utility;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Saturator
{
    [Export(typeof(GlowExtension))]
    public class Saturator : GlowDecorator
    {
        private bool running = false;
        private Dictionary<string, object> settings;
        public override int id { get; set; }
        public override bool IsDefault
        {
            get { return false; }
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
            get { return new Version("0.0.1"); }
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
            if (boringHSL.S < .35) { }//skip low saturation colors
            else if (boringHSL.S <= .65)
                boringHSL.S += .35;//(double)this.settings["Saturation Additive"]; //saturate
            else
                boringHSL.S = 1.0;
            return boringHSL.ToRgbColor();
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override Dictionary<string, object> Settings
        {
            get
            {
                return this.settings;
            }
            set
            {
                this.settings = Settings;
            }
        }

        public override bool Start()
        {
            this.settings = new Dictionary<string,object>();
            this.running = true;
            return true;
        }

        public override bool Stop()
        {
            this.running = false;
            return true;
        }
    }
}
