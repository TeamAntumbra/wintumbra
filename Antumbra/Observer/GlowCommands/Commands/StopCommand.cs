namespace Antumbra.Glow.Observer.GlowCommands.Commands {

    public class StopCommand : GlowCommand {

        #region Public Constructors

        public StopCommand(int id)
            : base(id) {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void ExecuteCommand(ExtensionFramework.Management.ExtensionManager mgr) {
            mgr.Stop(id);
        }

        #endregion Public Methods
    }
}
