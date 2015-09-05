namespace Antumbra.Glow.Observer.Configuration {

    public interface ConfigurationChangeAnnouncer {

        #region Public Methods

        void AttachObserver(ConfigurationChanger observer);

        #endregion Public Methods
    }
}
