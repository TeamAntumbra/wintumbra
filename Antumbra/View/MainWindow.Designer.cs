namespace Antumbra.Glow.View
{
    partial class MainWindow
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
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.closeBtn = new System.Windows.Forms.Button();
            this.flatTabControl = new FlatTabControl.FlatTabControl();
            this.manualTab = new System.Windows.Forms.TabPage();
            this.quitBtn = new System.Windows.Forms.Button();
            this.brightnessLabel = new System.Windows.Forms.Label();
            this.brightnessTrackBar = new System.Windows.Forms.TrackBar();
            this.offBtn = new System.Windows.Forms.RadioButton();
            this.onBtn = new System.Windows.Forms.RadioButton();
            this.mirrorTab = new System.Windows.Forms.TabPage();
            this.modeDescs = new System.Windows.Forms.Label();
            this.gameBtn = new System.Windows.Forms.Button();
            this.smoothBtn = new System.Windows.Forms.Button();
            this.augmentBtn = new System.Windows.Forms.Button();
            this.mirrorBtn = new System.Windows.Forms.Button();
            this.fadeTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.neonBtn = new System.Windows.Forms.Button();
            this.sinBtn = new System.Windows.Forms.Button();
            this.hsvBtn = new System.Windows.Forms.Button();
            this.customTab = new System.Windows.Forms.TabPage();
            this.customConfigBtn = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.antumbraLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.colorWheel = new Antumbra.Glow.View.CyotekColorWheel.ColorWheel();
            this.setPollingSizeBtn = new System.Windows.Forms.Button();
            this.flatTabControl.SuspendLayout();
            this.manualTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessTrackBar)).BeginInit();
            this.mirrorTab.SuspendLayout();
            this.fadeTab.SuspendLayout();
            this.customTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // closeBtn
            // 
            this.closeBtn.AutoSize = true;
            this.closeBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.closeBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.closeBtn.Location = new System.Drawing.Point(520, -2);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(33, 34);
            this.closeBtn.TabIndex = 73;
            this.closeBtn.Text = "X";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // flatTabControl
            // 
            this.flatTabControl.Controls.Add(this.manualTab);
            this.flatTabControl.Controls.Add(this.mirrorTab);
            this.flatTabControl.Controls.Add(this.fadeTab);
            this.flatTabControl.Controls.Add(this.customTab);
            this.flatTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.flatTabControl.Location = new System.Drawing.Point(-1, 51);
            this.flatTabControl.myBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.flatTabControl.Name = "flatTabControl";
            this.flatTabControl.SelectedIndex = 0;
            this.flatTabControl.Size = new System.Drawing.Size(577, 284);
            this.flatTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.flatTabControl.TabIndex = 74;
            // 
            // manualTab
            // 
            this.manualTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.manualTab.Controls.Add(this.brightnessLabel);
            this.manualTab.Controls.Add(this.brightnessTrackBar);
            this.manualTab.Controls.Add(this.offBtn);
            this.manualTab.Controls.Add(this.onBtn);
            this.manualTab.Controls.Add(this.colorWheel);
            this.manualTab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.manualTab.Location = new System.Drawing.Point(4, 25);
            this.manualTab.Name = "manualTab";
            this.manualTab.Padding = new System.Windows.Forms.Padding(3);
            this.manualTab.Size = new System.Drawing.Size(569, 255);
            this.manualTab.TabIndex = 0;
            this.manualTab.Text = "Manual";
            // 
            // quitBtn
            // 
            this.quitBtn.AutoSize = true;
            this.quitBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.quitBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.quitBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.quitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quitBtn.Font = new System.Drawing.Font("Lucida Sans Unicode", 8F);
            this.quitBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.quitBtn.Location = new System.Drawing.Point(460, -2);
            this.quitBtn.Name = "quitBtn";
            this.quitBtn.Size = new System.Drawing.Size(54, 32);
            this.quitBtn.TabIndex = 77;
            this.quitBtn.Text = "Quit";
            this.quitBtn.UseVisualStyleBackColor = false;
            this.quitBtn.Click += new System.EventHandler(this.quitBtn_Click);
            // 
            // brightnessLabel
            // 
            this.brightnessLabel.AutoSize = true;
            this.brightnessLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.brightnessLabel.Location = new System.Drawing.Point(358, 82);
            this.brightnessLabel.Name = "brightnessLabel";
            this.brightnessLabel.Size = new System.Drawing.Size(127, 29);
            this.brightnessLabel.TabIndex = 4;
            this.brightnessLabel.Text = "Brightness";
            // 
            // brightnessTrackBar
            // 
            this.brightnessTrackBar.Location = new System.Drawing.Point(273, 44);
            this.brightnessTrackBar.Name = "brightnessTrackBar";
            this.brightnessTrackBar.Size = new System.Drawing.Size(244, 69);
            this.brightnessTrackBar.TabIndex = 3;
            this.brightnessTrackBar.Scroll += new System.EventHandler(this.brightnessTrackBar_Scroll);
            // 
            // offBtn
            // 
            this.offBtn.AutoSize = true;
            this.offBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.offBtn.Location = new System.Drawing.Point(464, 214);
            this.offBtn.Name = "offBtn";
            this.offBtn.Size = new System.Drawing.Size(56, 28);
            this.offBtn.TabIndex = 2;
            this.offBtn.TabStop = true;
            this.offBtn.Text = "Off";
            this.offBtn.UseVisualStyleBackColor = true;
            // 
            // onBtn
            // 
            this.onBtn.AutoSize = true;
            this.onBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.onBtn.Location = new System.Drawing.Point(406, 214);
            this.onBtn.Name = "onBtn";
            this.onBtn.Size = new System.Drawing.Size(59, 28);
            this.onBtn.TabIndex = 1;
            this.onBtn.TabStop = true;
            this.onBtn.Text = "On";
            this.onBtn.UseVisualStyleBackColor = true;
            this.onBtn.CheckedChanged += new System.EventHandler(this.onBtn_CheckedChanged);
            // 
            // mirrorTab
            // 
            this.mirrorTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.mirrorTab.Controls.Add(this.setPollingSizeBtn);
            this.mirrorTab.Controls.Add(this.modeDescs);
            this.mirrorTab.Controls.Add(this.gameBtn);
            this.mirrorTab.Controls.Add(this.smoothBtn);
            this.mirrorTab.Controls.Add(this.augmentBtn);
            this.mirrorTab.Controls.Add(this.mirrorBtn);
            this.mirrorTab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.mirrorTab.Location = new System.Drawing.Point(4, 25);
            this.mirrorTab.Name = "mirrorTab";
            this.mirrorTab.Padding = new System.Windows.Forms.Padding(3);
            this.mirrorTab.Size = new System.Drawing.Size(569, 255);
            this.mirrorTab.TabIndex = 1;
            this.mirrorTab.Text = "Mirror";
            // 
            // modeDescs
            // 
            this.modeDescs.Location = new System.Drawing.Point(6, 107);
            this.modeDescs.Name = "modeDescs";
            this.modeDescs.Size = new System.Drawing.Size(531, 127);
            this.modeDescs.TabIndex = 4;
            this.modeDescs.Text = resources.GetString("modeDescs.Text");
            // 
            // gameBtn
            // 
            this.gameBtn.AutoSize = true;
            this.gameBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gameBtn.Location = new System.Drawing.Point(417, 44);
            this.gameBtn.Name = "gameBtn";
            this.gameBtn.Size = new System.Drawing.Size(75, 36);
            this.gameBtn.TabIndex = 3;
            this.gameBtn.Text = "Game";
            this.gameBtn.UseVisualStyleBackColor = true;
            this.gameBtn.Click += new System.EventHandler(this.gameBtn_Click);
            // 
            // smoothBtn
            // 
            this.smoothBtn.AutoSize = true;
            this.smoothBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.smoothBtn.Location = new System.Drawing.Point(301, 44);
            this.smoothBtn.Name = "smoothBtn";
            this.smoothBtn.Size = new System.Drawing.Size(87, 36);
            this.smoothBtn.TabIndex = 2;
            this.smoothBtn.Text = "Smooth";
            this.smoothBtn.UseVisualStyleBackColor = true;
            this.smoothBtn.Click += new System.EventHandler(this.smoothBtn_Click);
            // 
            // augmentBtn
            // 
            this.augmentBtn.AutoSize = true;
            this.augmentBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.augmentBtn.Location = new System.Drawing.Point(168, 44);
            this.augmentBtn.Name = "augmentBtn";
            this.augmentBtn.Size = new System.Drawing.Size(99, 36);
            this.augmentBtn.TabIndex = 1;
            this.augmentBtn.Text = "Augment";
            this.augmentBtn.UseVisualStyleBackColor = true;
            this.augmentBtn.Click += new System.EventHandler(this.augmentBtn_Click);
            // 
            // mirrorBtn
            // 
            this.mirrorBtn.AutoSize = true;
            this.mirrorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mirrorBtn.Location = new System.Drawing.Point(57, 44);
            this.mirrorBtn.Name = "mirrorBtn";
            this.mirrorBtn.Size = new System.Drawing.Size(75, 36);
            this.mirrorBtn.TabIndex = 0;
            this.mirrorBtn.Text = "Mirror";
            this.mirrorBtn.UseVisualStyleBackColor = true;
            this.mirrorBtn.Click += new System.EventHandler(this.mirrorBtn_Click);
            // 
            // fadeTab
            // 
            this.fadeTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.fadeTab.Controls.Add(this.label1);
            this.fadeTab.Controls.Add(this.neonBtn);
            this.fadeTab.Controls.Add(this.sinBtn);
            this.fadeTab.Controls.Add(this.hsvBtn);
            this.fadeTab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.fadeTab.Location = new System.Drawing.Point(4, 25);
            this.fadeTab.Name = "fadeTab";
            this.fadeTab.Padding = new System.Windows.Forms.Padding(3);
            this.fadeTab.Size = new System.Drawing.Size(569, 255);
            this.fadeTab.TabIndex = 2;
            this.fadeTab.Text = "Fade";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(531, 127);
            this.label1.TabIndex = 9;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // neonBtn
            // 
            this.neonBtn.AutoSize = true;
            this.neonBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.neonBtn.Location = new System.Drawing.Point(399, 32);
            this.neonBtn.Name = "neonBtn";
            this.neonBtn.Size = new System.Drawing.Size(75, 36);
            this.neonBtn.TabIndex = 7;
            this.neonBtn.Text = "Neon";
            this.neonBtn.UseVisualStyleBackColor = true;
            this.neonBtn.Click += new System.EventHandler(this.neonBtn_Click);
            // 
            // sinBtn
            // 
            this.sinBtn.AutoSize = true;
            this.sinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sinBtn.Location = new System.Drawing.Point(233, 32);
            this.sinBtn.Name = "sinBtn";
            this.sinBtn.Size = new System.Drawing.Size(98, 36);
            this.sinBtn.TabIndex = 6;
            this.sinBtn.Text = "Sin Fade";
            this.sinBtn.UseVisualStyleBackColor = true;
            this.sinBtn.Click += new System.EventHandler(this.sinBtn_Click);
            // 
            // hsvBtn
            // 
            this.hsvBtn.AutoSize = true;
            this.hsvBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hsvBtn.Location = new System.Drawing.Point(57, 32);
            this.hsvBtn.Name = "hsvBtn";
            this.hsvBtn.Size = new System.Drawing.Size(75, 36);
            this.hsvBtn.TabIndex = 5;
            this.hsvBtn.Text = "HSV";
            this.hsvBtn.UseVisualStyleBackColor = true;
            this.hsvBtn.Click += new System.EventHandler(this.hsvBtn_Click);
            // 
            // customTab
            // 
            this.customTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.customTab.Controls.Add(this.customConfigBtn);
            this.customTab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.customTab.Location = new System.Drawing.Point(4, 25);
            this.customTab.Name = "customTab";
            this.customTab.Padding = new System.Windows.Forms.Padding(3);
            this.customTab.Size = new System.Drawing.Size(569, 255);
            this.customTab.TabIndex = 3;
            this.customTab.Text = "Custom";
            // 
            // customConfigBtn
            // 
            this.customConfigBtn.AutoSize = true;
            this.customConfigBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customConfigBtn.Location = new System.Drawing.Point(203, 111);
            this.customConfigBtn.Name = "customConfigBtn";
            this.customConfigBtn.Size = new System.Drawing.Size(232, 36);
            this.customConfigBtn.TabIndex = 7;
            this.customConfigBtn.Text = "Open Advanced Settings";
            this.customConfigBtn.UseVisualStyleBackColor = true;
            this.customConfigBtn.Click += new System.EventHandler(this.customConfigBtn_Click);
            // 
            // antumbraLabel
            // 
            this.antumbraLabel.AutoSize = true;
            this.antumbraLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.antumbraLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.antumbraLabel.Location = new System.Drawing.Point(70, 8);
            this.antumbraLabel.Name = "antumbraLabel";
            this.antumbraLabel.Size = new System.Drawing.Size(189, 44);
            this.antumbraLabel.TabIndex = 75;
            this.antumbraLabel.Text = "antumbra";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.versionLabel.Location = new System.Drawing.Point(200, 14);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 28);
            this.versionLabel.TabIndex = 76;
            // 
            // colorWheel
            // 
            this.colorWheel.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorWheel.Location = new System.Drawing.Point(22, 19);
            this.colorWheel.Name = "colorWheel";
            this.colorWheel.Size = new System.Drawing.Size(202, 215);
            this.colorWheel.TabIndex = 0;
            this.colorWheel.ColorChanged += new System.EventHandler(this.colorWheel_ColorChanged);
            // 
            // setPollingSizeBtn
            // 
            this.setPollingSizeBtn.AutoSize = true;
            this.setPollingSizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setPollingSizeBtn.Location = new System.Drawing.Point(402, 216);
            this.setPollingSizeBtn.Name = "setPollingSizeBtn";
            this.setPollingSizeBtn.Size = new System.Drawing.Size(161, 36);
            this.setPollingSizeBtn.TabIndex = 5;
            this.setPollingSizeBtn.Text = "Set Capture Size";
            this.setPollingSizeBtn.UseVisualStyleBackColor = true;
            this.setPollingSizeBtn.Click += new System.EventHandler(this.setPollingSizeBtn_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(576, 334);
            this.Controls.Add(this.quitBtn);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.antumbraLabel);
            this.Controls.Add(this.flatTabControl);
            this.Controls.Add(this.closeBtn);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainWindow";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseDown);
            this.flatTabControl.ResumeLayout(false);
            this.manualTab.ResumeLayout(false);
            this.manualTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessTrackBar)).EndInit();
            this.mirrorTab.ResumeLayout(false);
            this.mirrorTab.PerformLayout();
            this.fadeTab.ResumeLayout(false);
            this.fadeTab.PerformLayout();
            this.customTab.ResumeLayout(false);
            this.customTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeBtn;
        private FlatTabControl.FlatTabControl flatTabControl;
        private System.Windows.Forms.TabPage manualTab;
        private System.Windows.Forms.TabPage fadeTab;
        private System.Windows.Forms.TabPage customTab;
        private System.Windows.Forms.BindingSource bindingSource1;
        private CyotekColorWheel.ColorWheel colorWheel;
        private System.Windows.Forms.Label antumbraLabel;
        private System.Windows.Forms.RadioButton offBtn;
        private System.Windows.Forms.RadioButton onBtn;
        private System.Windows.Forms.Label brightnessLabel;
        private System.Windows.Forms.TrackBar brightnessTrackBar;
        private System.Windows.Forms.TabPage mirrorTab;
        private System.Windows.Forms.Label modeDescs;
        private System.Windows.Forms.Button gameBtn;
        private System.Windows.Forms.Button smoothBtn;
        private System.Windows.Forms.Button augmentBtn;
        private System.Windows.Forms.Button mirrorBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button neonBtn;
        private System.Windows.Forms.Button sinBtn;
        private System.Windows.Forms.Button hsvBtn;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button customConfigBtn;
        private System.Windows.Forms.Button quitBtn;
        private System.Windows.Forms.Button setPollingSizeBtn;
    }
}