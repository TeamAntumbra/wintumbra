using Antumbra.Glow.Observer.Notifications;
using System;

namespace Antumbra.Glow.ExtensionFramework.Types {

    public abstract class GlowNotifier : GlowExtension//observed by core
    {
        #region Public Methods

        public abstract void AttachEvent(AntumbraNotificationObserver observer);

        public abstract GlowNotifier Create();

        public sealed override Type GetExtensionType() {
            return typeof(GlowNotifier);
        }

        #endregion Public Methods
    }
}
