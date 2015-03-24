using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Controller
{
    public class PollingAreaWindowController : GlowCommandSender
    {
        public delegate void PollingAreaUpdated(int x, int y, int height, int width);
        public event PollingAreaUpdated PollingAreaUpdatedEvent;
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private View.pollingAreaSetter pollingWindow;
        private Color16Bit color;
        public int id { get; private set; }
        public PollingAreaWindowController()
        {
            this.color = new Color16Bit(UniqueColorGenerator.GetInstance().GetUniqueColor());
            View.pollingAreaSetter pollingWindow = new View.pollingAreaSetter(color.ToRGBColor());
            pollingWindow.formClosingEvent += new EventHandler(UpdatePollingSelectionsEvent);
        }

        public void Show()
        {
            pollingWindow.Show();
            SendStopCommand();//stop device
            SendColorCommand(this.color);//set to unique color to match its window
        }
        
        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingWindow == null || this.pollingWindow.IsDisposed) {
                var back = UniqueColorGenerator.GetInstance().GetUniqueColor();
                this.pollingWindow = new View.pollingAreaSetter(back);
                SendStopCommand();
                SendColorCommand(new Color16Bit(back));
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

        private void SendColorCommand(Color16Bit col)
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
