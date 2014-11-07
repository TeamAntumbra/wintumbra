using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework.ScreenProcessors
{
    abstract class ScreenProcessor : GlowExtension //Base for screen processor extensions
    {
        public Color Process(Bitmap screen);//process screen and return color
        public String Type { get { return "Screen Processor"; } }
    }
}
