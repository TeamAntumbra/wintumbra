using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework.Decorators
{
    class Decorator : GlowExtension//Base for Decorator extensions
    {
        public Color Decorate(Color original);//called by main loop to modify generated color
        public String Type { get { return "Decorator"; } }
    }
}
