namespace Antumbra
{
    partial class SettingsWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pollingX = new System.Windows.Forms.TextBox();
            this.pollingY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.screenPollingWait = new System.Windows.Forms.TextBox();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ManualStepSize = new System.Windows.Forms.TextBox();
            this.HSVstepSize = new System.Windows.Forms.TextBox();
            this.colorFadeStepSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.fullBtn = new System.Windows.Forms.Button();
            this.stepSleepLabel = new System.Windows.Forms.Label();
            this.HSVsleepSizeLabel = new System.Windows.Forms.Label();
            this.ColorFadeSleepSizeLabel = new System.Windows.Forms.Label();
            this.ManualSleepSizeLabel = new System.Windows.Forms.Label();
            this.HSVsleepSize = new System.Windows.Forms.TextBox();
            this.ColorFadeSleepSize = new System.Windows.Forms.TextBox();
            this.ManualSleepSize = new System.Windows.Forms.TextBox();
            this.screenStepSleepLabel = new System.Windows.Forms.Label();
            this.screenStepSleep = new System.Windows.Forms.TextBox();
            this.screenStepSize = new System.Windows.Forms.TextBox();
            this.screenStepSizeLabel = new System.Windows.Forms.Label();
            this.displayLabel = new System.Windows.Forms.Label();
            this.displayIndex = new System.Windows.Forms.NumericUpDown();
            this.manualColorBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.displayIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Polling Area:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y:";
            // 
            // pollingX
            // 
            this.pollingX.Location = new System.Drawing.Point(109, 44);
            this.pollingX.Margin = new System.Windows.Forms.Padding(2);
            this.pollingX.Name = "pollingX";
            this.pollingX.Size = new System.Drawing.Size(41, 20);
            this.pollingX.TabIndex = 3;
            this.pollingX.Text = "0";
            this.pollingX.TextChanged += new System.EventHandler(this.pollingX_TextChanged);
            // 
            // pollingY
            // 
            this.pollingY.Location = new System.Drawing.Point(173, 44);
            this.pollingY.Margin = new System.Windows.Forms.Padding(2);
            this.pollingY.Name = "pollingY";
            this.pollingY.Size = new System.Drawing.Size(48, 20);
            this.pollingY.TabIndex = 4;
            this.pollingY.Text = "0";
            this.pollingY.TextChanged += new System.EventHandler(this.pollingY_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 101);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Screen Polling Frequency (ms): ";
            // 
            // screenPollingWait
            // 
            this.screenPollingWait.Location = new System.Drawing.Point(187, 101);
            this.screenPollingWait.Margin = new System.Windows.Forms.Padding(2);
            this.screenPollingWait.Name = "screenPollingWait";
            this.screenPollingWait.Size = new System.Drawing.Size(42, 20);
            this.screenPollingWait.TabIndex = 6;
            this.screenPollingWait.TextChanged += new System.EventHandler(this.screenPollingFq_TextChanged);
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.Location = new System.Drawing.Point(27, 128);
            this.stepSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(55, 13);
            this.stepSizeLabel.TabIndex = 7;
            this.stepSizeLabel.Text = "Step Size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(61, 189);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Manual:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(73, 147);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "HSV:";
            // 
            // ManualStepSize
            // 
            this.ManualStepSize.Location = new System.Drawing.Point(126, 185);
            this.ManualStepSize.Margin = new System.Windows.Forms.Padding(2);
            this.ManualStepSize.Name = "ManualStepSize";
            this.ManualStepSize.Size = new System.Drawing.Size(68, 20);
            this.ManualStepSize.TabIndex = 12;
            this.ManualStepSize.TextChanged += new System.EventHandler(this.ManualStepSize_TextChanged);
            // 
            // HSVstepSize
            // 
            this.HSVstepSize.Location = new System.Drawing.Point(126, 143);
            this.HSVstepSize.Margin = new System.Windows.Forms.Padding(2);
            this.HSVstepSize.Name = "HSVstepSize";
            this.HSVstepSize.Size = new System.Drawing.Size(68, 20);
            this.HSVstepSize.TabIndex = 13;
            this.HSVstepSize.TextChanged += new System.EventHandler(this.HSVstepSize_TextChanged);
            // 
            // colorFadeStepSize
            // 
            this.colorFadeStepSize.Location = new System.Drawing.Point(126, 164);
            this.colorFadeStepSize.Margin = new System.Windows.Forms.Padding(2);
            this.colorFadeStepSize.Name = "colorFadeStepSize";
            this.colorFadeStepSize.Size = new System.Drawing.Size(68, 20);
            this.colorFadeStepSize.TabIndex = 14;
            this.colorFadeStepSize.TextChanged += new System.EventHandler(this.colorFadeStepSize_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 168);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Color Fade:";
            // 
            // fullBtn
            // 
            this.fullBtn.Location = new System.Drawing.Point(119, 68);
            this.fullBtn.Margin = new System.Windows.Forms.Padding(2);
            this.fullBtn.Name = "fullBtn";
            this.fullBtn.Size = new System.Drawing.Size(82, 19);
            this.fullBtn.TabIndex = 16;
            this.fullBtn.Text = "Full Screen";
            this.fullBtn.UseVisualStyleBackColor = true;
            this.fullBtn.Click += new System.EventHandler(this.fullBtn_Click);
            // 
            // stepSleepLabel
            // 
            this.stepSleepLabel.AutoSize = true;
            this.stepSleepLabel.Location = new System.Drawing.Point(30, 244);
            this.stepSleepLabel.Name = "stepSleepLabel";
            this.stepSleepLabel.Size = new System.Drawing.Size(84, 13);
            this.stepSleepLabel.TabIndex = 17;
            this.stepSleepLabel.Text = "Step Sleep: (ms)";
            // 
            // HSVsleepSizeLabel
            // 
            this.HSVsleepSizeLabel.AutoSize = true;
            this.HSVsleepSizeLabel.Location = new System.Drawing.Point(73, 263);
            this.HSVsleepSizeLabel.Name = "HSVsleepSizeLabel";
            this.HSVsleepSizeLabel.Size = new System.Drawing.Size(32, 13);
            this.HSVsleepSizeLabel.TabIndex = 18;
            this.HSVsleepSizeLabel.Text = "HSV:";
            // 
            // ColorFadeSleepSizeLabel
            // 
            this.ColorFadeSleepSizeLabel.AutoSize = true;
            this.ColorFadeSleepSizeLabel.Location = new System.Drawing.Point(44, 284);
            this.ColorFadeSleepSizeLabel.Name = "ColorFadeSleepSizeLabel";
            this.ColorFadeSleepSizeLabel.Size = new System.Drawing.Size(61, 13);
            this.ColorFadeSleepSizeLabel.TabIndex = 20;
            this.ColorFadeSleepSizeLabel.Text = "Color Fade:";
            // 
            // ManualSleepSizeLabel
            // 
            this.ManualSleepSizeLabel.AutoSize = true;
            this.ManualSleepSizeLabel.Location = new System.Drawing.Point(61, 306);
            this.ManualSleepSizeLabel.Name = "ManualSleepSizeLabel";
            this.ManualSleepSizeLabel.Size = new System.Drawing.Size(45, 13);
            this.ManualSleepSizeLabel.TabIndex = 21;
            this.ManualSleepSizeLabel.Text = "Manual:";
            // 
            // HSVsleepSize
            // 
            this.HSVsleepSize.Location = new System.Drawing.Point(126, 259);
            this.HSVsleepSize.Name = "HSVsleepSize";
            this.HSVsleepSize.Size = new System.Drawing.Size(68, 20);
            this.HSVsleepSize.TabIndex = 22;
            this.HSVsleepSize.TextChanged += new System.EventHandler(this.HSVsleepSize_TextChanged);
            // 
            // ColorFadeSleepSize
            // 
            this.ColorFadeSleepSize.Location = new System.Drawing.Point(126, 280);
            this.ColorFadeSleepSize.Name = "ColorFadeSleepSize";
            this.ColorFadeSleepSize.Size = new System.Drawing.Size(68, 20);
            this.ColorFadeSleepSize.TabIndex = 23;
            this.ColorFadeSleepSize.TextChanged += new System.EventHandler(this.ColorFadeSleepSize_TextChanged);
            // 
            // ManualSleepSize
            // 
            this.ManualSleepSize.Location = new System.Drawing.Point(126, 302);
            this.ManualSleepSize.Name = "ManualSleepSize";
            this.ManualSleepSize.Size = new System.Drawing.Size(68, 20);
            this.ManualSleepSize.TabIndex = 24;
            this.ManualSleepSize.TextChanged += new System.EventHandler(this.ManualSleepSize_TextChanged);
            // 
            // screenStepSleepLabel
            // 
            this.screenStepSleepLabel.AutoSize = true;
            this.screenStepSleepLabel.Location = new System.Drawing.Point(14, 324);
            this.screenStepSleepLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.screenStepSleepLabel.Name = "screenStepSleepLabel";
            this.screenStepSleepLabel.Size = new System.Drawing.Size(103, 13);
            this.screenStepSleepLabel.TabIndex = 25;
            this.screenStepSleepLabel.Text = "Screen Responsive:";
            // 
            // screenStepSleep
            // 
            this.screenStepSleep.Location = new System.Drawing.Point(126, 324);
            this.screenStepSleep.Name = "screenStepSleep";
            this.screenStepSleep.Size = new System.Drawing.Size(68, 20);
            this.screenStepSleep.TabIndex = 26;
            this.screenStepSleep.TextChanged += new System.EventHandler(this.screenStepSleep_TextChanged);
            // 
            // screenStepSize
            // 
            this.screenStepSize.Location = new System.Drawing.Point(126, 208);
            this.screenStepSize.Margin = new System.Windows.Forms.Padding(2);
            this.screenStepSize.Name = "screenStepSize";
            this.screenStepSize.Size = new System.Drawing.Size(68, 20);
            this.screenStepSize.TabIndex = 28;
            this.screenStepSize.TextChanged += new System.EventHandler(this.screenStepSize_TextChanged);
            // 
            // screenStepSizeLabel
            // 
            this.screenStepSizeLabel.AutoSize = true;
            this.screenStepSizeLabel.Location = new System.Drawing.Point(4, 208);
            this.screenStepSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.screenStepSizeLabel.Name = "screenStepSizeLabel";
            this.screenStepSizeLabel.Size = new System.Drawing.Size(103, 13);
            this.screenStepSizeLabel.TabIndex = 27;
            this.screenStepSizeLabel.Text = "Screen Responsive:";
            // 
            // displayLabel
            // 
            this.displayLabel.AutoSize = true;
            this.displayLabel.Location = new System.Drawing.Point(23, 19);
            this.displayLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size(44, 13);
            this.displayLabel.TabIndex = 29;
            this.displayLabel.Text = "Display:";
            // 
            // displayIndex
            // 
            this.displayIndex.Location = new System.Drawing.Point(92, 19);
            this.displayIndex.Margin = new System.Windows.Forms.Padding(2);
            this.displayIndex.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.displayIndex.Name = "displayIndex";
            this.displayIndex.Size = new System.Drawing.Size(80, 20);
            this.displayIndex.TabIndex = 30;
            this.displayIndex.ValueChanged += new System.EventHandler(this.displayIndex_ValueChanged);
            // 
            // manualColorBtn
            // 
            this.manualColorBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.manualColorBtn.Location = new System.Drawing.Point(47, 374);
            this.manualColorBtn.Name = "manualColorBtn";
            this.manualColorBtn.Size = new System.Drawing.Size(162, 54);
            this.manualColorBtn.TabIndex = 31;
            this.manualColorBtn.UseVisualStyleBackColor = false;
            this.manualColorBtn.Click += new System.EventHandler(this.manualColorBtn_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 455);
            this.Controls.Add(this.manualColorBtn);
            this.Controls.Add(this.displayIndex);
            this.Controls.Add(this.displayLabel);
            this.Controls.Add(this.screenStepSize);
            this.Controls.Add(this.screenStepSizeLabel);
            this.Controls.Add(this.screenStepSleep);
            this.Controls.Add(this.screenStepSleepLabel);
            this.Controls.Add(this.ManualSleepSize);
            this.Controls.Add(this.ColorFadeSleepSize);
            this.Controls.Add(this.HSVsleepSize);
            this.Controls.Add(this.ManualSleepSizeLabel);
            this.Controls.Add(this.ColorFadeSleepSizeLabel);
            this.Controls.Add(this.HSVsleepSizeLabel);
            this.Controls.Add(this.stepSleepLabel);
            this.Controls.Add(this.fullBtn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.colorFadeStepSize);
            this.Controls.Add(this.HSVstepSize);
            this.Controls.Add(this.ManualStepSize);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.stepSizeLabel);
            this.Controls.Add(this.screenPollingWait);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pollingY);
            this.Controls.Add(this.pollingX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SettingsWindow";
            this.ShowIcon = false;
            this.Text = "Settings";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.displayIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pollingX;
        private System.Windows.Forms.TextBox pollingY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox screenPollingWait;
        private System.Windows.Forms.Label stepSizeLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ManualStepSize;
        private System.Windows.Forms.TextBox HSVstepSize;
        private System.Windows.Forms.TextBox colorFadeStepSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button fullBtn;
        private System.Windows.Forms.Label stepSleepLabel;
        private System.Windows.Forms.Label HSVsleepSizeLabel;
        private System.Windows.Forms.Label ColorFadeSleepSizeLabel;
        private System.Windows.Forms.Label ManualSleepSizeLabel;
        private System.Windows.Forms.TextBox HSVsleepSize;
        private System.Windows.Forms.TextBox ColorFadeSleepSize;
        private System.Windows.Forms.TextBox ManualSleepSize;
        private System.Windows.Forms.Label screenStepSleepLabel;
        private System.Windows.Forms.TextBox screenStepSleep;
        private System.Windows.Forms.TextBox screenStepSize;
        private System.Windows.Forms.Label screenStepSizeLabel;
        private System.Windows.Forms.Label displayLabel;
        private System.Windows.Forms.NumericUpDown displayIndex;
        private System.Windows.Forms.Button manualColorBtn;
    }
}