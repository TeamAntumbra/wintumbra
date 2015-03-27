namespace Antumbra.Glow.View
{
    partial class WhiteBalanceWindow
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
            this.redValue = new System.Windows.Forms.TextBox();
            this.greenValue = new System.Windows.Forms.TextBox();
            this.blueValue = new System.Windows.Forms.TextBox();
            this.bUpBtn = new System.Windows.Forms.Button();
            this.gUpBtn = new System.Windows.Forms.Button();
            this.rUpBtn = new System.Windows.Forms.Button();
            this.rDownBtn = new System.Windows.Forms.Button();
            this.gDownBtn = new System.Windows.Forms.Button();
            this.bDownBtn = new System.Windows.Forms.Button();
            this.redLabel = new System.Windows.Forms.Label();
            this.greenLabel = new System.Windows.Forms.Label();
            this.blueLabel = new System.Windows.Forms.Label();
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
            this.closeBtn.Location = new System.Drawing.Point(764, -6);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(32, 32);
            this.closeBtn.TabIndex = 74;
            this.closeBtn.Text = "X";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // redValue
            // 
            this.redValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.redValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.redValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.redValue.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.redValue.Location = new System.Drawing.Point(169, 207);
            this.redValue.Name = "redValue";
            this.redValue.ReadOnly = true;
            this.redValue.Size = new System.Drawing.Size(75, 30);
            this.redValue.TabIndex = 75;
            this.redValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // greenValue
            // 
            this.greenValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.greenValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.greenValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.greenValue.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.greenValue.Location = new System.Drawing.Point(358, 207);
            this.greenValue.Name = "greenValue";
            this.greenValue.ReadOnly = true;
            this.greenValue.Size = new System.Drawing.Size(75, 30);
            this.greenValue.TabIndex = 76;
            this.greenValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // blueValue
            // 
            this.blueValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.blueValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.blueValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.blueValue.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.blueValue.Location = new System.Drawing.Point(535, 207);
            this.blueValue.Name = "blueValue";
            this.blueValue.ReadOnly = true;
            this.blueValue.Size = new System.Drawing.Size(75, 30);
            this.blueValue.TabIndex = 77;
            this.blueValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bUpBtn
            // 
            this.bUpBtn.AutoSize = true;
            this.bUpBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.bUpBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.bUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bUpBtn.Location = new System.Drawing.Point(535, 159);
            this.bUpBtn.Name = "bUpBtn";
            this.bUpBtn.Size = new System.Drawing.Size(75, 32);
            this.bUpBtn.TabIndex = 78;
            this.bUpBtn.Text = "/\\";
            this.bUpBtn.UseVisualStyleBackColor = true;
            this.bUpBtn.Click += new System.EventHandler(this.bUpBtn_Click);
            // 
            // gUpBtn
            // 
            this.gUpBtn.AutoSize = true;
            this.gUpBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.gUpBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.gUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gUpBtn.Location = new System.Drawing.Point(358, 159);
            this.gUpBtn.Name = "gUpBtn";
            this.gUpBtn.Size = new System.Drawing.Size(75, 32);
            this.gUpBtn.TabIndex = 79;
            this.gUpBtn.Text = "/\\";
            this.gUpBtn.UseVisualStyleBackColor = true;
            this.gUpBtn.Click += new System.EventHandler(this.gUpBtn_Click);
            // 
            // rUpBtn
            // 
            this.rUpBtn.AutoSize = true;
            this.rUpBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rUpBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.rUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rUpBtn.Location = new System.Drawing.Point(169, 159);
            this.rUpBtn.Name = "rUpBtn";
            this.rUpBtn.Size = new System.Drawing.Size(76, 32);
            this.rUpBtn.TabIndex = 80;
            this.rUpBtn.Text = "/\\";
            this.rUpBtn.UseVisualStyleBackColor = true;
            this.rUpBtn.Click += new System.EventHandler(this.rUpBtn_Click);
            // 
            // rDownBtn
            // 
            this.rDownBtn.AutoSize = true;
            this.rDownBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.rDownBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.rDownBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rDownBtn.Location = new System.Drawing.Point(169, 261);
            this.rDownBtn.Name = "rDownBtn";
            this.rDownBtn.Size = new System.Drawing.Size(75, 32);
            this.rDownBtn.TabIndex = 81;
            this.rDownBtn.Text = "\\/";
            this.rDownBtn.UseVisualStyleBackColor = true;
            this.rDownBtn.Click += new System.EventHandler(this.rDownBtn_Click);
            // 
            // gDownBtn
            // 
            this.gDownBtn.AutoSize = true;
            this.gDownBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.gDownBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.gDownBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gDownBtn.Location = new System.Drawing.Point(358, 261);
            this.gDownBtn.Name = "gDownBtn";
            this.gDownBtn.Size = new System.Drawing.Size(75, 32);
            this.gDownBtn.TabIndex = 82;
            this.gDownBtn.Text = "\\/";
            this.gDownBtn.UseVisualStyleBackColor = true;
            this.gDownBtn.Click += new System.EventHandler(this.gDownBtn_Click);
            // 
            // bDownBtn
            // 
            this.bDownBtn.AutoSize = true;
            this.bDownBtn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.bDownBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.bDownBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bDownBtn.Location = new System.Drawing.Point(535, 261);
            this.bDownBtn.Name = "bDownBtn";
            this.bDownBtn.Size = new System.Drawing.Size(76, 32);
            this.bDownBtn.TabIndex = 83;
            this.bDownBtn.Text = "\\/";
            this.bDownBtn.UseVisualStyleBackColor = true;
            this.bDownBtn.Click += new System.EventHandler(this.bDownBtn_Click);
            // 
            // redLabel
            // 
            this.redLabel.AutoSize = true;
            this.redLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.redLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.redLabel.Location = new System.Drawing.Point(184, 114);
            this.redLabel.Name = "redLabel";
            this.redLabel.Size = new System.Drawing.Size(47, 25);
            this.redLabel.TabIndex = 85;
            this.redLabel.Text = "Red";
            // 
            // greenLabel
            // 
            this.greenLabel.AutoSize = true;
            this.greenLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.greenLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.greenLabel.Location = new System.Drawing.Point(363, 114);
            this.greenLabel.Name = "greenLabel";
            this.greenLabel.Size = new System.Drawing.Size(66, 25);
            this.greenLabel.TabIndex = 86;
            this.greenLabel.Text = "Green";
            // 
            // blueLabel
            // 
            this.blueLabel.AutoSize = true;
            this.blueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.blueLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.blueLabel.Location = new System.Drawing.Point(545, 114);
            this.blueLabel.Name = "blueLabel";
            this.blueLabel.Size = new System.Drawing.Size(51, 25);
            this.blueLabel.TabIndex = 87;
            this.blueLabel.Text = "Blue";
            // 
            // WhiteBalanceWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(822, 468);
            this.Controls.Add(this.blueLabel);
            this.Controls.Add(this.greenLabel);
            this.Controls.Add(this.redLabel);
            this.Controls.Add(this.bDownBtn);
            this.Controls.Add(this.gDownBtn);
            this.Controls.Add(this.rDownBtn);
            this.Controls.Add(this.rUpBtn);
            this.Controls.Add(this.gUpBtn);
            this.Controls.Add(this.bUpBtn);
            this.Controls.Add(this.blueValue);
            this.Controls.Add(this.greenValue);
            this.Controls.Add(this.redValue);
            this.Controls.Add(this.closeBtn);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WhiteBalanceWindow";
            this.Text = "WhiteBalanceWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.TextBox redValue;
        private System.Windows.Forms.TextBox greenValue;
        private System.Windows.Forms.TextBox blueValue;
        private System.Windows.Forms.Button bUpBtn;
        private System.Windows.Forms.Button gUpBtn;
        private System.Windows.Forms.Button rUpBtn;
        private System.Windows.Forms.Button rDownBtn;
        private System.Windows.Forms.Button gDownBtn;
        private System.Windows.Forms.Button bDownBtn;
        private System.Windows.Forms.Label redLabel;
        private System.Windows.Forms.Label greenLabel;
        private System.Windows.Forms.Label blueLabel;
    }
}