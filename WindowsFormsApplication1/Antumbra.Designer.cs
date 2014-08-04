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
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.colorChoose = new System.Windows.Forms.ColorDialog();
            this.powerToggleBtn = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // modeComboBox
            // 
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Items.AddRange(new object[] {
            "Off",
            "Color Fade",
            "HSV Sweep",
            "Screen Responsive",
            "Manual Selection"});
            this.modeComboBox.Location = new System.Drawing.Point(53, 175);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(189, 27);
            this.modeComboBox.TabIndex = 8;
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(0, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(100, 23);
            this.linkLabel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(45, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 45);
            this.label1.TabIndex = 2;
            this.label1.Text = "Antumbra";
            // 
            // powerToggleBtn
            // 
            this.powerToggleBtn.Location = new System.Drawing.Point(53, 322);
            this.powerToggleBtn.Name = "powerToggleBtn";
            this.powerToggleBtn.Size = new System.Drawing.Size(189, 34);
            this.powerToggleBtn.TabIndex = 7;
            this.powerToggleBtn.Text = "Toggle Power";
            this.powerToggleBtn.UseVisualStyleBackColor = true;
            this.powerToggleBtn.Click += new System.EventHandler(this.powerToggleBtn_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "notifyIcon";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // Antumbra
            // 
            this.ClientSize = new System.Drawing.Size(298, 368);
            this.Controls.Add(this.modeComboBox);
            this.Controls.Add(this.powerToggleBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel1);
            this.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Antumbra";
            this.Text = "Antumbra";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.Antumbra_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorChoose;
        private System.Windows.Forms.Button powerToggleBtn;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ComboBox modeComboBox;
    }
}

