namespace Antumbra.Glow.Observer.Connection {

    public interface ConnectionEventSource {

        #region Public Methods

        void AttachObserver(ConnectionEventObserver observer);

        #endregion Public Methods
    }
}
