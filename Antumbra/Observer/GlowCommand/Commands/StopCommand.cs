using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class StopCommand : GlowCommand
    {
        public StopCommand(int devId)
            : base(devId)
        {

        }

        public override void ExecuteCommand(DeviceManager mgr)
        {
            foreach (GlowDevice dev in mgr.Glows)
                mgr.Stop(dev.id);
        }
    }
}
