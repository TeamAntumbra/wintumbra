using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.GlowCommands
{
    public interface GlowCommandObserver
    {
        void NewGlowCommandAvail(GlowCommand command);
    }
}
