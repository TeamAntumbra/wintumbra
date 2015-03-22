using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Observer.GlowCommands
{
    public abstract class GlowCommand : IGlowCommand
    {
        public int id { get; private set; }
        public GlowCommand(int id)
        {
            this.id = id;
        }

        public abstract void ExecuteCommand(DeviceManager mgr);
    }
}
