using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class StopCommand : GlowCommand
    {
        public StopCommand(int devId)
            : base(devId)
        {

        }

        public override void ExecuteCommand(ToolbarIcon core)
        {
            core.Stop(this.id);
        }
    }
}
