using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antumbra.Glow.View {
    public partial class WhiteBalanceWindow : Form {
        public event EventHandler closeBtn_ClickEvent;
        public delegate void ColorWheelChanged(Color newColor, int id);
        public event ColorWheelChanged ColorWheelChangedEvent;
        private int id;
        public WhiteBalanceWindow(int id) {
            this.id = id;
            InitializeComponent();
        }

        public void SetColor(Color newColor) {
            this.colorWheel.HslColor = newColor;
        }

        private void closeBtn_Click(object sender, EventArgs e) {
            if(closeBtn_ClickEvent != null)
                closeBtn_ClickEvent(sender, e);
        }

        private void colorWheel_ColorChanged(object sender, EventArgs e) {
            if(ColorWheelChangedEvent != null)
                ColorWheelChangedEvent(this.colorWheel.HslColor.ToRgbColor(), id);
        }
    }
}
