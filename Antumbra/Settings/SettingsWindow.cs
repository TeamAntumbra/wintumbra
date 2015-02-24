using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow;
using Antumbra.Glow.ExtensionFramework;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Antumbra.Glow.Connector;

namespace Antumbra.Glow.Settings
{
    public partial class SettingsWindow : Form
    {
        private Color[] PollingWindowColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Pink, Color.Purple, Color.Orange, Color.White };
        private AntumbraCore antumbra;
        /// <summary>
        /// GlowDevice object for the device whose settings are being rendered currently
        /// </summary>
        public GlowDevice currentDevice { get; private set; }
        /// <summary>
        /// Form used to set the screen grabber polling area
        /// </summary>
        private Form pollingAreaWindow;
        private ExtensionLibrary library;
        /// <summary>
        /// Move form dependencies
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public SettingsWindow(GlowDevice device, ExtensionLibrary library, AntumbraCore core)
        {
            this.antumbra = core;
            this.library = library;
            this.currentDevice = device;
            InitializeComponent();
            PopulateExtTable();
            //updateValues();
            this.Focus();
        }

        private void PopulateExtTable()
        {
            foreach (var dvr in this.library.AvailDrivers)
                GenerateRowFromExt(dvr);
            foreach (var gbr in this.library.AvailGrabbers)
                GenerateRowFromExt(gbr);
            foreach (var pcr in this.library.AvailProcessors)
                GenerateRowFromExt(pcr);
            foreach (var dec in this.library.AvailDecorators)
                GenerateRowFromExt(dec);
            foreach (var notf in this.library.AvailNotifiers)
                GenerateRowFromExt(notf);
        }

        private DataGridViewRow GenerateRowFromExt(GlowExtension ext)
        {
            int i = extTable.Rows.Add();
            DataGridViewRow row = extTable.Rows[i];
            row.Cells["NameCol"].Value = ext.Name;
            row.Cells["EnabledCol"].Value = ext.IsDefault;
            DataGridViewCheckBoxCell check = (DataGridViewCheckBoxCell)row.Cells["EnabledCol"];
            row.Cells["DescCol"].Value = ext.Description;
            row.Cells["VersionCol"].Value = ext.Version.ToString();
            row.Cells["AuthorCol"].Value = ext.Author;
            row.Cells["idCol"].Value = ext.id;
            DataGridViewImageCell settings = (DataGridViewImageCell)row.Cells["SettingsCol"];
            Bitmap littleGear = new Bitmap(64,64);
            using (Graphics g = Graphics.FromImage(littleGear)) {
                g.DrawImage(global::Antumbra.Glow.Properties.Resources.gear, 0, 0, littleGear.Width, littleGear.Height);
            }
            settings.Value = littleGear;
            return row;
        }

