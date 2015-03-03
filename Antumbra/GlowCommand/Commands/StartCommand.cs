using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.GlowCommands.Commands
{
    public class StartCommand : GlowCommand
    {
        public StartCommand(int devId)
            : base(devId)
        {

        }
        public override void ExecuteCommand(AntumbraCore core)
        {
            core.Start(this.id);
        }

    }
}
