using Antumbra.Glow.ExtensionFramework;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Antumbra.Glow.View {

    public partial class AntumbraExtSettingsWindow : Form {

        #region Public Fields

        public const int HT_CAPTION = 0x2;

        /// <summary>
        /// Move form dependencies
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        #endregion Public Fields

        #region Private Fields

        private GlowExtension ext;

        #endregion Private Fields

        #region Public Constructors

        public AntumbraExtSettingsWindow(GlowExtension ext) {
            this.ext = ext;
            InitializeComponent();
            ExtName.Text = ext.Name;
            Author.Text = ext.Author;
            Version.Text = ext.Version.ToString();
            Description.Text = ext.Description;
            Show();
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
            Close();
        }

        #endregion Private Methods
    }
}
