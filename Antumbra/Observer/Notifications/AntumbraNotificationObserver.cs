using System;

namespace Antumbra.Glow.Observer.Notifications {

    public interface AntumbraNotificationObserver//TODO
    {
        #region Public Methods

        void NewNotificationAvail(object sender, EventArgs args);

        #endregion Public Methods
    }
}
