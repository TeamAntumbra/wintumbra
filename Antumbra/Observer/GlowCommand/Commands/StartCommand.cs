using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class StartCommand : GlowCommand
    {
        public StartCommand(int devId)
            : base(devId)
        {

        }

        public override void ExecuteCommand(DeviceManager mgr)
        {
            mgr.Start(this.id);
        }

    }
}
