using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class PowerOffCommand : GlowCommand
    {
        public PowerOffCommand(int devId)
            : base(devId)
        {

        }

        public override void ExecuteCommand(ExtensionManager mgr)
        {
            mgr.Off(id);
        }
    }
}
