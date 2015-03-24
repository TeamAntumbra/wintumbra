using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class SendColorCommand : GlowCommand
    {
        private Color16Bit newColor;
        public SendColorCommand(int devId, Color16Bit newColor)
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
