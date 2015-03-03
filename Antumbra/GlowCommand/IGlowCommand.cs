using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.GlowCommands
{
    public interface IGlowCommand
    {
        void ExecuteCommand(AntumbraCore core);
    }
}
