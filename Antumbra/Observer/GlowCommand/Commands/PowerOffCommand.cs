using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class PowerOffCommand : GlowCommand
    {
        public PowerOffCommand(int devId)
            : base(devId)
        {

        }

        public override void ExecuteCommand(DeviceManager mgr)
        {
            mgr.getDevice(id).Stop();
            mgr.sendColor(0, 0, 0, id);
        }
    }
}
