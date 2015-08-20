using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class StopAndSendColorCommand : GlowCommand
    {
        private Color16Bit newColor;
        public StopAndSendColorCommand(int id, Color16Bit newColor)
            : base(id)
        {
            this.newColor = newColor;
        }

        public override void ExecuteCommand(ExtensionFramework.Management.ExtensionManager mgr)
        {
            mgr.StopAndSendColor(newColor, id);
        }
    }
}
