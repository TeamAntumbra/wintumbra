using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.Settings;

namespace Antumbra.Glow.View
{
    public partial class pollingAreaSetter : Form
    {
        public event EventHandler formClosingEvent;
        public pollingAreaSetter(Color back)
        {
            InitializeComponent();
            this.BackColor = back;
        }

        private void pollingAreaSetter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formClosingEvent != null)
                formClosingEvent(sender, EventArgs.Empty);//replace with event args as to not have to use FormClosingEventHandler
        }
    }
}
