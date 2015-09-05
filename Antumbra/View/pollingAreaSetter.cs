using System;
using System.Drawing;
using System.Windows.Forms;

namespace Antumbra.Glow.View {

    public partial class pollingAreaSetter : Form {

        #region Public Constructors

        public pollingAreaSetter(Color BackColor, int id) {
            InitializeComponent();
            this.BackColor = BackColor;
            this.id = id;
            this.Refresh();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler formClosingEvent;

        #endregion Public Events

        #region Public Properties

        public int id { get; private set; }

        #endregion Public Properties

        #region Private Methods

        private void pollingAreaSetter_FormClosing(object sender, FormClosingEventArgs e) {
            if(formClosingEvent != null) {
                formClosingEvent(sender, e);
            }
        }

        #endregion Private Methods
    }
}
