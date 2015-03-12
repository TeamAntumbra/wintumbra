using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.Observer.GlowCommands
{
    public interface GlowCommandSender
    {
        void AttachObserver(GlowCommandObserver observer);
        void RegisterDevice(int id);
    }
}
