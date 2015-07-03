using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.ExtensionFramework.Types
{
    public abstract class GlowFilter : GlowExtension
    {
        abstract public Color16Bit Filter(Color16Bit origColor);//Returns filtered color
    }
}
