using Antumbra.Glow.Observer.Colors;
using System;

namespace Antumbra.Glow.ExtensionFramework.Types {

    public abstract class GlowDriver : GlowExtension {

        #region Public Properties

        public double newColorWeight { get; set; }

        public int stepSleep { get; set; }

        public bool weighted { get; set; }

        #endregion Public Properties

        #region Public Methods

        public abstract void AttachColorObserver(AntumbraColorObserver observer);

        public abstract GlowDriver Create();

        public sealed override Type GetExtensionType() {
            return typeof(GlowDriver);
        }

        public abstract void RecmmndCoreSettings();

        #endregion Public Methods
    }
}
