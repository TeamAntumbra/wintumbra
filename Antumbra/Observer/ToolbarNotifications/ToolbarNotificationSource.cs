using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.ToolbarNotifications
{
    public interface ToolbarNotificationSource
    {
        void AttachToolbarNotifObserver(ToolbarNotificationObserver observer);
    }
}