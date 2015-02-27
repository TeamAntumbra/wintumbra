namespace Saturator
{
    partial class SaturatorSettings
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
            this.closeBtn = new System.Windows.Forms.Button();
            this.extNameLabel = new System.Windows.Forms.Label();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.DescLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.ExtName = new System.Windows.Forms.Label();
            this.Author = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.Label();
            this.saturateAmtLabel = new System.Windows.Forms.Label();
            this.saturateAmtTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // closeBtn
            // 
            this.closeBtn.AutoSize = true;
            this.closeBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.closeBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.closeBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(627, -3);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(32, 32);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.Text = "X";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // extNameLabel
            // 
            this.extNameLabel.AutoSize = true;
            this.extNameLabel.Location = new System.Drawing.Point(69, 112);
            this.extNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.extNameLabel.Name = "extNameLabel";
            this.extNameLabel.Size = new System.Drawing.Size(55, 20);
            this.extNameLabel.TabIndex = 1;
            this.extNameLabel.Text = "Name:";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(69, 158);
            this.AuthorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(61, 20);
            this.AuthorLabel.TabIndex = 2;
            this.AuthorLabel.Text = "Author:";
            // 
            // DescLabel
            // 
            this.DescLabel.AutoSize = true;
            this.DescLabel.Location = new System.Drawing.Point(69, 263);
            this.DescLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Size = new System.Drawing.Size(93, 20);
            this.DescLabel.TabIndex = 3;
            this.DescLabel.Text = "Description:";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(69, 209);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(67, 20);
            this.VersionLabel.TabIndex = 4;
            this.VersionLabel.Text = "Version:";
            // 
            // ExtName
            // 
            this.ExtName.AutoSize = true;
            this.ExtName.Location = new System.Drawing.Point(166, 112);
            this.ExtName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ExtName.Name = "ExtName";
            this.ExtName.Size = new System.Drawing.Size(0, 20);
            this.ExtName.TabIndex = 5;
            // 
            // Author
            // 
            this.Author.AutoSize = true;
            this.Author.Location = new System.Drawing.Point(166, 158);
            this.Author.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Author.Name = "Author";
            this.Author.Size = new System.Drawing.Size(0, 20);
            this.Author.TabIndex = 6;
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Location = new System.Drawing.Point(166, 209);
            this.Version.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(0, 20);
            this.Version.TabIndex = 7;
            // 
            // Description
            // 
            this.Description.Location = new System.Drawing.Point(172, 263);
            this.Description.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(494, 208);
            this.Description.TabIndex = 8;
            // 
            // saturateAmtLabel
            // 
            this.saturateAmtLabel.AutoSize = true;
            this.saturateAmtLabel.Location = new System.Drawing.Point(69, 522);
            this.saturateAmtLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.saturateAmtLabel.Name = "saturateAmtLabel";
            this.saturateAmtLabel.Size = new System.Drawing.Size(190, 20);
            this.saturateAmtLabel.TabIndex = 9;
            this.saturateAmtLabel.Text = "Amount to Saturate (0-1):";
            // 
            // saturateAmtTxt
            // 
            this.saturateAmtTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.saturateAmtTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.saturateAmtTxt.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.saturateAmtTxt.Location = new System.Drawing.Point(266, 522);
            this.saturateAmtTxt.Name = "saturateAmtTxt";
            this.saturateAmtTxt.Size = new System.Drawing.Size(90, 19);
            this.saturateAmtTxt.TabIndex = 10;
            // 
            // SaturatorSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(742, 588);
            this.Controls.Add(this.saturateAmtTxt);
            this.Controls.Add(this.saturateAmtLabel);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.Author);
            this.Controls.Add(this.ExtName);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.DescLabel);
            this.Controls.Add(this.AuthorLabel);
            this.Controls.Add(this.extNameLabel);
            this.Controls.Add(this.closeBtn);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(450, 462);
            this.Name = "SaturatorSettings";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AntumbraExtSettingsWindow_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label extNameLabel;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label ExtName;
        private System.Windows.Forms.Label Author;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Label Description;
        public System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label saturateAmtLabel;
        public System.Windows.Forms.TextBox saturateAmtTxt;
    }
}