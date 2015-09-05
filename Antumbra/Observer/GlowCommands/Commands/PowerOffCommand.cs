using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.Observer.GlowCommands.Commands {

    public class PowerOffCommand : GlowCommand {

        #region Public Constructors

        public PowerOffCommand(int id)
            : base(id) {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void ExecuteCommand(ExtensionManager mgr) {
            mgr.Off(id);
        }

        #endregion Public Methods
    }
}
