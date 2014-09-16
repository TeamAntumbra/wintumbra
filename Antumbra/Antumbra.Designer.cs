namespace Antumbra
{
    partial class Antumbra
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Antumbra));
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.AntumbraLabel = new System.Windows.Forms.Label();
            this.colorChoose = new System.Windows.Forms.ColorDialog();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HSVMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sinWaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomColorFadeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenResponsiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusBtn = new System.Windows.Forms.Button();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(2, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(100, 23);
            this.linkLabel1.TabIndex = 0;
            // 
            // AntumbraLabel
            // 
            this.AntumbraLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AntumbraLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AntumbraLabel.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
            this.AntumbraLabel.Location = new System.Drawing.Point(45, 32);
            this.AntumbraLabel.Name = "AntumbraLabel";
            this.AntumbraLabel.Size = new System.Drawing.Size(198, 45);
            this.AntumbraLabel.TabIndex = 2;
            this.AntumbraLabel.Text = "Antumbra";
            this.AntumbraLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.AntumbraLabel.UseMnemonic = false;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Click for menu";
            this.notifyIcon.BalloonTipTitle = "Antumbra|Glow";
            this.notifyIcon.ContextMenuStrip = this.contextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Antumbra|Glow";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.settingsMenuItem,
            this.HSVMenuItem,
            this.sinWaveToolStripMenuItem,
            this.randomColorFadeMenuItem,
            this.screenResponsiveMenuItem,
            this.manualToolStripMenuItem,
            this.offToolStripMenuItem,
            this.quitMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.ShowCheckMargin = true;
            this.contextMenu.ShowImageMargin = false;
            this.contextMenu.ShowItemToolTips = false;
            this.contextMenu.Size = new System.Drawing.Size(244, 274);
            this.contextMenu.Text = "Antumbra|Glow";
            this.contextMenu.MouseLeave += new System.EventHandler(this.contextMenu_MouseLeave);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(243, 30);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(243, 30);
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // HSVMenuItem
            // 
            this.HSVMenuItem.Name = "HSVMenuItem";
            this.HSVMenuItem.Size = new System.Drawing.Size(243, 30);
            this.HSVMenuItem.Text = "HSV Sweep";
            this.HSVMenuItem.Click += new System.EventHandler(this.HSVMenuItem_Click);
            // 
            // sinWaveToolStripMenuItem
            // 
            this.sinWaveToolStripMenuItem.Name = "sinWaveToolStripMenuItem";
            this.sinWaveToolStripMenuItem.Size = new System.Drawing.Size(243, 30);
            this.sinWaveToolStripMenuItem.Text = "Sin Wave";
            this.sinWaveToolStripMenuItem.Click += new System.EventHandler(this.sinWaveToolStripMenuItem_Click);
            // 
            // randomColorFadeMenuItem
            // 
            this.randomColorFadeMenuItem.Name = "randomColorFadeMenuItem";
            this.randomColorFadeMenuItem.Size = new System.Drawing.Size(243, 30);
            this.randomColorFadeMenuItem.Text = "Random Color Fade";
            this.randomColorFadeMenuItem.Click += new System.EventHandler(this.randomColorFadeMenuItem_Click);
            // 
            // screenResponsiveMenuItem
            // 
            this.screenResponsiveMenuItem.Name = "screenResponsiveMenuItem";
            this.screenResponsiveMenuItem.Size = new System.Drawing.Size(243, 30);
            this.screenResponsiveMenuItem.Text = "Screen Responsive";
            this.screenResponsiveMenuItem.Click += new System.EventHandler(this.screenResponsiveMenuItem_Click);
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(243, 30);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(243, 30);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // quitMenuItem
            // 
            this.quitMenuItem.Name = "quitMenuItem";
            this.quitMenuItem.Size = new System.Drawing.Size(243, 30);
            this.quitMenuItem.Text = "Quit";
            this.quitMenuItem.Click += new System.EventHandler(this.quitMenuItem_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.statusLabel.Location = new System.Drawing.Point(69, 145);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(62, 20);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Status:";
            // 
            // statusBtn
            // 
            this.statusBtn.CausesValidation = false;
            this.statusBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.statusBtn.Location = new System.Drawing.Point(138, 145);
            this.statusBtn.Name = "statusBtn";
            this.statusBtn.Size = new System.Drawing.Size(23, 23);
            this.statusBtn.TabIndex = 4;
            this.statusBtn.UseVisualStyleBackColor = true;
            // 
            // Antumbra
            // 
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(298, 368);
            this.Controls.Add(this.statusBtn);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.AntumbraLabel);
            this.Controls.Add(this.linkLabel1);
            this.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Antumbra";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Antumbra";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.Antumbra_Resize);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label AntumbraLabel;
        private System.Windows.Forms.ColorDialog colorChoose;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HSVMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomColorFadeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenResponsiveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem sinWaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button statusBtn;
    }
}

