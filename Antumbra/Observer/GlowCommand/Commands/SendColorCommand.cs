using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class SendColorCommand : GlowCommand
    {
        private Color newColor;
        public SendColorCommand(int devId, Color newColor)
            : base(devId)
        {
            this.newColor = newColor;
        }

        public override void ExecuteCommand(ToolbarIcon core)
        {
            core.SendColor(this.id, this.newColor);
        }
    }
}
