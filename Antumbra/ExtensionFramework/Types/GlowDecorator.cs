using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.ExtensionFramework.Types
{
    public abstract class GlowDecorator : GlowExtension
    {
        abstract public Color16Bit Decorate(Color16Bit origColor);//Returns decorated color
    }
}
