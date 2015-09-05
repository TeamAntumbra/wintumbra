namespace Antumbra.Glow.Observer.GlowCommands {

    public interface GlowCommandSender {

        #region Public Methods

        void AttachObserver(GlowCommandObserver observer);

        #endregion Public Methods
    }
}
