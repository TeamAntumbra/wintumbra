using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.Connector;

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

        public override void ExecuteCommand(DeviceManager mgr)
        {
            mgr.sendColor(newColor, id);
        }
    }
}
