using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Extensions
{
    public interface GlowExtCollection
    {
        void AttachGlowExtCollectionObserver(GlowExtCollectionObserver observer);
    }
}
