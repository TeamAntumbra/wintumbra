using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;

namespace Antumbra.Glow.Observer.Extensions
{
    public interface GlowExtCollectionObserver
    {
        void LibraryUpdate(List<GlowExtension> extensions);
    }
}
