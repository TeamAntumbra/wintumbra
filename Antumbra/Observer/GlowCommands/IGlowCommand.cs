using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.Observer.GlowCommands {

    public interface IGlowCommand {

        #region Public Methods

        void ExecuteCommand(ExtensionManager mgr);

        #endregion Public Methods
    }
}
