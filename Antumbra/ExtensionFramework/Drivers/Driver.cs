using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework.Drivers
{
    class Driver : GlowExtension//Base for Driver extensions
    {
        public Color GetColor();//this function will be called in a loop to generate colors (tweakable in settings)
        public String Type { get { return "Driver"; } }
    }
}
