namespace Antumbra.Glow
{
    partial class AntumbraCore
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
            //base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AntumbraCore));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.whatsMyConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.currentOutRate = new System.Windows.Forms.ToolStripMenuItem();
            this.startAllItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopAllItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDeviceList = new System.Windows.Forms.ToolStripComboBox();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
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
            this.contextMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whatsMyConfig,
            this.currentOutRate,
            this.startAllItem,
            this.stopAllItem,
            this.toolStripDeviceList,
            this.settingsMenuItem,
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.offToolStripMenuItem,
            this.quitMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenu.ShowImageMargin = false;
            this.contextMenu.ShowItemToolTips = false;
            this.contextMenu.Size = new System.Drawing.Size(256, 271);
            this.contextMenu.Text = "Antumbra|Glow";
            // 
            // whatsMyConfig
            // 
            this.whatsMyConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.whatsMyConfig.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.whatsMyConfig.Name = "whatsMyConfig";
            this.whatsMyConfig.Size = new System.Drawing.Size(255, 26);
            this.whatsMyConfig.Text = "What is Currently Enabled?";
            this.whatsMyConfig.Click += new System.EventHandler(this.whatsMyConfig_Click);
            // 
            // currentOutRate
            // 
            this.currentOutRate.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.currentOutRate.Name = "currentOutRate";
            this.currentOutRate.Size = new System.Drawing.Size(255, 26);
            this.currentOutRate.Text = "Current Output Rate?";
            this.currentOutRate.Click += new System.EventHandler(this.currentOutRateItem_Click);
            // 
            // startAllItem
            // 
            this.startAllItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.startAllItem.Name = "startAllItem";
            this.startAllItem.Size = new System.Drawing.Size(255, 26);
            this.startAllItem.Text = "Start All Glows";
            this.startAllItem.Click += new System.EventHandler(this.startAllItem_Click);
            // 
            // stopAllItem
            // 
            this.stopAllItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stopAllItem.Name = "stopAllItem";
            this.stopAllItem.Size = new System.Drawing.Size(255, 26);
            this.stopAllItem.Text = "Stop All Glows";
            this.stopAllItem.Click += new System.EventHandler(this.stopAllItem_Click);
            // 
            // toolStripDeviceList
            // 
            this.toolStripDeviceList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.toolStripDeviceList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toolStripDeviceList.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripDeviceList.Name = "toolStripDeviceList";
            this.toolStripDeviceList.Size = new System.Drawing.Size(121, 29);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.settingsMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(255, 26);
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.offToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // quitMenuItem
            // 
            this.quitMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.quitMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.quitMenuItem.Name = "quitMenuItem";
            this.quitMenuItem.Size = new System.Drawing.Size(255, 26);
            this.quitMenuItem.Text = "Quit";
            this.quitMenuItem.Click += new System.EventHandler(this.quitMenuItem_Click);
            // 
            // AntumbraCore
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(5, 5);
            this.ControlBox = false;
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AntumbraCore";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem offToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whatsMyConfig;
        public System.Windows.Forms.ToolStripComboBox toolStripDeviceList;
        private System.Windows.Forms.ToolStripMenuItem currentOutRate;
        private System.Windows.Forms.ToolStripMenuItem startAllItem;
        private System.Windows.Forms.ToolStripMenuItem stopAllItem;
    }
}

