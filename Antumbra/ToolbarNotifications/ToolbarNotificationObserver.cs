using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antumbra.Glow.ToolbarNotifications
{
    public interface ToolbarNotificationObserver
    {
        void NewToolbarNotifAvail(int time, String title, String msg, int icon);
    }
}
