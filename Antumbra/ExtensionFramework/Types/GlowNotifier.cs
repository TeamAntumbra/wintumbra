using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Notifications;

namespace Antumbra.Glow.ExtensionFramework.Types {
    public abstract class GlowNotifier : GlowExtension//observed by core
    {
        public abstract void AttachEvent(AntumbraNotificationObserver observer);
        public abstract GlowNotifier Create();
        public sealed override Type GetExtensionType() {
            return typeof(GlowNotifier);
        }
    }
}
