using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.View;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Settings;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Antumbra.Glow.Controller
{
    public class MainWindowController : Loggable, ToolbarNotificationSource, GlowCommandSender, GlowCommandObserver,
                                        ToolbarNotificationObserver, ConfigurationObserver, ConnectionEventObserver
    {
        public const Int32 VID = 0x16D0;
        public const Int32 PID = 0x0A85;

        public delegate void NewLogMsgAvail(String source, String msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewGlowCmdAvail(GlowCommand cmd);
        public event NewGlowCmdAvail NewGlowCmdAvailEvent;
        private event EventHandler quitEventHandler;
        private MainWindow window;
        private int glowCount, pollingIndex, id;
        private bool manual;
        private WhiteBalanceWindowController whiteBalController;
        private SettingsManager settingsManager;
        private ConnectionManager connectionManager;
        private ExtensionManager extensionManager;
        private PreOutputProcessor preOutputProcessor;
        private Color16Bit lastManualColor;
        private Color16Bit controlColor;
        public MainWindowController(string productVersion, EventHandler quitHandler)
        {
            controlColor = new Color16Bit(new Utility.HslColor(0, 0, 0.5).ToRgbColor());

            AttachObserver((LogMsgObserver)(LoggerHelper.GetInstance()));//attach logger
            // Create Manager instances
            connectionManager = new ConnectionManager(VID, PID);
            settingsManager = new SettingsManager();
            extensionManager = new ExtensionManager(new ExtensionLibrary());
            preOutputProcessor = new PreOutputProcessor();
            whiteBalController = new WhiteBalanceWindowController(settingsManager);
            // Attach event observers
            connectionManager.AttachObserver((ConnectionEventObserver)settingsManager);
            connectionManager.AttachObserver((ConnectionEventObserver)extensionManager);
            connectionManager.AttachObserver((ConnectionEventObserver)whiteBalController);
            preOutputProcessor.AttachObserver((AntumbraColorObserver)connectionManager);
            extensionManager.AttachObserver((AntumbraColorObserver)preOutputProcessor);
            settingsManager.AttachObserver((ConfigurationObserver)extensionManager);
            settingsManager.AttachObserver((ConfigurationObserver)preOutputProcessor);
            // Find devices
            connectionManager.UpdateDeviceConnections();

            AttachObserver((GlowCommandObserver)extensionManager);

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerModeChanged);
            this.window = new MainWindow();
            this.window.closeBtn_ClickEvent += new EventHandler(closeBtnClicked);
            this.window.colorWheel_ColorChangedEvent += new EventHandler(colorWheelColorChanged);
            this.window.brightnessTrackBar_ScrollEvent += new EventHandler(brightnessValueChanged);
            this.window.hsvBtn_ClickEvent += new EventHandler(hsvBtnClicked);
            this.window.sinBtn_ClickEvent += new EventHandler(sinBtnClicked);
            this.window.neonBtn_ClickEvent += new EventHandler(neonBtnClicked);
            this.window.mirrorBtn_ClickEvent += new EventHandler(mirrorBtnClicked);
            this.window.augmentBtn_ClickEvent += new EventHandler(augmentBtnClicked);
            this.window.smoothBtn_ClickEvent += new EventHandler(smoothBtnClicked);
            this.window.gameBtn_ClickEvent += new EventHandler(gameBtnClicked);
            this.window.mainWindow_MouseDownEvent += new System.Windows.Forms.MouseEventHandler(mouseDownEvent);
            this.window.quitBtn_ClickEvent += new EventHandler(quitBtnClicked);
            this.quitEventHandler += quitHandler;
            this.window.setPollingBtn_ClickEvent += new EventHandler(setPollingBtnClickHandler);
            this.window.onOffValueChanged += new EventHandler(OnOffValueChangedHandler);
            this.window.whiteBalanceBtn_ClickEvent += new EventHandler(whiteBalanceBtnClicked);
            this.window.throttleBar_ValueChange += new EventHandler(throttleBarValueChanged);
        }

        public void ConnectionUpdate(int devCount)
        {
            glowCount = devCount;
        }

        /// <summary>
        /// Update UI to match with new config announced
        /// </summary>
        /// <param name="config"></param>
        public void ConfigurationUpdate(Configurable config)
        {
            if (config is DeviceSettings) {//settings changed
                DeviceSettings settings = (DeviceSettings)config;
                this.window.SetCaptureThrottleValue(settings.captureThrottle);
                this.window.SetBrightnessValue((int)(settings.maxBrightness * 100));
                if (manual)//manual mode enabled
                    ResendManualColor(-1);//re-send to all devices
            }
        }

        public void ResendManualColor(int id)
        {
            this.colorWheelColorChanged(this.window.colorWheel.HslColor, EventArgs.Empty);
        }

        private void throttleBarValueChanged(object sender, EventArgs args)
        {
            //TODO
        }

        private void whiteBalanceBtnClicked(object sender, EventArgs args)
        {
            if (glowCount == 0) {
                this.ShowMessage(3000, "No Glows Found",
                    "White balance cannot be opened because no Glow devices were found.", 2);
                return;//can't open
            }
            NewGlowCmdAvailEvent(new StopCommand(-1));
            NewGlowCmdAvailEvent(new SendColorCommand(-1, this.controlColor));
            window.colorWheel.HslColor = new Utility.HslColor(0,0,.5);//reset selector to center
            window.colorWheel.Enabled = false;//disable main color wheel until color balance window closed
            whiteBalController.Show();
        }

        private void WhiteBalanceWindowClosingHandler(object sender, System.Windows.Forms.FormClosingEventArgs args)
        {
            this.window.colorWheel.Enabled = true;//re-enable main wheel
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);//pass it up
        }

        public void NewGlowCommandAvail(GlowCommand cmd)
        {
            if (NewGlowCmdAvailEvent != null)
                NewGlowCmdAvailEvent(cmd);//pass it up
        }

        private void setPollingBtnClickHandler(object sender, EventArgs args)
        {
            if (this.pollingIndex == glowCount)//all open
                return;
            this.pollingIndex += 1;
            if (this.pollingIndex == 1 && glowCount > 1) {//first device just triggered & more to go
                this.window.SetPollingBtnText("Next Device");
            }
            int id = pollingIndex - 1;
            PollingAreaWindowController cont = new PollingAreaWindowController(id);
            cont.PollingAreaUpdatedEvent += new PollingAreaWindowController.PollingAreaUpdated(UpdatePollingSelection);
            cont.AttachObserver(this);
            DeviceSettings settings = settingsManager.getSettings(id);
            cont.SetBounds(settings.x, settings.y, settings.width, settings.height);
            cont.Show();
        }

        private void UpdatePollingSelection(int id, int x, int y, int width, int height)
        {
            manual = false;//force false to stop resending of manual color
            DeviceSettings settings = settingsManager.getSettings(id);
            settings.x = x;
            settings.y = y;
            settings.width = width;
            settings.height = height;
            this.pollingIndex -= 1;
            if (this.pollingIndex == 0) {//time to reset btn text & restart
                NewGlowCommandAvail(new StartCommand(-1));
                this.window.SetPollingBtnText("Set Capture Area");
            }
        }

        private void OnOffValueChangedHandler(object sender, EventArgs args)
        {
            if (sender is bool) {
                bool on = (bool)sender;
                if (on)
                    if (manual)
                        ResendManualColor(-1);//can't 'start' manual mode
                    else
                        NewGlowCmdAvailEvent(new StartCommand(-1));
                else
                    NewGlowCmdAvailEvent(new PowerOffCommand(-1));
            }
        }

        public void RegisterDevice(int id)
        {
            this.id = id;
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            this.NewGlowCmdAvailEvent += observer.NewGlowCommandAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        private void ShowMessage(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
        }

        private void Log(string msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("MainWindowController", msg);
        }

        public void showWindowEventHandler(object sender, EventArgs args)
        {
            if (this.window.Visible)
                this.window.Activate();
            else
                this.window.Show();
        }

        public void restartEventHandler(object sender, EventArgs args)
        {
            this.NewGlowCmdAvailEvent(new StopCommand(-1));
            this.NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void closeBtnClicked(object sender, EventArgs args)
        {
            this.window.Hide();
        }

        public void colorWheelColorChanged(object sender, EventArgs args)
        {
            if (sender is Utility.HslColor) {
                manual = true;
                NewGlowCmdAvailEvent(new StopCommand(-1));//stop devices if running (dev mgr will check)
                Utility.HslColor col = (Utility.HslColor)sender;
                this.lastManualColor = new Color16Bit(col.ToRgbColor());
                connectionManager.sendColor(lastManualColor, -1);
            }
        }

        public void brightnessValueChanged(object sender, EventArgs args)
        {
            if (sender is int[]) {
                int[] values = (int[])sender;
                double value = (double)values[0] / values[1];
                for (int i = 0; i < connectionManager.GlowsFound; i += 1) {
                    settingsManager.getSettings(i).maxBrightness = value;
                }
            }
        }

        public void hsvBtnClicked(object sender, EventArgs args)
        {
            extensionManager.SetInstance(id, ExtensionManager.MODE.HSV);
        }

        public void sinBtnClicked(object sender, EventArgs args)
        {
            extensionManager.SetInstance(id, ExtensionManager.MODE.SIN);
        }

        public void neonBtnClicked(object sender, EventArgs args)
        {
            extensionManager.SetInstance(id, ExtensionManager.MODE.NEON);
        }

        public void mirrorBtnClicked(object sender, EventArgs args)
        {
            extensionManager.SetInstance(id, ExtensionManager.MODE.MIRROR);
        }

        public void augmentBtnClicked(object sender, EventArgs args)
        {
            extensionManager.SetInstance(id, ExtensionManager.MODE.AUGMENT);
        }

        public void smoothBtnClicked(object sender, EventArgs args)
        {
            extensionManager.SetInstance(id, ExtensionManager.MODE.SMOOTH);
        }

        public void gameBtnClicked(object sender, EventArgs args)
        {
            extensionManager.SetInstance(id, ExtensionManager.MODE.GAME);
        }

        public void quitBtnClicked(object sender, EventArgs args)
        {
            this.window.Close();
            NewGlowCmdAvailEvent(new PowerOffCommand(-1));//turn all devices off
            connectionManager.Dispose();
            if (quitEventHandler != null)
                quitEventHandler(sender, args);
        }

        /// <summary>
        /// Event handler for session switching. Used for handling locking and unlocking of the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason) {
                case SessionSwitchReason.SessionLogoff:
                case SessionSwitchReason.SessionLock:
                    NewGlowCmdAvailEvent(new PowerOffCommand(-1));//turn all off
                    break;
                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                    Thread.Sleep(2500);//wait for system to be ready
                    NewGlowCmdAvailEvent(new StartCommand(-1));//start all
                    break;
            }
        }
        /// <summary>
        /// Event handler for PowerModeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            // User is putting the system into standby 
            if (e.Mode == PowerModes.Suspend) {
                NewGlowCmdAvailEvent(new PowerOffCommand(-1));
            }
            else if (e.Mode == PowerModes.Resume) {
                Thread.Sleep(2500);//wait some for system to be ready
                NewGlowCmdAvailEvent(new StartCommand(-1));//start all
            }
        }

        public void mouseDownEvent(object sender, System.Windows.Forms.MouseEventArgs args)
        {
            //allows dragging of forms to move them (because of hidden menu bars and window frame)
            if (args.Button == System.Windows.Forms.MouseButtons.Left) {//drag with left mouse btn
                ReleaseCapture();
                SendMessage(this.window.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Move form dependencies
        /// </summary>
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
    }
}
