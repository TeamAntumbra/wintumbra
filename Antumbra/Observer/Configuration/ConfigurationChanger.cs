namespace Antumbra.Glow.Observer.Configuration {

    public interface ConfigurationChanger {

        #region Public Methods

        void ConfigChange(Settings.SettingsDelta Delta);

        #endregion Public Methods
    }
}
