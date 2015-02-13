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
        public delegate void SetToColor(Color newColor, int id);
        public event SetToColor SetToColorEvent;
        private DeviceSettings settingsObj;
        private Color[] WindowColors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Pink, Color.Purple, Color.Orange, Color.White };
        public pollingAreaSetter(DeviceSettings settingsObj)
        {
            this.settingsObj = settingsObj;
            InitializeComponent();
        }

       /* public override void AttachEvent(SetToColorObserver observer)
        {
            this.SetToColorEvent += new SetToColor(observer.SetToColor);
        }*/

        private void pollingAreaSetter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.settingsObj.x = this.Location.X;
            this.settingsObj.y = this.Location.Y;
            this.settingsObj.height = this.Height;
            this.settingsObj.width = this.Width;
        }

        private void setToUniqueColorBtn_Click(object sender, EventArgs args)
        {
            this.BackColor = GetUniqueColor(this.settingsObj.id);
        }

        private Color GetUniqueColor(int id)
        {
            int index = id % 8;
            return WindowColors[index];
        }
    }
}
