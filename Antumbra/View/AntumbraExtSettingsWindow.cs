﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Management;

namespace Antumbra.Glow.View
{
    public partial class AntumbraExtSettingsWindow : Form
    {
        private GlowExtension ext;
        /// <summary>
        /// Move form dependencies
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private AntumbraExtSettingsWindow(GlowExtension ext)
        {
            this.ext = ext;
            InitializeComponent();
            this.ExtName.Text = ext.Name;
            this.Author.Text = ext.Author;
            this.Version.Text = ext.Version.ToString();
            this.Description.Text = ext.Description;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AntumbraExtSettingsWindow_MouseDown(object sender, MouseEventArgs e)
        {
            // Drag form to move
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public class ExtWindowFactory {
            private ExtensionLibrary lib;
            public ExtWindowFactory(ExtensionLibrary lib)
            {
                this.lib = lib;
            }

            public AntumbraExtSettingsWindow MakeAndShowWindow(Guid id)
            {
                GlowExtension ext = this.lib.findExt(id);
                AntumbraExtSettingsWindow win = new AntumbraExtSettingsWindow(ext);
                win.Show();
                return win;
            }
        }
    }
}