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
            this.takeScreenshotBtn = new System.Windows.Forms.Button();
            this.continuousCheckBox = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.toggleSerial = new System.Windows.Forms.Button();
            this.colorChoose = new System.Windows.Forms.ColorDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // takeScreenshotBtn
            // 
            this.takeScreenshotBtn.Location = new System.Drawing.Point(53, 221);
            this.takeScreenshotBtn.Name = "takeScreenshotBtn";
            this.takeScreenshotBtn.Size = new System.Drawing.Size(189, 37);
            this.takeScreenshotBtn.TabIndex = 0;
            this.takeScreenshotBtn.Text = "Avg Color";
            this.takeScreenshotBtn.UseVisualStyleBackColor = true;
            this.takeScreenshotBtn.Click += new System.EventHandler(this.takeScreenshotBtn_Click);
            // 
            // continuousCheckBox
            // 
            this.continuousCheckBox.AutoSize = true;
            this.continuousCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.continuousCheckBox.Location = new System.Drawing.Point(83, 264);
            this.continuousCheckBox.Name = "continuousCheckBox";
            this.continuousCheckBox.Size = new System.Drawing.Size(125, 24);
            this.continuousCheckBox.TabIndex = 1;
            this.continuousCheckBox.Text = "Continuous?";
            this.continuousCheckBox.UseVisualStyleBackColor = true;
            this.continuousCheckBox.CheckedChanged += new System.EventHandler(this.continuousCheckBox_CheckedChanged);
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
            // toggleSerial
            // 
            this.toggleSerial.Location = new System.Drawing.Point(83, 336);
            this.toggleSerial.Name = "toggleSerial";
            this.toggleSerial.Size = new System.Drawing.Size(125, 23);
            this.toggleSerial.TabIndex = 4;
            this.toggleSerial.Text = "Toggle Serial";
            this.toggleSerial.UseVisualStyleBackColor = true;
            this.toggleSerial.Click += new System.EventHandler(this.toggleSerial_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(83, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Choose Color";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Antumbra
            // 
            this.ClientSize = new System.Drawing.Size(298, 368);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.toggleSerial);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.continuousCheckBox);
            this.Controls.Add(this.takeScreenshotBtn);
            this.Font = new System.Drawing.Font("Times New Roman", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Antumbra";
            this.Text = "Antumbra";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button takeScreenshotBtn;
        private System.Windows.Forms.CheckBox continuousCheckBox;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button toggleSerial;
        private System.Windows.Forms.ColorDialog colorChoose;
        private System.Windows.Forms.Button button1;
    }
}

