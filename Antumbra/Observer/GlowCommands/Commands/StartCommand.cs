namespace Antumbra.Glow.Observer.GlowCommands.Commands {

    public class StartCommand : GlowCommand {

        #region Public Constructors

        public StartCommand(int id)
            : base(id) {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void ExecuteCommand(ExtensionFramework.Management.ExtensionManager mgr) {
            mgr.Start(id);
        }

        #endregion Public Methods
    }
}
