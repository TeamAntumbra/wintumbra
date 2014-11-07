using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework.Drivers
{
    interface DriverInterface : GlowExtension
    {
        Color GetColor();//this function will be called in a loop to generate colors (tweakable in settings)
    }
}
