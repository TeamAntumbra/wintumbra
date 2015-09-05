using Antumbra.Glow.ExtensionFramework;
using System.Collections.Generic;

namespace Antumbra.Glow.Observer.Extensions {

    public interface GlowExtCollectionObserver {

        #region Public Methods

        void LibraryUpdate(List<GlowExtension> extensions);

        #endregion Public Methods
    }
}