        public void CleanUp()
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// Update the settings window form to reflect the settings found in the GlowDevice settings object
        /// </summary>
        public void updateValues()
        {
            this.versionLabel.Text = this.antumbra.ProductVersion;
            compoundDecorationCheck.Checked = this.currentDevice.settings.compoundDecoration;
            newColorWeight.Text = (this.currentDevice.settings.newColorWeight * 100).ToString();
            weightingEnabled.Checked = this.currentDevice.settings.weightingEnabled;
            sleepSize.Text = this.currentDevice.settings.stepSleep.ToString();
            pollingHeight.Text = this.currentDevice.settings.height.ToString();
            pollingWidth.Text = this.currentDevice.settings.width.ToString();
            pollingX.Text = this.currentDevice.settings.x.ToString();
            pollingY.Text = this.currentDevice.settings.y.ToString();
            glowStatus.Text = GetStatusString(this.currentDevice.status);
            deviceName.Text = this.currentDevice.id.ToString();
            //currentSetup.Text = this.currentDevice.GetSetupDesc();
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
        /// <summary>
        /// Update stepSleepSize for current device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sleepSize_TextChanged(object sender, EventArgs e)
        {
            try {
                this.currentDevice.settings.stepSleep = Convert.ToInt32(sleepSize.Text);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void SettingsWindow_MouseEnter(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null || !this.pollingAreaWindow.Visible)
                this.Focus();
        }

        private void pollingArea_Click(object sender, EventArgs e)
        {
            if (this.pollingAreaWindow == null || this.pollingAreaWindow.IsDisposed) {
                var current = this.currentDevice.id;
                var back = PollingWindowColors[current % 8];
                this.pollingAreaWindow = new pollingAreaSetter(this.currentDevice.settings, back);
                this.antumbra.Stop(current);
                this.antumbra.SendColor(current, back);
                this.pollingAreaWindow.FormClosing += new FormClosingEventHandler(UpdatePollingSelectionsEvent);
            }
            this.pollingAreaWindow.Show();
        }

        private void UpdatePollingSelectionsEvent(object sender, EventArgs args)
        {
            this.updateValues();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Start(this.currentDevice.id);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Stop(this.currentDevice.id);
        }

        private void offBtn_Click(object sender, EventArgs e)
        {
            this.antumbra.Off();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsWindow_MouseDown(object sender, MouseEventArgs e)
        {
            // Drag form to move
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void newColorWeight_TextChanged(object sender, EventArgs e)
        {
            try {
                int percent = Convert.ToInt16(newColorWeight.Text);
                if (percent >= 0 && percent <= 100)//valid
                    this.currentDevice.settings.newColorWeight = Convert.ToDouble(percent / 100.0);
            }
            catch (System.FormatException) {
                Console.WriteLine("Format exception in settings");
            }
        }

        private void weightingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.currentDevice.settings.weightingEnabled = weightingEnabled.Checked;
        }

        private void driverRecBtn_Click(object sender, EventArgs e)
        {
            this.currentDevice.ActiveDriver.RecmmndCoreSettings();
            this.currentDevice.settings.stepSleep = this.currentDevice.ActiveDriver.stepSleep;
            updateValues();
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void compoundDecorationCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.currentDevice.settings.compoundDecoration = compoundDecorationCheck.Checked;
        }

        private void AttemptToOpenSettingsWindow(GlowExtension ext)
        {
            if (ext == null) {
                this.antumbra.ShowMessage(3000, "No Selected Extension",
                    "There is no extension to open the settings of.",
                    ToolTipIcon.Warning);
                return;
            }
            if (!ext.Settings()) {
                var win = new AntumbraExtSettingsWindow(ext);
                win.Show();
            }
                
        }

        private void extTable_CellContentClick(object sender, DataGridViewCellEventArgs e)//handle checkbox clicks
        {
            DataGridViewRow row = extTable.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells[6].Value);
            GlowExtension ext = this.library.findExt(id);
            switch (e.ColumnIndex) {
                case 0://checkbox col
                    if (this.currentDevice.isRunning())
                        this.antumbra.Stop(this.currentDevice.id);
                    if (ext is GlowDecorator) {
                        GlowDecorator dec = (GlowDecorator)ext;
                        GetRowByDeviceId(dec.id).Cells[0].Value = !this.currentDevice.RemoveDecOrAddIfNew(dec);
                    }
                    else if (ext is GlowNotifier) {
                        GlowNotifier notf = (GlowNotifier)ext;
                        GetRowByDeviceId(notf.id).Cells[0].Value = !this.currentDevice.RemoveNotfOrAddIfNew(notf);
                    }
                    if (Convert.ToBoolean(row.Cells[0].EditedFormattedValue)) {//now checked
                        if (this.currentDevice.ActiveDriver.id == id) { }//already selected
                        else {//a different driver is selected
                            if (ext is GlowDriver) {
                                DataGridViewRow current = GetRowByDeviceId(this.currentDevice.ActiveDriver.id);
                                current.Cells[0].Value = false;//uncheck
                                this.currentDevice.ActiveDriver = (GlowDriver)ext;
                            }
                            else if (ext is GlowScreenGrabber) {
                                DataGridViewRow current = GetRowByDeviceId(this.currentDevice.ActiveGrabber.id);
                                current.Cells[0].Value = false;//uncheck
                                this.currentDevice.ActiveGrabber = (GlowScreenGrabber)ext;
                            }
                            else if (ext is GlowScreenProcessor) {
                                DataGridViewRow current = GetRowByDeviceId(this.currentDevice.ActiveProcessor.id);
                                current.Cells[0].Value = false;//uncheck
                                this.currentDevice.ActiveProcessor = (GlowScreenProcessor)ext;
                            }
                        }
                    }
                    else {//now unchecked

                    }
                    break;
                case 4://settings button
                    AttemptToOpenSettingsWindow(ext);
                    break;
            }
        }

        private DataGridViewRow GetRowByDeviceId(int id)
        {
            foreach (DataGridViewRow r in this.extTable.Rows)
                if (Convert.ToInt32(r.Cells[6].Value) == id)
                    return r;
            return null;
        }
    }
}
