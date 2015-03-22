using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using System.Drawing;

namespace Antumbra.Glow.Controller
{
    public class PollingAreaWindowController : GlowCommandSender
    {
        public delegate void PollingAreaUpdated(int x, int y, int height, int width);
        public event PollingAreaUpdated PollingAreaUpdatedEvent;
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private View.pollingAreaSetter pollingWindow;
        private int id;
        public PollingAreaWindowController()
        {
            View.pollingAreaSetter pollingWindow = new View.pollingAreaSetter(UniqueColorGenerator.GetInstance().GetUniqueColor());
            pollingWindow.formClosingEvent += new EventHandler(UpdatePollingSelectionsEvent);
        }
        
        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingWindow == null || this.pollingWindow.IsDisposed) {
                var back = UniqueColorGenerator.GetInstance().GetUniqueColor();
                this.pollingWindow = new View.pollingAreaSetter(back);
                SendStopCommand();
                SendColorCommand(back);
                this.pollingWindow.formClosingEvent += new EventHandler(UpdatePollingSelectionsEvent);
            }
            this.pollingWindow.Show();
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            this.NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public void RegisterDevice(int id)
        {
            this.id = id;
        }

        private void SendStopCommand()
        {
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(new StopCommand(this.id));
        }

        private void SendColorCommand(Color col)
        {
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(new SendColorCommand(this.id, col));
        }

        private void UpdatePollingSelectionsEvent(object sender, EventArgs args)
        {
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)sender;
            if (PollingAreaUpdatedEvent != null)
                PollingAreaUpdatedEvent(form.Bounds.X, form.Bounds.Y, form.Width, form.Height);
            UniqueColorGenerator.GetInstance().RetireUniqueColor(form.BackColor);
        }
    }
}
