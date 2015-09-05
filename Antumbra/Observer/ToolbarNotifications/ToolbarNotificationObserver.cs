using System;

namespace Antumbra.Glow.Observer.ToolbarNotifications {

    public interface ToolbarNotificationObserver {

        #region Public Methods

        void NewToolbarNotifAvail(int time, String title, String msg, int icon);

        #endregion Public Methods
    }
}
