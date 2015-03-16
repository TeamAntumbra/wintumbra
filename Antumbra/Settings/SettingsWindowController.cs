using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Extensions;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.GlowCommands.Commands;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Connector;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Exceptions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Antumbra.Glow.Settings
{
    public class SettingsWindowController : ConfigurationObserver, GlowCommandSender, ToolbarNotificationSource,
                                            GlowExtCollectionObserver
    {
        public delegate void NewGlowCommandAvail(GlowCommand cmd);
        public event NewGlowCommandAvail NewGlowCommandAvailEvent;
        public delegate void NewToolbarNotifAvail(int time, String title, String msg, int icon);
        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;
        public int id;
        private SettingsWindow window;
        private GlowDevice dev;
        private BasicExtSettingsWinFactory settingsFactory;
        private pollingAreaSetter pollingAreaWindow;
        public SettingsWindowController(GlowDevice dev, String version, BasicExtSettingsWinFactory factory)
        {
            this.dev = dev;
            this.id = dev.id;
            this.settingsFactory = factory;
            this.window = new SettingsWindow(version);
            this.window.driverComboBox_SelectedIndexChangedEvent += new EventHandler(DriverComboBoxIndexChanged);
            this.window.driverRecomBtn_ClickEvent += new EventHandler(ApplyDriverRecomSettings);
            this.window.sleepSize_TextChangedEvent += new EventHandler(UpdateStepSleep);
            this.window.newColorWeight_TextChangedEvent += new EventHandler(UpdateNewColorWeight);
            this.window.updateGrabberProcessorChoiceEvent += new EventHandler(UpdateGrabberProcessorChoice);
            this.window.decoratorComboBx_SelectedIndexChangedEvent += new EventHandler(updateSelectedDecorator);
            this.window.toggleDecoratorEvent += new EventHandler(toggleDecorator);
            this.window.pollingArea_ClickEvent += new EventHandler(pollingArea_Click);
        }

        public void Show()
        {
            this.window.Show();
        }

        public void ConfigurationUpdate(Configurable obj)
        {
            try {
                if (obj is DeviceSettings)
                    this.window.UpdateConfiguration((DeviceSettings)obj, GetStatusString(this.dev.status));
                else
                    if (obj is ActiveExtensions) {
                        ActiveExtensions actives = (ActiveExtensions)obj;
                        UpdateActive(actives);
                    }
            }
            catch (ExtensionNotFoundException e) {
                NewToolbarNotifAvailEvent(3000, "Extension Not Found", e.Message, 1);
            }
        }

        private void UpdateActive(ActiveExtensions actives)
        {
            this.window.UpdatedComboBoxSelectedExt(actives.ActiveDriver.id, this.window.driverComboBox);
            this.window.UpdatedComboBoxSelectedExt(actives.ActiveGrabber.id, this.window.grabberComboBx);
            this.window.UpdatedComboBoxSelectedExt(actives.ActiveProcessor.id, this.window.processorComboBx);
            GlowDecorator dec = actives.ActiveDecorators.First<GlowDecorator>();
            if (dec != null)
                this.window.UpdatedComboBoxSelectedExt(dec.id, this.window.decoratorComboBx);
            //TODO notifier
        }

        public void LibraryUpdate(List<GlowExtension> exts)
        {
            List<GlowExtension> current = new List<GlowExtension>();
            foreach (GlowExtension ext in this.window.driverComboBox.Items)
                current.Add(ext);
            foreach (GlowExtension ext in this.window.grabberComboBx.Items)
                current.Add(ext);
            foreach (GlowExtension ext in this.window.processorComboBx.Items)
                current.Add(ext);
            foreach (GlowExtension ext in this.window.decoratorComboBx.Items)
                current.Add(ext);
            foreach (GlowExtension ext in exts) {
                if (CheckForExtInList(ext, current))//already have it
                    continue;
                //else add it
                if (ext is GlowDriver) {
                    this.window.driverComboBox.Items.Add(ext);
                }
                else if (ext is GlowScreenGrabber) {
                    this.window.grabberComboBx.Items.Add(ext);
                }
                else if (ext is GlowScreenProcessor) {
                    this.window.processorComboBx.Items.Add(ext);
                }
                else if (ext is GlowDecorator) {
                    this.window.decoratorComboBx.Items.Add(ext);
                }
                else if (ext is GlowNotifier) {
                    //TODO
                }
            }
        }

        private bool CheckForExtInList(GlowExtension ext, List<GlowExtension> list)
        {
            foreach (GlowExtension item in list)
                if (item.id.Equals(ext.id))
                    return true;
            return false;
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            this.NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public void RegisterDevice(int id)
        {
            //ignore, already have device object
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        /// <summary>
        /// Return the string representation of the given status value
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetStatusString(int status)
        {
            switch (status) {
                case 0:
                    return "Sending/Recieving Successfully";
                case 1:
                    return "Glow Device Disconnected";
                case 2:
                    return "LibAntumbra Memory Allocation Failed";
                case 3:
                    return "LibUSB Exception";
                case 4:
                    return "Device in Invalid State for Operation";
                case 5:
                    return "Index or Size Out of Range";
                case 6:
                    return "Protocol Command Not Supported";
                case 7:
                    return "Protocol Command Failure";
                case 8:
                    return "Unspecified Protocol Error";
                default:
                    return "Invalid Status";
            }
        }

        private void UpdateStepSleep(object sender, EventArgs args)
        {
            try {
                TextBox sleepSize = (TextBox)sender;
                this.dev.settings.stepSleep = Convert.ToInt32(sleepSize.Text);
            }
            catch (Exception) {
                Console.WriteLine("Update exception updating stepSleep in settings");
            }
        }

        private void UpdateNewColorWeight(object sender, EventArgs args)
        {
            try {
                TextBox newColorWeight = (TextBox)sender;
                int percent = Convert.ToInt16(newColorWeight.Text);
                if (percent >= 0 && percent <= 100)//valid
                    this.dev.settings.newColorWeight = Convert.ToDouble(percent / 100.0);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void UpdateWeightingEnabled(object sender, EventArgs args)
        {
            try {
                CheckBox weightingEnabled = (CheckBox)sender;
                this.dev.settings.weightingEnabled = weightingEnabled.Checked;
            }
            catch (Exception) {
                //TODO report
            }
        }

        private void SettingsWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            try {
                SettingsWindow win = (SettingsWindow)sender;
                win.Hide();
                e.Cancel = true;
            }
            catch (Exception) {
                //TODO report
            }
        }

        private void UpdateCompoundDecorationCheck(object sender, EventArgs a)
        {
            try {
                CheckBox box = (CheckBox)sender;
                this.dev.settings.compoundDecoration = box.Checked;
            }
            catch (Exception) {
                //TODO report
            }
        }

        private void DriverComboBoxIndexChanged(object sender, EventArgs a)
        {
            try {
                ComboBox box = (ComboBox)sender;
                GlowDriver ext = (GlowDriver)box.SelectedItem;
                UpdateDriverChoice(ext);
            }
            catch (Exception) {
                //TODO report
            }
        }

        private void UpdateDriverChoice(GlowDriver ext)
        {
            if (ext == null)
                return;
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(new StopCommand(this.dev.id));
            this.dev.SetDvrGbbrOrPrcsrExt(ext.id);
            bool screenBased = (ext is GlowScreenDriverCoupler);
            this.window.UpdateIfDriverScreenBased(screenBased);
        }

        private void UpdateGrabberProcessorChoice(object sender, EventArgs args)
        {
            try {
                GlowExtension ext = GetExtFromSender(sender);
                if (ext == null)
                    return;
                SendStopCommand();
                this.dev.SetDvrGbbrOrPrcsrExt(ext.id);
            }
            catch (Exception) {
                //TODO report
            }
        }

        private void updateSelectedDecorator(object sender, EventArgs a)
        {
            try {
                ComboBox box = (ComboBox)sender;
                GlowDecorator dec = (GlowDecorator)box.SelectedItem;
                if (dec != null)
                    this.window.SetCurrentDecStatus(this.dev.GetDecOrNotfStatus(dec.id).ToString());
            }
            catch (Exception) {
                //TODO report
            }
        }

        private void toggleDecorator(object sender, EventArgs args)
        {
            try {
                ComboBox box = (ComboBox)sender;
                if (HandleToggle(box))
                    this.window.SetCurrentDecStatus("True");
                else
                    this.window.SetCurrentDecStatus("False");
            }
            catch (Exception) {
                //TODO report
            }
        }

        private bool HandleToggle(ComboBox box)
        {
            GlowExtension ext = (GlowExtension)box.SelectedItem;
            if (ext == null)
                return false;
            SendStopCommand();
            return this.dev.SetDecOrNotf(ext.id);
        }

        private void AttemptToOpenSettingsWindow(Guid id)
        {
            try {
                if (!this.dev.GetExtSettingsWin(id)) {
                    var win = this.settingsFactory.GenerateWindow(id);
                    win.Show();
                }
            }
            catch (Exception) {
                NewToolbarNotifAvailEvent(3000, "No Selected Extension",
                    "There is no extension to open the settings of.", 1);
            }
        }

        private void ApplyDriverRecomSettings(object sender, EventArgs a)
        {
            this.dev.settings.stepSleep = this.dev.ApplyDriverRecomSettings();
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null || this.pollingAreaWindow.IsDisposed) {
                var current = this.dev.id;
                var back = GetUniquePollingColor();
                this.pollingAreaWindow = new pollingAreaSetter(this.dev.settings, back);
                NewGlowCommandAvailEvent(new StopCommand(current));
                NewGlowCommandAvailEvent(new SendColorCommand(current, back));//update device to unique color matching window
                this.pollingAreaWindow.FormClosing += new FormClosingEventHandler(UpdatePollingSelectionsEvent);
            }
            this.pollingAreaWindow.Show();
        }

        private Color GetUniquePollingColor()
        {
            //TODO ask settings window manager for unique color
            return Color.AliceBlue;
        }

        private void UpdatePollingSelectionsEvent(object sender, FormClosingEventArgs args)
        {
            Form form = (Form)sender;
            this.dev.settings.x = form.Bounds.X;
            this.dev.settings.y = form.Bounds.Y;
            this.dev.settings.width = form.Bounds.Width;
            this.dev.settings.height = form.Bounds.Height;
        }

        private void SendStopCommand()
        {
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(new StopCommand(this.dev.id));
        }

        private GlowExtension GetExtFromSender(object sender)
        {
            ComboBox box = (ComboBox)sender;
            return (GlowExtension)box.SelectedItem;
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
