using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.Observer.GlowCommands {

    public abstract class GlowCommand : IGlowCommand {

        #region Public Constructors

        public GlowCommand(int id) {
            this.id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public int id { get; protected set; }

        #endregion Public Properties

        #region Public Methods

        public abstract void ExecuteCommand(ExtensionManager mgr);

        #endregion Public Methods
    }
}
