using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.Notifications {
    public interface AntumbraNotificationObserver//TODO
    {
        void NewNotificationAvail(object sender, EventArgs args);
    }
}
