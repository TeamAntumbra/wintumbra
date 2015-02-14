using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antumbra.Glow.Settings
{
    public partial class pollingAreaSetter : Form
    {
        private DeviceSettings settingsObj;
        public pollingAreaSetter(DeviceSettings settingsObj, Color back)
        {
            this.settingsObj = settingsObj;
            InitializeComponent();
            this.BackColor = back;
        }

        private void pollingAreaSetter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.settingsObj.x = this.Location.X;
            this.settingsObj.y = this.Location.Y;
            this.settingsObj.height = this.Height;
            this.settingsObj.width = this.Width;
        }
    }
}
