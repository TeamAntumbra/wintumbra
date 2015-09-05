using System;
using System.Drawing;
using System.Windows.Forms;

namespace Antumbra.Glow.View {

    public partial class WhiteBalanceWindow : Form {

        #region Private Fields

        private int id;

        #endregion Private Fields

        #region Public Constructors

        public WhiteBalanceWindow(int id) {
            this.id = id;
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void ColorWheelChanged(Color newColor, int id);

        #endregion Public Delegates

        #region Public Events

        public event EventHandler closeBtn_ClickEvent;

        public event ColorWheelChanged ColorWheelChangedEvent;

        #endregion Public Events

        #region Public Methods

        public void SetColor(Color newColor) {
            this.colorWheel.HslColor = newColor;
        }

        #endregion Public Methods

        #region Private Methods

        private void closeBtn_Click(object sender, EventArgs e) {
            if(closeBtn_ClickEvent != null)
                closeBtn_ClickEvent(id, EventArgs.Empty);
        }

        private void colorWheel_ColorChanged(object sender, EventArgs e) {
            if(ColorWheelChangedEvent != null)
                ColorWheelChangedEvent(this.colorWheel.HslColor.ToRgbColor(), id);
        }

        #endregion Private Methods
    }
}
