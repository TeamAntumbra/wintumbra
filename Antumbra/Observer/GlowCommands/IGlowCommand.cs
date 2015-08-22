using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.Observer.GlowCommands {
    public interface IGlowCommand {
        void ExecuteCommand(ExtensionManager mgr);
    }
}
