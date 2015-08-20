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
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.Controller
{
    public class PollingAreaWindowController : GlowCommandSender, Loggable
    {
        public delegate void NewLogMsg(string source, string msg);
        public event NewLogMsg NewLogMsgEvent;
        public delegate void PollingAreaUpdated(int id, int x, int y, int width, int height);
        public event PollingAreaUpdated PollingAreaUpdatedEvent;
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private View.pollingAreaSetter pollingWindow;
        private Color color;
        private Rectangle boundRange;
        public int id { get; private set; }

        public PollingAreaWindowController(int id)
        {
            AttachObserver(LoggerHelper.GetInstance());
            this.id = id;
            int x = int.MaxValue, y = int.MaxValue, width = 0, height = int.MaxValue;
            foreach(var screen in System.Windows.Forms.Screen.AllScreens) {
                x = x > screen.Bounds.X ? screen.Bounds.X : x;
                y = y > screen.Bounds.Y ? screen.Bounds.Y : y;
                width += screen.Bounds.Width;
                height = height > screen.Bounds.Height ? screen.Bounds.Height : height;
            }
            boundRange = new Rectangle(x, y, width, height);
            color = UniqueColorGenerator.GetInstance().GetUniqueColor();
            pollingWindow = new View.pollingAreaSetter(this.color);
            pollingWindow.formClosingEvent += new EventHandler(UpdatePollingSelectionsEvent);
        }

        public void Show()
        {
            pollingWindow.Show();
            SendCommand(new StopCommand(id));//stop all
            SendCommand(new StopAndSendColorCommand(id, new Color16Bit(color)));//set to unique color to match its window
        }

        public void SetBounds(int x, int y, int width, int height)
        {
            if (boundRange.Contains(new Rectangle(x, y, width, height))) {
                MoveWindow(pollingWindow.Handle, x, y, width, height, true);
            }
            else {
                Log("Invalid SetBounds parameters passed.\tx: " + x + ", y: " + y +
                    ", width: " + width + ", height: " + height);
                MoveWindow(pollingWindow.Handle, 200, 200, 500, 500, true);
            }
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgEvent += observer.NewLogMsgAvail;
        }

        private void SendCommand(GlowCommand cmd)
        {
            if (NewGlowCommandAvailEvent != null) {
                NewGlowCommandAvailEvent(cmd);
            }
        }

        private void UpdatePollingSelectionsEvent(object sender, EventArgs args)
        {
            if (sender is System.Windows.Forms.Form) {
                System.Windows.Forms.Form form = (System.Windows.Forms.Form)sender;
                if (PollingAreaUpdatedEvent != null) {
                    Log("Polling window " + id + " closed with bounds: " + form.Bounds);
                    PollingAreaUpdatedEvent(id, form.Location.X, form.Location.Y, form.Width, form.Height);
                }
                UniqueColorGenerator.GetInstance().RetireUniqueColor(form.BackColor);
            }
        }

        private void Log(string msg)
        {
            if (NewLogMsgEvent != null) {
                NewLogMsgEvent("PollingAreaWindowController " + id, msg);
            }
        }

        /// <summary>
        /// The MoveWindow function changes the position and dimensions of the specified window. For a top-level window,
        /// the position and dimensions are relative to the upper-left corner of the screen. For a child window,
        /// they are relative to the upper-left corner of the parent window's client area.
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
