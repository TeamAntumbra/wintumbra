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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
