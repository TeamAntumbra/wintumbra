using Antumbra.Glow.Observer.Colors;
using System;

namespace Antumbra.Glow.ExtensionFramework.Types {

    public abstract class GlowFilter : GlowExtension {

        #region Public Methods

        public abstract GlowFilter Create();

        abstract public Color16Bit Filter(Color16Bit origColor);//Returns filtered color

        public sealed override Type GetExtensionType() {
            return typeof(GlowFilter);
        }

        #endregion Public Methods
    }
}
