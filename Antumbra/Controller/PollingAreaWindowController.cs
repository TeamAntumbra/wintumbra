using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using Antumbra.Glow.Utility;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Controller
{
    public class PollingAreaWindowController : GlowCommandSender
    {
        public delegate void PollingAreaUpdated(int id, int x, int y, int width, int height);
        public event PollingAreaUpdated PollingAreaUpdatedEvent;
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private View.pollingAreaSetter pollingWindow;
        private Color color;
        public int id { get; private set; }
        public PollingAreaWindowController(int id)
        {
            this.id = id;
            this.color = UniqueColorGenerator.GetInstance().GetUniqueColor();
            this.pollingWindow = new View.pollingAreaSetter(this.color);
            this.pollingWindow.formClosingEvent += new EventHandler(UpdatePollingSelectionsEvent);
        }

        public void Show()
        {
            this.pollingWindow.Show();
            SendCommand(new StopCommand(-1));//stop all
            SendCommand(new SendColorCommand(this.id, new Color16Bit(this.color)));//set to unique color to match its window
        }

        public void SetBounds(int x, int y, int width, int height)
        {
            MoveWindow(this.pollingWindow.Handle, x, y, width, height, true);
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingWindow == null || this.pollingWindow.IsDisposed) {
                var back = UniqueColorGenerator.GetInstance().GetUniqueColor();
                this.pollingWindow = new View.pollingAreaSetter(back);
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

        private void SendCommand(GlowCommand cmd)
        {
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(cmd);
        }

        private void UpdatePollingSelectionsEvent(object sender, EventArgs args)
        {
            if (sender is System.Windows.Forms.Form) {
                System.Windows.Forms.Form form = (System.Windows.Forms.Form)sender;
                if (PollingAreaUpdatedEvent != null)
                    PollingAreaUpdatedEvent(this.id, form.Location.X, form.Location.Y, form.Width, form.Height);
                UniqueColorGenerator.GetInstance().RetireUniqueColor(form.BackColor);
                form.Hide();
            }
        }

        /// <summary>
        /// The MoveWindow function changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="X">Specifies the new position of the left side of the window.</param>
        /// <param name="Y">Specifies the new position of the top of the window.</param>
        /// <param name="nWidth">Specifies the new width of the window.</param>
        /// <param name="nHeight">Specifies the new height of the window.</param>
        /// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para></returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    }
}
