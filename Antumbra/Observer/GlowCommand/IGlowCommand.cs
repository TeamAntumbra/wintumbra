using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Observer.GlowCommands
{
    public interface IGlowCommand
    {
        void ExecuteCommand(DeviceManager mgr);
    }
}
