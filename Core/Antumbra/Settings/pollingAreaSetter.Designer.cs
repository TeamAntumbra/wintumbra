﻿namespace Antumbra.Glow.Settings
{
    partial class pollingAreaSetter
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
            this.title = new System.Windows.Forms.Label();
            this.setToUniqueColorBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(108, 120);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(541, 24);
            this.title.TabIndex = 0;
            this.title.Tag = "";
            this.title.Text = "Cover the area you would like to capture then close this window";
            // 
            // setToUniqueColorBtn
            // 
            this.setToUniqueColorBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.setToUniqueColorBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.setToUniqueColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setToUniqueColorBtn.Location = new System.Drawing.Point(338, 195);
            this.setToUniqueColorBtn.Name = "setToUniqueColorBtn";
            this.setToUniqueColorBtn.Size = new System.Drawing.Size(97, 38);
            this.setToUniqueColorBtn.TabIndex = 1;
            this.setToUniqueColorBtn.Text = "Set To Unique Color";
            this.setToUniqueColorBtn.UseVisualStyleBackColor = true;
            this.setToUniqueColorBtn.Click += new System.EventHandler(this.setToUniqueColorBtn_Click);
            // 
            // pollingAreaSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(785, 338);
            this.Controls.Add(this.setToUniqueColorBtn);
            this.Controls.Add(this.title);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "pollingAreaSetter";
            this.Opacity = 0.7D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.pollingAreaSetter_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;
        public System.Windows.Forms.Button setToUniqueColorBtn;
    }
}