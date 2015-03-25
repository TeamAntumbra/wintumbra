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
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Settings;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Antumbra.Glow.Controller
{
    public class MainWindowController : Loggable, ToolbarNotificationSource, GlowCommandSender, GlowCommandObserver,
                                        ToolbarNotificationObserver
    {
        public delegate void NewLogMsgAvail(String source, String msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewGlowCmdAvail(GlowCommand cmd);
        public event NewGlowCmdAvail NewGlowCmdAvailEvent;
        public bool goodStart { get; private set; }
        private event EventHandler quitEventHandler;
        private const string extPath = "./Extensions/";
        private MainWindow window;
        private PresetBuilder presetBuilder;
        private DeviceManager deviceMgr;
        private AdvancedSettingsWindowManager advSettingsMgr;
        private int id;
        public MainWindowController(String productVersion, EventHandler quitHandler)
        {
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
            this.window.customConfigBtn_ClickEvent += new EventHandler(customConfigBtnClicked);
            this.window.quitBtn_ClickEvent += new EventHandler(quitBtnClicked);
            this.window.setPollingBtn_ClickEvent += new EventHandler(setPollingBtnClickHandler);
            this.window.onOffValueChanged += new EventHandler(OnOffValueChangedHandler);
            ExtensionLibrary extLibrary = null;
            try {
                extLibrary = new ExtensionLibrary(extPath);//load extensions into lib
            }
            catch (System.Reflection.ReflectionTypeLoadException e) {
                string msg = "";
                foreach (var err in e.LoaderExceptions)
                    msg += err.Message;
                ShowMessage(10000, "Exception Occured While Loading Extensions", msg, 2);
                Thread.Sleep(10000);//wait for message
                throw e;//pass up
            }
            this.Log("Creating DeviceManager");
            this.deviceMgr = new DeviceManager(0x16D0, 0x0A85, extLibrary, productVersion);//find devices
            this.deviceMgr.AttachObserver(this);
            this.AttachObserver((GlowCommandObserver)this.deviceMgr);
            this.quitEventHandler += quitHandler;
            if (this.deviceMgr.GlowsFound > 0) {//ready first device for output if any are found
                GlowDevice dev = this.deviceMgr.getDevice(0);
                this.RegisterDevice(dev.id);
            }
            this.presetBuilder = new PresetBuilder(extLibrary);
            this.advSettingsMgr = new AdvancedSettingsWindowManager(productVersion, extLibrary);
            this.advSettingsMgr.AttachObserver((ToolbarNotificationObserver)this);
            this.advSettingsMgr.AttachObserver((GlowCommandObserver)this);
        }

        public void NewToolbarNotifAvail(int time, string title, string msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);//pass it up
        }

        public void NewGlowCommandAvail(GlowCommand cmd)
        {
            if (NewGlowCmdAvailEvent != null) {
                if (cmd is StartCommand)
                    this.window.SetOnSelection(true);
                if (cmd is PowerOffCommand)
                    this.window.SetOnSelection(false);
                NewGlowCmdAvailEvent(cmd);//pass it up
            }
        }

        private void setPollingBtnClickHandler(object sender, EventArgs args)
        {
            PollingAreaWindowController cont = new PollingAreaWindowController();
            cont.PollingAreaUpdatedEvent += new PollingAreaWindowController.PollingAreaUpdated(UpdatePollingSelection);
            cont.AttachObserver(this);
            cont.Show();
        }

        private void UpdatePollingSelection(int x, int y, int width, int height)
        {
            foreach (GlowDevice dev in this.deviceMgr.Glows) {//TODO change for easier multi-glow setup
                dev.settings.x = x;
                dev.settings.y = y;
                dev.settings.width = width;
                dev.settings.height = height;
            }
        }

        private void OnOffValueChangedHandler(object sender, EventArgs args)
        {
            if (sender is bool) {
                bool on = (bool)sender;
                if (on)
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
                foreach (GlowDevice dev in this.deviceMgr.Glows) {
                    dev.settings.weightingEnabled = false;
                }
                this.window.SetOnSelection(true);//mark device on
                NewGlowCmdAvailEvent(new StopCommand(-1));//stop devices if running (dev mgr will make check)
                Utility.HslColor col = (Utility.HslColor)sender;
                NewGlowCmdAvailEvent(new SendColorCommand(-1, new Color16Bit(col.ToRgbColor())));
            }
        }

        public void brightnessValueChanged(object sender, EventArgs args)
        {
            if (sender is int[]) {
                int[] values = (int[])sender;
                double value = (double)values[0] / values[1];
                foreach (GlowDevice dev in this.deviceMgr.Glows) {
                    dev.settings.maxBrightness = Convert.ToUInt16(UInt16.MaxValue * value);
                }
            }
        }

        private void ApplyNewSetup(ActiveExtensions actives, int stepSleep, bool weighted, double weight)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));//stop all
            foreach (GlowDevice dev in this.deviceMgr.Glows) {
                dev.SetActives(actives);
                dev.settings.weightingEnabled = weighted;
                dev.settings.newColorWeight = weight;
                dev.settings.stepSleep = stepSleep;
            }
            NewGlowCmdAvailEvent(new StartCommand(-1));//start all
            this.window.SetOnSelection(true);//mark device on
        }

        private void ApplyNewSetup(ActiveExtensions actives)
        {
            NewGlowCmdAvailEvent(new StopCommand(-1));//stop all
            foreach (GlowDevice dev in this.deviceMgr.Glows) {
                dev.SetActives(actives);
                dev.ApplyDriverRecomSettings();
            }
            NewGlowCmdAvailEvent(new StartCommand(-1));//start all
            this.window.SetOnSelection(true);//mark device on
        }

        public void hsvBtnClicked(object sender, EventArgs args)
        {
            ApplyNewSetup(this.presetBuilder.GetHSVFadePreset());
        }

        public void sinBtnClicked(object sender, EventArgs args)
        {
            ApplyNewSetup(this.presetBuilder.GetSinFadePreset());
        }

        public void neonBtnClicked(object sender, EventArgs args)
        {
            ApplyNewSetup(this.presetBuilder.GetNeonFadePreset());
        }

        public void mirrorBtnClicked(object sender, EventArgs args)
        {
            ApplyNewSetup(this.presetBuilder.GetMirrorPreset());
        }

        public void augmentBtnClicked(object sender, EventArgs args)
        {
            ApplyNewSetup(this.presetBuilder.GetAugmentMirrorPreset(), 0, true, .05);
        }

        public void smoothBtnClicked(object sender, EventArgs args)
        {
            ApplyNewSetup(this.presetBuilder.GetSmoothMirrorPreset(), 1, true, .05);
        }

        public void gameBtnClicked(object sender, EventArgs args)
        {
            ApplyNewSetup(this.presetBuilder.GetGameMirrorPreset());
        }

        public void customConfigBtnClicked(object sender, EventArgs args)
        {
            if (!this.advSettingsMgr.Show(this.id)) {
                this.advSettingsMgr.CreateAndAddNewController(this.deviceMgr.getDevice(this.id));
                this.advSettingsMgr.Show(this.id);
            }
        }

        public void quitBtnClicked(object sender, EventArgs args)
        {
            this.window.Close();
            NewGlowCmdAvailEvent(new PowerOffCommand(-1));//turn all devices off
            this.deviceMgr.CleanUp();
            this.advSettingsMgr.CleanUp();
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
                    //this.Off();
                    Console.WriteLine("locked/logged off");
                    break;
                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                    Console.WriteLine("unlocked/logged on");
                   // StartAllAfterDelay();
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
                //this.Off();
                Console.WriteLine("Suspended.");
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
