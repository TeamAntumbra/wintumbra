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
            this.mirrorTab = new System.Windows.Forms.TabPage();
            this.fadeTab = new System.Windows.Forms.TabPage();
            this.customTab = new System.Windows.Forms.TabPage();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.colorWheel = new Antumbra.Glow.View.CyotekColorWheel.ColorWheel();
            this.antumbraLabel = new System.Windows.Forms.Label();
            this.onBtn = new System.Windows.Forms.RadioButton();
            this.offBtn = new System.Windows.Forms.RadioButton();
            this.brightnessTrackBar = new System.Windows.Forms.TrackBar();
            this.brightnessLabel = new System.Windows.Forms.Label();
            this.flatTabControl.SuspendLayout();
            this.manualTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessTrackBar)).BeginInit();
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
            this.closeBtn.Size = new System.Drawing.Size(28, 28);
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
            this.flatTabControl.Location = new System.Drawing.Point(14, 51);
            this.flatTabControl.myBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.flatTabControl.Name = "flatTabControl";
            this.flatTabControl.SelectedIndex = 0;
            this.flatTabControl.Size = new System.Drawing.Size(551, 284);
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
            this.manualTab.Size = new System.Drawing.Size(543, 255);
            this.manualTab.TabIndex = 0;
            this.manualTab.Text = "Manual";
            // 
            // mirrorTab
            // 
            this.mirrorTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.mirrorTab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.mirrorTab.Location = new System.Drawing.Point(4, 25);
            this.mirrorTab.Name = "mirrorTab";
            this.mirrorTab.Padding = new System.Windows.Forms.Padding(3);
            this.mirrorTab.Size = new System.Drawing.Size(747, 255);
            this.mirrorTab.TabIndex = 1;
            this.mirrorTab.Text = "Mirror";
            // 
            // fadeTab
            // 
            this.fadeTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.fadeTab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.fadeTab.Location = new System.Drawing.Point(4, 25);
            this.fadeTab.Name = "fadeTab";
            this.fadeTab.Padding = new System.Windows.Forms.Padding(3);
            this.fadeTab.Size = new System.Drawing.Size(747, 255);
            this.fadeTab.TabIndex = 2;
            this.fadeTab.Text = "Fade";
            // 
            // customTab
            // 
            this.customTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.customTab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.customTab.Location = new System.Drawing.Point(4, 25);
            this.customTab.Name = "customTab";
            this.customTab.Padding = new System.Windows.Forms.Padding(3);
            this.customTab.Size = new System.Drawing.Size(747, 255);
            this.customTab.TabIndex = 3;
            this.customTab.Text = "Custom";
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
            // antumbraLabel
            // 
            this.antumbraLabel.AutoSize = true;
            this.antumbraLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.antumbraLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.antumbraLabel.Location = new System.Drawing.Point(70, 8);
            this.antumbraLabel.Name = "antumbraLabel";
            this.antumbraLabel.Size = new System.Drawing.Size(124, 28);
            this.antumbraLabel.TabIndex = 75;
            this.antumbraLabel.Text = "antumbra";
            // 
            // onBtn
            // 
            this.onBtn.AutoSize = true;
            this.onBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.onBtn.Location = new System.Drawing.Point(406, 214);
            this.onBtn.Name = "onBtn";
            this.onBtn.Size = new System.Drawing.Size(42, 20);
            this.onBtn.TabIndex = 1;
            this.onBtn.TabStop = true;
            this.onBtn.Text = "On";
            this.onBtn.UseVisualStyleBackColor = true;
            // 
            // offBtn
            // 
            this.offBtn.AutoSize = true;
            this.offBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.offBtn.Location = new System.Drawing.Point(464, 214);
            this.offBtn.Name = "offBtn";
            this.offBtn.Size = new System.Drawing.Size(41, 20);
            this.offBtn.TabIndex = 2;
            this.offBtn.TabStop = true;
            this.offBtn.Text = "Off";
            this.offBtn.UseVisualStyleBackColor = true;
            // 
            // brightnessTrackBar
            // 
            this.brightnessTrackBar.Location = new System.Drawing.Point(273, 44);
            this.brightnessTrackBar.Name = "brightnessTrackBar";
            this.brightnessTrackBar.Size = new System.Drawing.Size(244, 45);
            this.brightnessTrackBar.TabIndex = 3;
            // 
            // brightnessLabel
            // 
            this.brightnessLabel.AutoSize = true;
            this.brightnessLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.brightnessLabel.Location = new System.Drawing.Point(358, 82);
            this.brightnessLabel.Name = "brightnessLabel";
            this.brightnessLabel.Size = new System.Drawing.Size(78, 18);
            this.brightnessLabel.TabIndex = 4;
            this.brightnessLabel.Text = "Brightness";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(576, 338);
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
            this.flatTabControl.ResumeLayout(false);
            this.manualTab.ResumeLayout(false);
            this.manualTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeBtn;
        private FlatTabControl.FlatTabControl flatTabControl;
        private System.Windows.Forms.TabPage manualTab;
        private System.Windows.Forms.TabPage mirrorTab;
        private System.Windows.Forms.TabPage fadeTab;
        private System.Windows.Forms.TabPage customTab;
        private System.Windows.Forms.BindingSource bindingSource1;
        private CyotekColorWheel.ColorWheel colorWheel;
        private System.Windows.Forms.Label antumbraLabel;
        private System.Windows.Forms.RadioButton offBtn;
        private System.Windows.Forms.RadioButton onBtn;
        private System.Windows.Forms.Label brightnessLabel;
        private System.Windows.Forms.TrackBar brightnessTrackBar;
    }
}