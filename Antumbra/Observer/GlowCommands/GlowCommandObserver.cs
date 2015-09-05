namespace Antumbra.Glow.Observer.GlowCommands {

    public interface GlowCommandObserver {

        #region Public Methods

        void NewGlowCommandAvail(GlowCommand command);

        #endregion Public Methods
    }
}
