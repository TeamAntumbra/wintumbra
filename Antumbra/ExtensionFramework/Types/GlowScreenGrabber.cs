using Antumbra.Glow.Observer.ScreenInfo;
using System;

namespace Antumbra.Glow.ExtensionFramework.Types {

    public abstract class GlowScreenGrabber : GlowExtension//observed by screen processor
        //special type of driver that deals with bitmaps captured from the screen
        //uses a GlowScreenProcessor to determine color to return
    {
        #region Public Properties

        public int captureThrottle { get; set; }

        public int height { get; set; }

        public int width { get; set; }

        public int x { get; set; }

        public int y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public abstract void AttachObserver(AntumbraScreenInfoObserver observer);

        public abstract GlowScreenGrabber Create();

        public sealed override Type GetExtensionType() {
            return typeof(GlowScreenGrabber);
        }

        #endregion Public Methods
    }
}
