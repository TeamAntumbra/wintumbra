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
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Settings;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Antumbra.Glow.Controller
{
    public class MainWindowController : Loggable, ToolbarNotificationSource, GlowCommandSender, GlowCommandObserver,
                                        ToolbarNotificationObserver, ConfigurationObserver
    {
        public delegate void NewLogMsgAvail(String source, String msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewGlowCmdAvail(GlowCommand cmd);
        public event NewGlowCmdAvail NewGlowCmdAvailEvent;
        private event EventHandler quitEventHandler;
        private const string extPath = "./Extensions/";
        private MainWindow window;
        private PresetBuilder presetBuilder;
        private DeviceManager deviceMgr;
        private WhiteBalanceWindowController whiteBalController;
        private int id;
        private bool manual;
        private Color16Bit lastManualColor;
        private Color16Bit controlColor;
        private int pollingIndex;
        public MainWindowController()
        {
            this.manual = false;
            this.AttachObserver((LogMsgObserver)(LoggerHelper.GetInstance()));//attach logger
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
            this.window.setPollingBtn_ClickEvent += new EventHandler(setPollingBtnClickHandler);
            this.window.onOffValueChanged += new EventHandler(OnOffValueChangedHandler);
            this.window.whiteBalanceBtn_ClickEvent += new EventHandler(whiteBalanceBtnClicked);
            this.window.throttleBar_ValueChange += new EventHandler(throttleBarValueChanged);
        }

        private void throttleBarValueChanged(object sender, EventArgs e)
        {
            int value = int.Parse(sender.ToString());
            foreach (GlowDevice dev in deviceMgr.Glows) {
                dev.settings.captureThrottle = value;
            }
        }

        public bool Setup(string productVersion, EventHandler quitHandler)
        {
            ExtensionLibrary extLibrary;
            try {
                extLibrary = new ExtensionLibrary(extPath);//load extensions into lib
            }
            catch (Exception ex) {
                string msg = "";
                if (ex is System.Reflection.ReflectionTypeLoadException) {
                    System.Reflection.ReflectionTypeLoadException e = (System.Reflection.ReflectionTypeLoadException)ex;
                    foreach (var err in e.LoaderExceptions)
                        msg += err.Message + '\n' + err.StackTrace;
                }
                else
                    msg = ex.Message + '\n' + ex.StackTrace;
                ShowMessage(10000, "Exception Occured While Loading Extensions", msg, 2);
                this.Log(msg);
                Thread.Sleep(10000);//wait for message
                return false;//failed
            }
            this.Log("Creating DeviceManager");
            this.pollingIndex = 0;
            this.deviceMgr = new DeviceManager(0x16D0, 0x0A85, extLibrary, productVersion);//find devices
            this.deviceMgr.AttachObserver(this);
            this.AttachObserver((GlowCommandObserver)this.deviceMgr);
            this.quitEventHandler += quitHandler;
            if (this.deviceMgr.GlowsFound > 0) {//ready first device for output if any are found
                foreach (GlowDevice device in this.deviceMgr.Glows) {
                    device.AttachObserver((ConfigurationObserver)this);
                    device.AttachObserver((ToolbarNotificationObserver)this);
                    device.LoadSettings();
                }
                this.whiteBalController = new WhiteBalanceWindowController(this.deviceMgr.Glows);//setup white balancer to control all devices
            }
            else {//no devices found
                ShowMessage(5000, "No Glows Found", "No Glow devices were found.  Please ensure " +
                    "your device is connected and glowing green before starting the application", 2);
                Thread.Sleep(5000);
                return false;
            }
            this.presetBuilder = new PresetBuilder(extLibrary);
            this.controlColor = new Color16Bit(new Utility.HslColor(0, 0, .5).ToRgbColor());
            NewGlowCmdAvailEvent(new StartCommand(-1));
            return true;
        }

        public void ConfigurationUpdate(Configurable config)
        {
            if (config is DeviceSettings) {//settings changed
                DeviceSettings settings = (DeviceSettings)config;
                this.window.SetOnSelection(settings.powerState);
                this.window.SetCaptureThrottleValue(settings.captureThrottle);
                this.window.SetBrightnessValue(settings.maxBrightness);
                if (manual)//manual mode enabled
                    ResendManualColor(-1);//re-send to all devices
            }
        }

        public void ResendManualColor(int id)
        {
            this.colorWheelColorChanged(this.window.colorWheel.HslColor, EventArgs.Empty);
        }

        private void whiteBalanceBtnClicked(object sender, EventArgs args)
        {
            if (this.deviceMgr.GlowsFound == 0) {
                this.ShowMessage(3000, "No Glows Found",
                    "White balance cannot be opened because no Glow devices were found.", 2);
                return;//can't open, controller is most likely null anyways
            }
            NewGlowCmdAvailEvent(new StopCommand(-1));
            NewGlowCmdAvailEvent(new SendColorCommand(-1, this.controlColor));
            this.window.colorWheel.HslColor = new Utility.HslColor(0,0,.5);//reset selector to center
            this.window.colorWheel.Enabled = false;//disable main color wheel until color balance window closed
            this.whiteBalController.Show(WhiteBalanceWindowClosingHandler);
        }

        private void WhiteBalanceWindowClosingHandler(object sender, System.Windows.Forms.FormClosingEventArgs args)
        {
            this.window.colorWheel.Enabled = true;//re-enable main wheel
        }

        private void advDevSelectionChangedHandler(object sender, EventArgs args)
        {
            if (sender is int) {
                int selection = (int)sender;
                this.RegisterDevice(selection);
            }
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
            if (this.pollingIndex == this.deviceMgr.GlowsFound)//all open
                return;
            this.pollingIndex += 1;
            if (this.pollingIndex == 1 && this.deviceMgr.GlowsFound > 1) {//first device just triggered & more to go
                this.window.SetPollingBtnText("Next Device");
            }
            int id = pollingIndex - 1;
            PollingAreaWindowController cont = new PollingAreaWindowController(id);
            cont.PollingAreaUpdatedEvent += new PollingAreaWindowController.PollingAreaUpdated(UpdatePollingSelection);
            cont.AttachObserver(this);
            DeviceSettings settings = this.deviceMgr.getDevice(id).settings;
            cont.SetBounds(settings.x, settings.y, settings.width, settings.height);
            cont.Show();
        }

        private void UpdatePollingSelection(int id, int x, int y, int width, int height)
        {
            this.manual = false;//force false to stop resending of manual color
            GlowDevice dev = this.deviceMgr.getDevice(id);
            dev.settings.x = x;
            dev.settings.y = y;
            dev.settings.width = width;
            dev.settings.height = height;
            this.pollingIndex -= 1;
            if (this.pollingIndex == 0) {//time to reset btn text & restart
                this.deviceMgr.Start(-1);//re-start all
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

        public void closeBtnClicked(object sender, EventArgs args)
        {
            this.window.Hide();
        }

        public void colorWheelColorChanged(object sender, EventArgs args)
        {
            if (sender is Utility.HslColor) {
                manual = true;
                foreach (GlowDevice dev in this.deviceMgr.Glows) {
                    if (dev.settings.weightingEnabled)
                        dev.settings.weightingEnabled = false;
                }
                NewGlowCmdAvailEvent(new StopCommand(-1));//stop devices if running (dev mgr will check)
                Utility.HslColor col = (Utility.HslColor)sender;
                this.lastManualColor = new Color16Bit(col.ToRgbColor());
                NewGlowCmdAvailEvent(new SendColorCommand(-1, lastManualColor));
            }
        }

        public void brightnessValueChanged(object sender, EventArgs args)
        {
            if (sender is int[]) {
                int[] values = (int[])sender;
                double value = (double)values[0] / values[1];
                UInt16 max = UInt16.MaxValue;
                foreach (GlowDevice dev in this.deviceMgr.Glows) {
                    max = Convert.ToUInt16(UInt16.MaxValue * value);
                    dev.settings.maxBrightness = max;
                }
            }
        }

        private Color16Bit ApplyBrightnessSettings(Color16Bit filtered, UInt16 maxBrightness)
        {
            UInt16 red = Convert.ToUInt16(((double)filtered.red / UInt16.MaxValue) * maxBrightness);
            UInt16 green = Convert.ToUInt16(((double)filtered.green / UInt16.MaxValue) * maxBrightness);
            UInt16 blue = Convert.ToUInt16(((double)filtered.blue / UInt16.MaxValue) * maxBrightness);
            return new Color16Bit(red, green, blue);
        }

        private void ApplyNewSetup(int id, ActiveExtensions actives, int stepSleep, bool weighted, double weight)
        {
            ApplyNewSetup(id, actives);
            GlowDevice dev = this.deviceMgr.getDevice(id);
            dev.settings.weightingEnabled = weighted;
            dev.settings.newColorWeight = weight;
            dev.settings.stepSleep = stepSleep;
        }

        private void ApplyNewSetup(int id, ActiveExtensions actives)
        {
            manual = false;
            GlowDevice dev = this.deviceMgr.getDevice(id);
            dev.SetActives(actives);
            dev.ApplyDriverRecomSettings();
        }

        public void hsvBtnClicked(object sender, EventArgs args)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));
            foreach (GlowDevice dev in this.deviceMgr.Glows)
                ApplyNewSetup(dev.id, this.presetBuilder.GetHSVFadePreset());
            NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void sinBtnClicked(object sender, EventArgs args)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));
            foreach (GlowDevice dev in this.deviceMgr.Glows)
                ApplyNewSetup(dev.id, this.presetBuilder.GetSinFadePreset());
            NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void neonBtnClicked(object sender, EventArgs args)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));
            foreach (GlowDevice dev in this.deviceMgr.Glows)
                ApplyNewSetup(dev.id, this.presetBuilder.GetNeonFadePreset());
            NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void mirrorBtnClicked(object sender, EventArgs args)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));
            foreach (GlowDevice dev in this.deviceMgr.Glows)
                ApplyNewSetup(dev.id, this.presetBuilder.GetMirrorPreset(), 1, true, 0.25);
            NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void augmentBtnClicked(object sender, EventArgs args)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));
            foreach (GlowDevice dev in this.deviceMgr.Glows)
                ApplyNewSetup(dev.id, this.presetBuilder.GetAugmentMirrorPreset(), 1, true, .05);
            NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void smoothBtnClicked(object sender, EventArgs args)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));
            foreach (GlowDevice dev in this.deviceMgr.Glows)
                ApplyNewSetup(dev.id, this.presetBuilder.GetSmoothMirrorPreset(), 1, true, .1);
            NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void gameBtnClicked(object sender, EventArgs args)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));
            foreach (GlowDevice dev in this.deviceMgr.Glows)
                ApplyNewSetup(dev.id, this.presetBuilder.GetGameMirrorPreset(), 1, true, .1);
            NewGlowCmdAvailEvent(new StartCommand(-1));
        }

        public void quitBtnClicked(object sender, EventArgs args)
        {
            this.window.Close();
            NewGlowCmdAvailEvent(new PowerOffCommand(-1));//turn all devices off
            this.deviceMgr.CleanUp();
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
                    if (manual)
                        ResendManualColor(-1);
                    else
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
                Thread.Sleep(2500);//wait for system to be ready
                if (manual)
                    ResendManualColor(-1);
                else
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
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
    }
}
