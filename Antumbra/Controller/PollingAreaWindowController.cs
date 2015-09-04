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
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Settings;

namespace Antumbra.Glow.Controller {
    public class PollingAreaWindowController : GlowCommandSender, Loggable, IDisposable, ConnectionEventObserver {
        public delegate void NewLogMsg(string source, string msg);
        public event NewLogMsg NewLogMsgEvent;
        public delegate void PollingAreaUpdated(Dictionary<int, Settings.SettingsDelta> PollingAreaChanges);
        public event PollingAreaUpdated PollingAreaUpdatedEvent;
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        private List<View.pollingAreaSetter> pollingWindows;
        private Dictionary<int, SettingsDelta> PollingAreaChanges;
        private Rectangle boundRange;

        public PollingAreaWindowController() {
            AttachObserver(LoggerHelper.GetInstance());
            int x = int.MaxValue, y = int.MaxValue, width = 0, height = int.MaxValue;
            foreach(var screen in System.Windows.Forms.Screen.AllScreens) {
                x = x > screen.Bounds.X ? screen.Bounds.X : x;
                y = y > screen.Bounds.Y ? screen.Bounds.Y : y;
                width += screen.Bounds.Width;
                height = height > screen.Bounds.Height ? screen.Bounds.Height : height;
            }
            boundRange = new Rectangle(x, y, width, height);
            pollingWindows = new List<View.pollingAreaSetter>();
            PollingAreaChanges = new Dictionary<int, Settings.SettingsDelta>();
        }

        public void ShowAll() {
            for(int i = 0; i < pollingWindows.Count; i += 1) {
                View.pollingAreaSetter window = pollingWindows[i];
                if(window.IsDisposed) {
                    window = new View.pollingAreaSetter(window.BackColor, window.id);
                    pollingWindows[window.id] = window;
                }
                window.formClosingEvent += new EventHandler(UpdatePollingSelectionsEvent);
                window.Show();
                //Set to unique color to match its window
                SendCommand(new StopAndSendColorCommand(window.id, Color16BitUtil.FromRGBColor(window.BackColor)));
            }
        }

        public void SetBounds(int id, int x, int y, int width, int height) {
            View.pollingAreaSetter pollingWindow = pollingWindows[id];
            if(pollingWindow.IsDisposed) {
                pollingWindow = new View.pollingAreaSetter(pollingWindow.BackColor, id);
                pollingWindows[id] = pollingWindow;
            }

            if(boundRange.Contains(new Rectangle(x, y, width, height))) {
                MoveWindow(pollingWindow.Handle, x, y, width, height, true);
            } else {
                Log("Invalid SetBounds parameters passed.\tx: " + x + ", y: " + y +
                    ", width: " + width + ", height: " + height);
                x = x < boundRange.X ? boundRange.X : x;
                y = y < 0 ? 0 : y;
                width = width > 0 ? width : 500;
                height = height > 0 ? height : 375;
                Log("Params funneled to x: " + x + " y: " + y + " width: " + width + " height: " + height);
                MoveWindow(pollingWindow.Handle, x, y, width, height, true);
            }
        }

        public void ConnectionUpdate(int devCount) {
            foreach(View.pollingAreaSetter window in pollingWindows) {
                window.Dispose();
            }
            pollingWindows.Clear();
            for(int i = 0; i < devCount; i += 1) {
                pollingWindows.Add(new View.pollingAreaSetter(UniqueColorGenerator.GetInstance().GetUniqueColor(), i));
            }
        }

        public void AttachObserver(GlowCommandObserver observer) {
            NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgEvent += observer.NewLogMsgAvail;
        }

        public void Dispose() {
            foreach(View.pollingAreaSetter pollingWindow in pollingWindows) {
                if(!pollingWindow.IsDisposed) {
                    pollingWindow.Dispose();
                }
            }
        }

        private bool AllClosed(int excludeId) {
            foreach(var window in pollingWindows) {
                if(window.Visible && window.id != excludeId) {
                    return false;
                }
            }
            return true;
        }

        private void SendCommand(GlowCommand cmd) {
            if(NewGlowCommandAvailEvent != null) {
                NewGlowCommandAvailEvent(cmd);
            }
        }

        private void SendPollingUpdatesEvent(int excludeId) {
            if(AllClosed(excludeId) && PollingAreaUpdatedEvent != null) {
                PollingAreaUpdatedEvent(PollingAreaChanges);
                PollingAreaChanges.Clear();
            }
        }

        private void UpdatePollingSelectionsEvent(object sender, EventArgs args) {
            if(sender is View.pollingAreaSetter) {
                View.pollingAreaSetter window = (View.pollingAreaSetter)sender;
                if(PollingAreaUpdatedEvent != null) {
                    Log("Polling window " + window.id + " closed with bounds: " + window.Bounds);
                    SettingsDelta Delta = new SettingsDelta();
                    Delta.changes[SettingValue.X] = window.Bounds.X;
                    Delta.changes[SettingValue.Y] = window.Bounds.Y;
                    Delta.changes[SettingValue.WIDTH] = window.Bounds.Width;
                    Delta.changes[SettingValue.HEIGHT] = window.Bounds.Height;
                    PollingAreaChanges.Add(window.id, Delta);
                    SendPollingUpdatesEvent(window.id);
                }
                UniqueColorGenerator.GetInstance().RetireUniqueColor(window.BackColor);
            }
        }

        private void Log(string msg) {
            if(NewLogMsgEvent != null) {
                NewLogMsgEvent("PollingAreaWindowController", msg);
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
