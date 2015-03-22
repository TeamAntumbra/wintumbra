using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Controller;
using Antumbra.Glow.Observer.GlowCommands;

namespace Antumbra.Glow.Settings
{
    public class PollingAreaWindowManager : GlowCommandSender, GlowCommandObserver
    {
        public delegate void NewGlowCmdAvail(GlowCommand cmd);
        public event NewGlowCmdAvail NewGlowCommandAvailEvent;
        public PollingAreaWindowManager()
        {

        }

        public void CreateAndAddController(int id)
        {
            PollingAreaWindowController cont = new PollingAreaWindowController();
            cont.RegisterDevice(id);//register related to passed device via id
            cont.AttachObserver(this);//attach to pass through
        }

        public void RegisterDevice(int id)
        {
            //ignore, simply a pass through not registered to a single device
        }

        public void NewGlowCommandAvail(GlowCommand cmd)
        {
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(cmd);//pass up
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            this.NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }
    }
}
