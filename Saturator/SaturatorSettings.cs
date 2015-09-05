using Antumbra.Glow.ExtensionFramework.Types;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Saturator {

    public partial class SaturatorSettings : Form {

        #region Public Fields

        public const int HT_CAPTION = 0x2;

        /// <summary>
        /// Move form dependencies
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        #endregion Public Fields

        #region Private Fields

        private GlowFilter ext;

        #endregion Private Fields

        #region Public Constructors

        public SaturatorSettings(GlowFilter ext) {
            this.ext = ext;
            InitializeComponent();
            this.ExtName.Text = ext.Name;
            this.Author.Text = ext.Author;
            this.Version.Text = ext.Version.ToString();
            this.Description.Text = ext.Description;
        }

        #endregion Public Constructors

        #region Public Methods

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        #endregion Public Methods

        #region Private Methods

        private void AntumbraExtSettingsWindow_MouseDown(object sender, MouseEventArgs e) {
            // Drag form to move
            if(e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e) {
            this.Close();
        }

        #endregion Private Methods
    }
}
