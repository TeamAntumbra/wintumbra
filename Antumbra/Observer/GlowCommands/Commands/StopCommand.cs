using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class StopCommand : GlowCommand
    {
        public StopCommand(int id)
            : base(id)
        {

        }

        public override void ExecuteCommand(ExtensionFramework.Management.ExtensionManager mgr)
        {
            mgr.Stop(id);
        }
    }
}
