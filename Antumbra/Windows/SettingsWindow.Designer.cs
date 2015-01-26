namespace Antumbra.Glow.Windows
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pollingX = new MetroFramework.Controls.MetroTextBox();
            this.pollingY = new MetroFramework.Controls.MetroTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.stepSize = new MetroFramework.Controls.MetroTextBox();
            this.fullBtn = new MetroFramework.Controls.MetroButton();
            this.stepSleepLabel = new System.Windows.Forms.Label();
            this.sleepSize = new MetroFramework.Controls.MetroTextBox();
            this.displayLabel = new System.Windows.Forms.Label();
            this.displayIndex = new System.Windows.Forms.NumericUpDown();
            this.settingsStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.ms = new System.Windows.Forms.Label();
            this.DriverLabel = new System.Windows.Forms.Label();
            this.driverExtensions = new System.Windows.Forms.ComboBox();
            this.screenGrabbers = new System.Windows.Forms.ComboBox();
            this.screenGrabberLabel = new System.Windows.Forms.Label();
            this.screenProcessors = new System.Windows.Forms.ComboBox();
            this.screenProcessorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.displayIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(51, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Polling Area:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(154, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(250, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y:";
            // 
            // pollingX
            // 
            this.pollingX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingX.Lines = new string[] {
        "0"};
            this.pollingX.Location = new System.Drawing.Point(184, 148);
            this.pollingX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pollingX.MaxLength = 32767;
            this.pollingX.Name = "pollingX";
            this.pollingX.PasswordChar = '\0';
            this.pollingX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.pollingX.SelectedText = "";
            this.pollingX.Size = new System.Drawing.Size(62, 31);
            this.pollingX.TabIndex = 3;
            this.pollingX.Text = "0";
            this.pollingX.UseSelectable = true;
            this.pollingX.TextChanged += new System.EventHandler(this.pollingX_TextChanged);
            // 
            // pollingY
            // 
            this.pollingY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingY.Lines = new string[] {
        "0"};
            this.pollingY.Location = new System.Drawing.Point(280, 148);
            this.pollingY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pollingY.MaxLength = 32767;
            this.pollingY.Name = "pollingY";
            this.pollingY.PasswordChar = '\0';
            this.pollingY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.pollingY.SelectedText = "";
            this.pollingY.Size = new System.Drawing.Size(72, 31);
            this.pollingY.TabIndex = 4;
            this.pollingY.Text = "0";
            this.pollingY.UseSelectable = true;
            this.pollingY.TextChanged += new System.EventHandler(this.pollingY_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Location = new System.Drawing.Point(46, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(239, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Time Since Last Update (ms): ";
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSizeLabel.Location = new System.Drawing.Point(77, 371);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(86, 20);
            this.stepSizeLabel.TabIndex = 7;
            this.stepSizeLabel.Text = "Step Size:";
            // 
            // stepSize
            // 
            this.stepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSize.Lines = new string[0];
            this.stepSize.Location = new System.Drawing.Point(204, 368);
            this.stepSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stepSize.MaxLength = 32767;
            this.stepSize.Name = "stepSize";
            this.stepSize.PasswordChar = '\0';
            this.stepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.stepSize.SelectedText = "";
            this.stepSize.Size = new System.Drawing.Size(102, 31);
            this.stepSize.TabIndex = 13;
            this.stepSize.UseSelectable = true;
            this.stepSize.TextChanged += new System.EventHandler(this.stepSize_TextChanged);
            // 
            // fullBtn
            // 
            this.fullBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.fullBtn.Location = new System.Drawing.Point(200, 185);
            this.fullBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fullBtn.Name = "fullBtn";
            this.fullBtn.Size = new System.Drawing.Size(123, 29);
            this.fullBtn.TabIndex = 16;
            this.fullBtn.Text = "Full Screen";
            this.fullBtn.UseSelectable = true;
            this.fullBtn.Click += new System.EventHandler(this.fullBtn_Click);
            // 
            // stepSleepLabel
            // 
            this.stepSleepLabel.AutoSize = true;
            this.stepSleepLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSleepLabel.Location = new System.Drawing.Point(55, 420);
            this.stepSleepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stepSleepLabel.Name = "stepSleepLabel";
            this.stepSleepLabel.Size = new System.Drawing.Size(135, 20);
            this.stepSleepLabel.TabIndex = 17;
            this.stepSleepLabel.Text = "Step Sleep: (ms)";
            // 
            // sleepSize
            // 
            this.sleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.sleepSize.Lines = new string[0];
            this.sleepSize.Location = new System.Drawing.Point(204, 420);
            this.sleepSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sleepSize.MaxLength = 32767;
            this.sleepSize.Name = "sleepSize";
            this.sleepSize.PasswordChar = '\0';
            this.sleepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sleepSize.SelectedText = "";
            this.sleepSize.Size = new System.Drawing.Size(102, 31);
            this.sleepSize.TabIndex = 22;
            this.sleepSize.UseSelectable = true;
            this.sleepSize.TextChanged += new System.EventHandler(this.sleepSize_TextChanged);
            // 
            // displayLabel
            // 
            this.displayLabel.AutoSize = true;
            this.displayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.displayLabel.Location = new System.Drawing.Point(56, 109);
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size(70, 20);
            this.displayLabel.TabIndex = 29;
            this.displayLabel.Text = "Display:";
            // 
            // displayIndex
            // 
            this.displayIndex.ForeColor = System.Drawing.Color.Black;
            this.displayIndex.Location = new System.Drawing.Point(159, 109);
            this.displayIndex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.displayIndex.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.displayIndex.Name = "displayIndex";
            this.displayIndex.Size = new System.Drawing.Size(120, 26);
            this.displayIndex.TabIndex = 30;
            this.displayIndex.ValueChanged += new System.EventHandler(this.displayIndex_ValueChanged);
            // 
            // settingsStyleManager
            // 
            this.settingsStyleManager.Owner = this;
            this.settingsStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // ms
            // 
            this.ms.AutoSize = true;
            this.ms.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ms.Location = new System.Drawing.Point(292, 335);
            this.ms.Name = "ms";
            this.ms.Size = new System.Drawing.Size(0, 20);
            this.ms.TabIndex = 31;
            // 
            // DriverLabel
            // 
            this.DriverLabel.AutoSize = true;
            this.DriverLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DriverLabel.Location = new System.Drawing.Point(51, 228);
            this.DriverLabel.Name = "DriverLabel";
            this.DriverLabel.Size = new System.Drawing.Size(128, 20);
            this.DriverLabel.TabIndex = 32;
            this.DriverLabel.Text = "Driver Extension:";
            // 
            // driverExtensions
            // 
            this.driverExtensions.FormattingEnabled = true;
            this.driverExtensions.Location = new System.Drawing.Point(186, 225);
            this.driverExtensions.Name = "driverExtensions";
            this.driverExtensions.Size = new System.Drawing.Size(166, 28);
            this.driverExtensions.TabIndex = 33;
            this.driverExtensions.SelectedIndexChanged += new System.EventHandler(this.driverExtensions_SelectedIndexChanged);
            // 
            // screenGrabbers
            // 
            this.screenGrabbers.FormattingEnabled = true;
            this.screenGrabbers.Location = new System.Drawing.Point(186, 259);
            this.screenGrabbers.Name = "screenGrabbers";
            this.screenGrabbers.Size = new System.Drawing.Size(166, 28);
            this.screenGrabbers.TabIndex = 35;
            this.screenGrabbers.SelectedIndexChanged += new System.EventHandler(this.screenGrabbers_SelectedIndexChanged);
            // 
            // screenGrabberLabel
            // 
            this.screenGrabberLabel.AutoSize = true;
            this.screenGrabberLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenGrabberLabel.Location = new System.Drawing.Point(51, 262);
            this.screenGrabberLabel.Name = "screenGrabberLabel";
            this.screenGrabberLabel.Size = new System.Drawing.Size(127, 20);
            this.screenGrabberLabel.TabIndex = 34;
            this.screenGrabberLabel.Text = "Screen Grabber:";
            // 
            // screenProcessors
            // 
            this.screenProcessors.FormattingEnabled = true;
            this.screenProcessors.Location = new System.Drawing.Point(186, 293);
            this.screenProcessors.Name = "screenProcessors";
            this.screenProcessors.Size = new System.Drawing.Size(166, 28);
            this.screenProcessors.TabIndex = 37;
            this.screenProcessors.SelectedIndexChanged += new System.EventHandler(this.screenProcessors_SelectedIndexChanged);
            // 
            // screenProcessorLabel
            // 
            this.screenProcessorLabel.AutoSize = true;
            this.screenProcessorLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenProcessorLabel.Location = new System.Drawing.Point(39, 296);
            this.screenProcessorLabel.Name = "screenProcessorLabel";
            this.screenProcessorLabel.Size = new System.Drawing.Size(139, 20);
            this.screenProcessorLabel.TabIndex = 36;
            this.screenProcessorLabel.Text = "Screen Processor:";
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 464);
            this.Controls.Add(this.screenProcessors);
            this.Controls.Add(this.screenProcessorLabel);
            this.Controls.Add(this.screenGrabbers);
            this.Controls.Add(this.screenGrabberLabel);
            this.Controls.Add(this.driverExtensions);
            this.Controls.Add(this.DriverLabel);
            this.Controls.Add(this.ms);
            this.Controls.Add(this.displayIndex);
            this.Controls.Add(this.displayLabel);
            this.Controls.Add(this.sleepSize);
            this.Controls.Add(this.stepSleepLabel);
            this.Controls.Add(this.fullBtn);
            this.Controls.Add(this.stepSize);
            this.Controls.Add(this.stepSizeLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pollingY);
            this.Controls.Add(this.pollingX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SettingsWindow";
            this.Opacity = 0.9D;
            this.Padding = new System.Windows.Forms.Padding(30, 92, 30, 31);
            this.Resizable = false;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Text = "Settings";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.displayIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsStyleManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label stepSizeLabel;
        private System.Windows.Forms.Label stepSleepLabel;
        private System.Windows.Forms.Label displayLabel;
        private System.Windows.Forms.NumericUpDown displayIndex;
        private MetroFramework.Controls.MetroTextBox pollingX;
        private MetroFramework.Controls.MetroTextBox pollingY;
        private MetroFramework.Controls.MetroTextBox stepSize;
        private MetroFramework.Controls.MetroButton fullBtn;
        private MetroFramework.Controls.MetroTextBox sleepSize;
        private MetroFramework.Components.MetroStyleManager settingsStyleManager;
        private System.Windows.Forms.Label ms;
        private System.Windows.Forms.ComboBox driverExtensions;
        private System.Windows.Forms.Label DriverLabel;
        private System.Windows.Forms.ComboBox screenProcessors;
        private System.Windows.Forms.Label screenProcessorLabel;
        private System.Windows.Forms.ComboBox screenGrabbers;
        private System.Windows.Forms.Label screenGrabberLabel;
    }
}