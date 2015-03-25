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
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Connector;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.View;
using Antumbra.Glow.Exceptions;
using Antumbra.Glow.Utility;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Antumbra.Glow.Controller
{
    public class AdvancedSettingsWindowController : ConfigurationObserver, GlowCommandSender, ToolbarNotificationSource,
                                            GlowExtCollectionObserver, GlowCommandObserver
    {
        public delegate void NewGlowCmdAvail(GlowCommand cmd);
        public event NewGlowCmdAvail NewGlowCommandAvailEvent;
        public delegate void NewToolbarNotifAvail(int time, String title, String msg, int icon);
        public event NewToolbarNotifAvail NewToolbarNotifAvailEvent;
        public int id;
        private AdvancedSettingsWindow window;
        private GlowDevice dev;
        private AntumbraExtSettingsWindow.ExtWindowFactory settingsFactory;
        public AdvancedSettingsWindowController(GlowDevice dev, String version, AntumbraExtSettingsWindow.ExtWindowFactory factory)
        {
            this.dev = dev;
            this.dev.AttachObserver(this);
            this.id = dev.id;
            this.settingsFactory = factory;
            this.window = new AdvancedSettingsWindow(version);
            this.window.driverComboBox_SelectedIndexChangedEvent += new EventHandler(DriverComboBoxIndexChanged);
            this.window.driverRecomBtn_ClickEvent += new EventHandler(ApplyDriverRecomSettings);
            this.window.sleepSize_TextChangedEvent += new EventHandler(UpdateStepSleep);
            this.window.newColorWeight_TextChangedEvent += new EventHandler(UpdateNewColorWeight);
            this.window.updateGrabberProcessorChoiceEvent += new EventHandler(UpdateGrabberProcessorChoice);
            this.window.decoratorComboBx_SelectedIndexChangedEvent += new EventHandler(updateSelectedDecorator);
            this.window.toggleDecoratorEvent += new EventHandler(toggleDecorator);
            this.window.pollingArea_ClickEvent += new EventHandler(pollingArea_Click);
            this.window.startBtn_ClickEvent += new EventHandler(StartBtnClickHandler);
            this.window.stopBtn_ClickEvent += new EventHandler(StopBtnClickHandler);
            this.window.offBtn_ClickEvent += new EventHandler(OffBtnClickHandler);
            this.window.closeBtn_ClickEvent += new EventHandler(CloseBtnClickHandler);
            this.window.settingsWindow_FormClosingEvent += new FormClosingEventHandler(SettingsWindowFormClosing);
            this.window.settingsWindow_MouseDownEvent += new MouseEventHandler(SettingsWindow_MouseDownHandler);
            this.window.weightingEnabled_CheckedChangedEvent += new EventHandler(UpdateWeightingEnabled);
            this.window.loadBtn_ClickEvent += new EventHandler(LoadBtnClickHandler);
            this.window.saveBtn_ClickEvent += new EventHandler(SaveBtnClickHandler);
            this.window.decoratorSettingsBtn_ClickEvent += new EventHandler(ExtSettingsBtnClickHandler);
            this.window.processorSettingsBtn_ClickEvent += new EventHandler(ExtSettingsBtnClickHandler);
            this.window.driverSettingsBtn_ClickEvent += new EventHandler(ExtSettingsBtnClickHandler);
            this.window.decoratorComboBx_SelectedIndexChangedEvent += new EventHandler(DecoratorSelectedItemChangeHandler);
            this.window.compoundDecorationCheck_CheckedChangedEvent += new EventHandler(UpdateCompoundDecorationCheck);
            this.window.grabberSettingsBtn_ClickEvent += new EventHandler(ExtSettingsBtnClickHandler);
            this.window.resetBtn_ClickEvent += new EventHandler(ResetBtnClickHandler);
        }

        public void NewGlowCommandAvail(GlowCommand cmd)
        {
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(cmd);//pass it up
        }

        private void DecoratorSelectedItemChangeHandler(object sender, EventArgs args)
        {
            GlowDecorator dec = (GlowDecorator)this.window.decoratorComboBx.SelectedItem;
            if (dec != null)
                this.window.SetCurrentDecStatus(this.dev.GetDecOrNotfStatus(dec.id).ToString());
        }

        private void ExtSettingsBtnClickHandler(object sender, EventArgs args)
        {
            GlowExtension ext = (GlowExtension)sender;
            if (ext != null) {
                if (!ext.Settings())
                    this.settingsFactory.MakeAndShowWindow(ext.id);
            }
        }

        private void ResetBtnClickHandler(object sender, EventArgs args)
        {
            this.dev.Reset();
        }

        private void SaveBtnClickHandler(object sender, EventArgs args)
        {
            this.dev.SaveSettings();
        }

        private void LoadBtnClickHandler(object sender, EventArgs args)
        {
            this.dev.LoadSettings();
        }

        private void SettingsWindow_MouseDownHandler(object sender, MouseEventArgs args)
        {
            //allows dragging of forms to move them (because of hidden menu bars and window frame)
            if (args.Button == MouseButtons.Left) {//drag with left mouse btn
                ReleaseCapture();
                SendMessage(this.window.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void CloseBtnClickHandler(object sender, EventArgs args)
        {
            this.window.Close();
        }

        private void OffBtnClickHandler(object sender, EventArgs args)
        {
            NewGlowCommandAvailEvent(new SendColorCommand(this.dev.id, new Color16Bit(0,0,0)));
        }

        private void StopBtnClickHandler(object sender, EventArgs args)
        {
            SendStopCommand();
        }

        private void StartBtnClickHandler(object sender, EventArgs args)
        {
            if (this.dev.running)
                SendStopCommand();
            NewGlowCommandAvailEvent(new StartCommand(this.dev.id));
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
            if (actives != null) {
                if (actives.ActiveDriver != null)
                    this.window.UpdatedComboBoxSelectedExt(actives.ActiveDriver.id, this.window.driverComboBox);
                if (actives.ActiveGrabber != null)
                    this.window.UpdatedComboBoxSelectedExt(actives.ActiveGrabber.id, this.window.grabberComboBx);
                if (actives.ActiveProcessor != null)
                    this.window.UpdatedComboBoxSelectedExt(actives.ActiveProcessor.id, this.window.processorComboBx);
                List<GlowDecorator> decs = actives.ActiveDecorators;
                if (decs.Count != 0)
                    this.window.UpdatedComboBoxSelectedExt(decs.First<GlowDecorator>().id, this.window.decoratorComboBx);
                //TODO notifier
            }
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
                AdvancedSettingsWindow win = (AdvancedSettingsWindow)sender;
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
                //TODO
            }
        }

        private void UpdateDriverChoice(GlowDriver ext)
        {
            if (ext == null)
                return;
            SendStopCommand();
            this.dev.SetDvrGbbrOrPrcsrExt(ext.id);
            bool screenBased = (ext is GlowScreenDriverCoupler);
            this.window.UpdateIfDriverScreenBased(screenBased);
        }

        private void UpdateGrabberProcessorChoice(object sender, EventArgs args)
        {
            GlowExtension ext = GetExtFromSender(sender);
            if (ext == null)
                return;
            SendStopCommand();
            this.dev.SetDvrGbbrOrPrcsrExt(ext.id);
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
                if (HandleToggle(this.window.decoratorComboBx))
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
                if (!this.dev.GetExtSettingsWin(id))
                    this.settingsFactory.MakeAndShowWindow(id);
            }
            catch (Exception) {
                NewToolbarNotifAvailEvent(3000, "No Selected Extension",
                    "There is no extension to open the settings of.", 1);
            }
        }

        private void ApplyDriverRecomSettings(object sender, EventArgs a)
        {
            this.dev.ApplyDriverRecomSettings();
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            PollingAreaWindowController cont = new PollingAreaWindowController();
            cont.AttachObserver(this);
            cont.Show();
        }

        private void UpdatePollingSelectionsEvent(object sender, FormClosingEventArgs args)
        {
            Form form = (Form)sender;
            this.dev.settings.x = form.Bounds.X;
            this.dev.settings.y = form.Bounds.Y;
            this.dev.settings.width = form.Bounds.Width;
            this.dev.settings.height = form.Bounds.Height;
            UniqueColorGenerator.GetInstance().RetireUniqueColor(form.BackColor);
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
