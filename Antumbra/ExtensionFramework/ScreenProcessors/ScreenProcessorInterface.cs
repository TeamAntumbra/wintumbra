using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework.ScreenProcessors
{
    public interface ScreenProcessorInterface : GlowExtension
    {
        Color Process(Bitmap screen);//process screen and return color
    }
}
