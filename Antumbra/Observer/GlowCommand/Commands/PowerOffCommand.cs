using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class PowerOffCommand : GlowCommand
    {
        public PowerOffCommand(int devId)
            : base(devId)
        {

        }

        public override void ExecuteCommand(AntumbraCore core)
        {
            core.Off();//TODO add id for specific device
        }
    }
}
