using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework.Decorators
{
    interface DecoratorInterface : GlowExtension
    {
        Color Decorate(Color original);//called by main loop to modify generated color
    }
}
