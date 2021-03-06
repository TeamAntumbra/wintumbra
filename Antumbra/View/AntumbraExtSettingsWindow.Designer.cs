﻿namespace Antumbra.Glow.View {
    partial class AntumbraExtSettingsWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.closeBtn = new System.Windows.Forms.Button();
            this.extNameLabel = new System.Windows.Forms.Label();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.DescLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.ExtName = new System.Windows.Forms.Label();
            this.Author = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.Label();
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
            this.closeBtn.Location = new System.Drawing.Point(418, -2);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(26, 25);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.Text = "X";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // extNameLabel
            // 
            this.extNameLabel.AutoSize = true;
            this.extNameLabel.Location = new System.Drawing.Point(46, 73);
            this.extNameLabel.Name = "extNameLabel";
            this.extNameLabel.Size = new System.Drawing.Size(38, 13);
            this.extNameLabel.TabIndex = 1;
            this.extNameLabel.Text = "Name:";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(46, 103);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(41, 13);
            this.AuthorLabel.TabIndex = 2;
            this.AuthorLabel.Text = "Author:";
            // 
            // DescLabel
            // 
            this.DescLabel.AutoSize = true;
            this.DescLabel.Location = new System.Drawing.Point(46, 171);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Size = new System.Drawing.Size(63, 13);
            this.DescLabel.TabIndex = 3;
            this.DescLabel.Text = "Description:";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(46, 136);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(45, 13);
            this.VersionLabel.TabIndex = 4;
            this.VersionLabel.Text = "Version:";
            // 
            // ExtName
            // 
            this.ExtName.AutoSize = true;
            this.ExtName.Location = new System.Drawing.Point(111, 73);
            this.ExtName.Name = "ExtName";
            this.ExtName.Size = new System.Drawing.Size(0, 13);
            this.ExtName.TabIndex = 5;
            // 
            // Author
            // 
            this.Author.AutoSize = true;
            this.Author.Location = new System.Drawing.Point(111, 103);
            this.Author.Name = "Author";
            this.Author.Size = new System.Drawing.Size(0, 13);
            this.Author.TabIndex = 6;
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Location = new System.Drawing.Point(111, 136);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(0, 13);
            this.Version.TabIndex = 7;
            // 
            // Description
            // 
            this.Description.Location = new System.Drawing.Point(115, 171);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(329, 135);
            this.Description.TabIndex = 8;
            // 
            // AntumbraExtSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(495, 341);
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
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "AntumbraExtSettingsWindow";
            this.Padding = new System.Windows.Forms.Padding(5);
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
    }
}