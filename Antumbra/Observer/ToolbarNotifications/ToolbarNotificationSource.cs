namespace Antumbra.Glow.Observer.ToolbarNotifications {

    public interface ToolbarNotificationSource {

        #region Public Methods

        void AttachObserver(ToolbarNotificationObserver observer);

        #endregion Public Methods
    }
}
