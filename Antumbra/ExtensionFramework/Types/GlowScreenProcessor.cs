using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.ScreenInfo;
using System;
using System.Collections.Generic;

namespace Antumbra.Glow.ExtensionFramework.Types {

    public abstract class GlowScreenProcessor : GlowExtension, AntumbraScreenInfoObserver, AntumbraColorSource {

        #region Public Methods

        public abstract void AttachObserver(AntumbraColorObserver observer);

        public abstract GlowScreenProcessor Create();

        public sealed override Type GetExtensionType() {
            return typeof(GlowScreenProcessor);
        }

        public abstract void NewScreenInfoAvail(List<int[, ,]> pixelArrays, EventArgs args);

        public abstract void SetArea(int x, int y, int width, int height);

        #endregion Public Methods
    }
}
