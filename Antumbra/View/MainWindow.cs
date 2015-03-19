using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antumbra.Glow.View
{
    public partial class MainWindow : Form
    {
        public event EventHandler closeBtn_ClickEvent;
        public event EventHandler colorWheel_ColorChangedEvent;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (closeBtn_ClickEvent != null)
                closeBtn_ClickEvent(sender, e);
        }

        private void colorWheel_ColorChanged(object sender, EventArgs e)
        {
            if (colorWheel_ColorChangedEvent != null)
                colorWheel_ColorChangedEvent(sender, e);
        }
    }
}
