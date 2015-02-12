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

namespace Antumbra.Glow.Settings
{
    public partial class pollingAreaSetter : Form
    {
        //private AntumbraCore core;
        private DeviceSettings settingsObj;
        private SettingsWindow settings;
        public pollingAreaSetter(DeviceSettings settingsObj, SettingsWindow settings)
        {
            this.settings = settings;
            this.settingsObj = settingsObj;
            //this.core = core;
            InitializeComponent();
        }

        private void pollingAreaSetter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.settingsObj.x = this.Location.X;
            this.settingsObj.y = this.Location.Y;
            this.settingsObj.height = this.Height;
            this.settingsObj.width = this.Width;
            this.settings.updateValues();
        }

        private void nextDevBtn_Click(object sender, EventArgs e)
        {
            //TODO cycle to next devices settings and change polling window color to match color output by current device (all others set to black)
        }
    }
}
