using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.Observer.GlowCommands
{
    public abstract class GlowCommand : IGlowCommand
    {
        public int id { get; protected set; }
        public GlowCommand(int id)
        {
            this.id = id;
        }

        public abstract void ExecuteCommand(ExtensionManager mgr);
    }
}
