using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;//might not be needed TODO

namespace ExampleGlowDriver
{
    [Export(typeof(GlowExtension))]
    public class ExampleGlowDriver : GlowDriver
    {
        public override System.Drawing.Color getColor()
        {
            return Color.Lavender;
        }

        public override string Name
        {
            get { return "Example Glow Driver"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Version
        {
            get { return "V0.1.0"; }
        }

        public override string Description
        {
            get { return "A super simple implementation example " +
                         "of a Glow Driver extension that always " +
                         "returns Lavender. :)"; }
        }
    }
}
