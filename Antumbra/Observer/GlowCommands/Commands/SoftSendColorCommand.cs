using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Observer.GlowCommands.Commands {

    public class SoftSendColorCommand : GlowCommand {

        #region Private Fields

        private Color16Bit newColor;

        #endregion Private Fields

        #region Public Constructors

        public SoftSendColorCommand(int id, Color16Bit newColor)
            : base(id) {
            this.newColor = newColor;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void ExecuteCommand(ExtensionFramework.Management.ExtensionManager mgr) {
            mgr.SoftSendColor(newColor, id);
        }

        #endregion Public Methods
    }
}
