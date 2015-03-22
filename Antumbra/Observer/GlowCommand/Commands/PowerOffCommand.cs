using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Observer.GlowCommands.Commands
{
    public class PowerOffCommand : GlowCommand
    {
        public PowerOffCommand(int devId)
            : base(devId)
        {

        }

        public override void ExecuteCommand(DeviceManager mgr)
        {
            if (this.id == -1)//turn off all
                foreach (GlowDevice dev in mgr.Glows) {
                    mgr.Stop(dev.id);
                    mgr.sendColor(0, 0, 0, dev.id);
                }
            else {
                mgr.getDevice(id).Stop();
                mgr.sendColor(0, 0, 0, id);
            }
        }
    }
}
