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

namespace Antumbra.Glow.Windows
{
    public partial class pollingAreaSetter : MetroFramework.Forms.MetroForm
    {
        private AntumbraCore core;
        private SettingsWindow settings;
        public pollingAreaSetter(AntumbraCore core, SettingsWindow settings)
        {
            this.settings = settings;
            this.core = core;
            InitializeComponent();
        }

        private void pollingAreaSetter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.core.pollingX = this.Location.X;
            this.core.pollingY = this.Location.Y;
            this.core.pollingHeight = this.Height;
            this.core.pollingWidth = this.Width;
            this.settings.updateValues();
        }
    }
}
