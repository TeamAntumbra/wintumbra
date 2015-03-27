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
    public partial class WhiteBalanceWindow : Form
    {
        public event EventHandler closeBtn_ClickEvent;
        public event EventHandler rUpBtn_ClickEvent;
        public event EventHandler gUpBtn_ClickEvent;
        public event EventHandler bUpBtn_clickEvent;
        public event EventHandler rDownBtn_ClickEvent;
        public event EventHandler gDownBtn_ClickEvent;
        public event EventHandler bDownBtn_ClickEvent;
        public WhiteBalanceWindow()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (closeBtn_ClickEvent != null)
                closeBtn_ClickEvent(sender, e);
        }

        private void rUpBtn_Click(object sender, EventArgs e)
        {
            if (rUpBtn_ClickEvent != null)
                rUpBtn_ClickEvent(sender, e);
        }

        private void gUpBtn_Click(object sender, EventArgs e)
        {
            if (gUpBtn_ClickEvent != null)
                gUpBtn_ClickEvent(sender, e);
        }

        private void bUpBtn_Click(object sender, EventArgs e)
        {
            if (bUpBtn_clickEvent != null)
                bUpBtn_clickEvent(sender, e);
        }

        private void rDownBtn_Click(object sender, EventArgs e)
        {
            if (rDownBtn_ClickEvent != null)
                rDownBtn_ClickEvent(sender, e);
        }

        private void gDownBtn_Click(object sender, EventArgs e)
        {
            if (gDownBtn_ClickEvent != null)
                gDownBtn_ClickEvent(sender, e);
        }

        private void bDownBtn_Click(object sender, EventArgs e)
        {
            if (bDownBtn_ClickEvent != null)
                bDownBtn_ClickEvent(sender, e);
        }
    }
}
