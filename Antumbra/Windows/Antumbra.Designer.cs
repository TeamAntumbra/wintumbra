namespace Antumbra.Glow.Windows
{
    partial class Antumbra : MetroFramework.Forms.MetroForm
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
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HSVMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sinWaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomColorFadeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenResponsiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabel = new MetroFramework.Controls.MetroLabel();
            this.statusBtn = new System.Windows.Forms.Button();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.antumbraLabel = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.SuspendLayout();
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
            this.contextMenu.Size = new System.Drawing.Size(200, 220);
            this.contextMenu.Style = MetroFramework.MetroColorStyle.Blue;
            this.contextMenu.Text = "Antumbra|Glow";
            this.contextMenu.UseStyleColors = true;
            this.contextMenu.MouseLeave += new System.EventHandler(this.contextMenu_MouseLeave);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(199, 24);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(199, 24);
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // HSVMenuItem
            // 
            this.HSVMenuItem.Name = "HSVMenuItem";
            this.HSVMenuItem.Size = new System.Drawing.Size(199, 24);
            this.HSVMenuItem.Text = "HSV Sweep";
            this.HSVMenuItem.Click += new System.EventHandler(this.HSVMenuItem_Click);
            // 
            // sinWaveToolStripMenuItem
            // 
            this.sinWaveToolStripMenuItem.Name = "sinWaveToolStripMenuItem";
            this.sinWaveToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.sinWaveToolStripMenuItem.Text = "Sin Wave";
            this.sinWaveToolStripMenuItem.Click += new System.EventHandler(this.sinWaveToolStripMenuItem_Click);
            // 
            // randomColorFadeMenuItem
            // 
            this.randomColorFadeMenuItem.Name = "randomColorFadeMenuItem";
            this.randomColorFadeMenuItem.Size = new System.Drawing.Size(199, 24);
            this.randomColorFadeMenuItem.Text = "Random Color Fade";
            this.randomColorFadeMenuItem.Click += new System.EventHandler(this.randomColorFadeMenuItem_Click);
            // 
            // screenResponsiveMenuItem
            // 
            this.screenResponsiveMenuItem.Name = "screenResponsiveMenuItem";
            this.screenResponsiveMenuItem.Size = new System.Drawing.Size(199, 24);
            this.screenResponsiveMenuItem.Text = "Screen Responsive";
            this.screenResponsiveMenuItem.Click += new System.EventHandler(this.screenResponsiveMenuItem_Click);
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(199, 24);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // quitMenuItem
            // 
            this.quitMenuItem.Name = "quitMenuItem";
            this.quitMenuItem.Size = new System.Drawing.Size(199, 24);
            this.quitMenuItem.Text = "Quit";
            this.quitMenuItem.Click += new System.EventHandler(this.quitMenuItem_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.CausesValidation = false;
            this.statusLabel.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.statusLabel.ForeColor = System.Drawing.Color.White;
            this.statusLabel.Location = new System.Drawing.Point(64, 150);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(52, 20);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Status:";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.statusLabel.UseCustomBackColor = true;
            this.statusLabel.UseCustomForeColor = true;
            this.statusLabel.UseStyleColors = true;
            // 
            // statusBtn
            // 
            this.statusBtn.BackColor = System.Drawing.Color.Silver;
            this.statusBtn.CausesValidation = false;
            this.statusBtn.FlatAppearance.BorderSize = 0;
            this.statusBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.statusBtn.ForeColor = System.Drawing.Color.Transparent;
            this.statusBtn.Location = new System.Drawing.Point(138, 150);
            this.statusBtn.Name = "statusBtn";
            this.statusBtn.Size = new System.Drawing.Size(25, 18);
            this.statusBtn.TabIndex = 4;
            this.statusBtn.UseVisualStyleBackColor = false;
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Magenta;
            this.metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // antumbraLabel
            // 
            this.antumbraLabel.CausesValidation = false;
            this.antumbraLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.antumbraLabel.Font = new System.Drawing.Font("Calibri", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.antumbraLabel.ForeColor = System.Drawing.Color.White;
            this.antumbraLabel.Location = new System.Drawing.Point(1, 34);
            this.antumbraLabel.Name = "antumbraLabel";
            this.antumbraLabel.Size = new System.Drawing.Size(273, 70);
            this.antumbraLabel.TabIndex = 5;
            this.antumbraLabel.Text = "ANTUMBRA";
            this.antumbraLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Antumbra
            // 
            this.ClientSize = new System.Drawing.Size(275, 333);
            this.Controls.Add(this.antumbraLabel);
            this.Controls.Add(this.statusBtn);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Verdana", 56F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Antumbra";
            this.Opacity = 0.85D;
            this.Resizable = false;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.Antumbra_Resize);
            this.contextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HSVMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomColorFadeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenResponsiveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitMenuItem;
        private MetroFramework.Controls.MetroContextMenu contextMenu;
        private System.Windows.Forms.ToolStripMenuItem sinWaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private MetroFramework.Controls.MetroLabel statusLabel;
        private System.Windows.Forms.Button statusBtn;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private System.Windows.Forms.Label antumbraLabel;
    }
}

