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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pollingX = new MetroFramework.Controls.MetroTextBox();
            this.pollingY = new MetroFramework.Controls.MetroTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.screenPollingWait = new MetroFramework.Controls.MetroTextBox();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ManualStepSize = new MetroFramework.Controls.MetroTextBox();
            this.HSVstepSize = new MetroFramework.Controls.MetroTextBox();
            this.colorFadeStepSize = new MetroFramework.Controls.MetroTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.fullBtn = new MetroFramework.Controls.MetroButton();
            this.stepSleepLabel = new System.Windows.Forms.Label();
            this.HSVsleepSizeLabel = new System.Windows.Forms.Label();
            this.ColorFadeSleepSizeLabel = new System.Windows.Forms.Label();
            this.ManualSleepSizeLabel = new System.Windows.Forms.Label();
            this.HSVsleepSize = new MetroFramework.Controls.MetroTextBox();
            this.ColorFadeSleepSize = new MetroFramework.Controls.MetroTextBox();
            this.ManualSleepSize = new MetroFramework.Controls.MetroTextBox();
            this.screenStepSleepLabel = new System.Windows.Forms.Label();
            this.screenStepSleep = new MetroFramework.Controls.MetroTextBox();
            this.screenStepSize = new MetroFramework.Controls.MetroTextBox();
            this.screenStepSizeLabel = new System.Windows.Forms.Label();
            this.displayLabel = new System.Windows.Forms.Label();
            this.displayIndex = new System.Windows.Forms.NumericUpDown();
            this.manualColorBtn = new MetroFramework.Controls.MetroButton();
            this.maxBright = new MetroFramework.Controls.MetroTrackBar();
            this.minBright = new MetroFramework.Controls.MetroTrackBar();
            this.settingsStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.maxBrightLabel = new System.Windows.Forms.Label();
            this.minBrightLabel = new System.Windows.Forms.Label();
            this.saturationAdditiveLabel = new System.Windows.Forms.Label();
            this.saturationEnabledLabel = new System.Windows.Forms.Label();
            this.saturationAdditiveTxt = new MetroFramework.Controls.MetroTextBox();
            this.saturationEnabledCheck = new System.Windows.Forms.CheckBox();
            this.sinSleepLabel = new System.Windows.Forms.Label();
            this.sinStepLabel = new System.Windows.Forms.Label();
            this.sinSleepSize = new MetroFramework.Controls.MetroTextBox();
            this.sinStepSize = new MetroFramework.Controls.MetroTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.displayIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(45, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Polling Area:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(137, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(222, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y:";
            // 
            // pollingX
            // 
            this.pollingX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingX.Lines = new string[] {
        "0"};
            this.pollingX.Location = new System.Drawing.Point(164, 118);
            this.pollingX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pollingX.MaxLength = 32767;
            this.pollingX.Name = "pollingX";
            this.pollingX.PasswordChar = '\0';
            this.pollingX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.pollingX.SelectedText = "";
            this.pollingX.Size = new System.Drawing.Size(55, 25);
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
            this.pollingY.Location = new System.Drawing.Point(249, 118);
            this.pollingY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pollingY.MaxLength = 32767;
            this.pollingY.Name = "pollingY";
            this.pollingY.PasswordChar = '\0';
            this.pollingY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.pollingY.SelectedText = "";
            this.pollingY.Size = new System.Drawing.Size(64, 25);
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
            this.label4.Location = new System.Drawing.Point(55, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(210, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Screen Polling Frequency (ms): ";
            // 
            // screenPollingWait
            // 
            this.screenPollingWait.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenPollingWait.Lines = new string[0];
            this.screenPollingWait.Location = new System.Drawing.Point(268, 188);
            this.screenPollingWait.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.screenPollingWait.MaxLength = 32767;
            this.screenPollingWait.Name = "screenPollingWait";
            this.screenPollingWait.PasswordChar = '\0';
            this.screenPollingWait.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.screenPollingWait.SelectedText = "";
            this.screenPollingWait.Size = new System.Drawing.Size(56, 25);
            this.screenPollingWait.TabIndex = 6;
            this.screenPollingWait.UseSelectable = true;
            this.screenPollingWait.TextChanged += new System.EventHandler(this.screenPollingFq_TextChanged);
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSizeLabel.Location = new System.Drawing.Point(55, 222);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(72, 17);
            this.stepSizeLabel.TabIndex = 7;
            this.stepSizeLabel.Text = "Step Size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label6.Location = new System.Drawing.Point(100, 297);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Manual:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label7.Location = new System.Drawing.Point(116, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "HSV:";
            // 
            // ManualStepSize
            // 
            this.ManualStepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ManualStepSize.Lines = new string[0];
            this.ManualStepSize.Location = new System.Drawing.Point(187, 292);
            this.ManualStepSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ManualStepSize.MaxLength = 32767;
            this.ManualStepSize.Name = "ManualStepSize";
            this.ManualStepSize.PasswordChar = '\0';
            this.ManualStepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ManualStepSize.SelectedText = "";
            this.ManualStepSize.Size = new System.Drawing.Size(91, 25);
            this.ManualStepSize.TabIndex = 12;
            this.ManualStepSize.UseSelectable = true;
            this.ManualStepSize.TextChanged += new System.EventHandler(this.ManualStepSize_TextChanged);
            // 
            // HSVstepSize
            // 
            this.HSVstepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.HSVstepSize.Lines = new string[0];
            this.HSVstepSize.Location = new System.Drawing.Point(187, 240);
            this.HSVstepSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HSVstepSize.MaxLength = 32767;
            this.HSVstepSize.Name = "HSVstepSize";
            this.HSVstepSize.PasswordChar = '\0';
            this.HSVstepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.HSVstepSize.SelectedText = "";
            this.HSVstepSize.Size = new System.Drawing.Size(91, 25);
            this.HSVstepSize.TabIndex = 13;
            this.HSVstepSize.UseSelectable = true;
            this.HSVstepSize.TextChanged += new System.EventHandler(this.HSVstepSize_TextChanged);
            // 
            // colorFadeStepSize
            // 
            this.colorFadeStepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.colorFadeStepSize.Lines = new string[0];
            this.colorFadeStepSize.Location = new System.Drawing.Point(187, 266);
            this.colorFadeStepSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.colorFadeStepSize.MaxLength = 32767;
            this.colorFadeStepSize.Name = "colorFadeStepSize";
            this.colorFadeStepSize.PasswordChar = '\0';
            this.colorFadeStepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.colorFadeStepSize.SelectedText = "";
            this.colorFadeStepSize.Size = new System.Drawing.Size(91, 25);
            this.colorFadeStepSize.TabIndex = 14;
            this.colorFadeStepSize.UseSelectable = true;
            this.colorFadeStepSize.TextChanged += new System.EventHandler(this.colorFadeStepSize_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label8.Location = new System.Drawing.Point(77, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Color Fade:";
            // 
            // fullBtn
            // 
            this.fullBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.fullBtn.Location = new System.Drawing.Point(178, 148);
            this.fullBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fullBtn.Name = "fullBtn";
            this.fullBtn.Size = new System.Drawing.Size(109, 23);
            this.fullBtn.TabIndex = 16;
            this.fullBtn.Text = "Full Screen";
            this.fullBtn.UseSelectable = true;
            this.fullBtn.UseVisualStyleBackColor = true;
            this.fullBtn.Click += new System.EventHandler(this.fullBtn_Click);
            // 
            // stepSleepLabel
            // 
            this.stepSleepLabel.AutoSize = true;
            this.stepSleepLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSleepLabel.Location = new System.Drawing.Point(59, 404);
            this.stepSleepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stepSleepLabel.Name = "stepSleepLabel";
            this.stepSleepLabel.Size = new System.Drawing.Size(113, 17);
            this.stepSleepLabel.TabIndex = 17;
            this.stepSleepLabel.Text = "Step Sleep: (ms)";
            // 
            // HSVsleepSizeLabel
            // 
            this.HSVsleepSizeLabel.AutoSize = true;
            this.HSVsleepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.HSVsleepSizeLabel.Location = new System.Drawing.Point(116, 428);
            this.HSVsleepSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HSVsleepSizeLabel.Name = "HSVsleepSizeLabel";
            this.HSVsleepSizeLabel.Size = new System.Drawing.Size(40, 17);
            this.HSVsleepSizeLabel.TabIndex = 18;
            this.HSVsleepSizeLabel.Text = "HSV:";
            // 
            // ColorFadeSleepSizeLabel
            // 
            this.ColorFadeSleepSizeLabel.AutoSize = true;
            this.ColorFadeSleepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ColorFadeSleepSizeLabel.Location = new System.Drawing.Point(77, 454);
            this.ColorFadeSleepSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ColorFadeSleepSizeLabel.Name = "ColorFadeSleepSizeLabel";
            this.ColorFadeSleepSizeLabel.Size = new System.Drawing.Size(81, 17);
            this.ColorFadeSleepSizeLabel.TabIndex = 20;
            this.ColorFadeSleepSizeLabel.Text = "Color Fade:";
            // 
            // ManualSleepSizeLabel
            // 
            this.ManualSleepSizeLabel.AutoSize = true;
            this.ManualSleepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ManualSleepSizeLabel.Location = new System.Drawing.Point(100, 481);
            this.ManualSleepSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ManualSleepSizeLabel.Name = "ManualSleepSizeLabel";
            this.ManualSleepSizeLabel.Size = new System.Drawing.Size(58, 17);
            this.ManualSleepSizeLabel.TabIndex = 21;
            this.ManualSleepSizeLabel.Text = "Manual:";
            // 
            // HSVsleepSize
            // 
            this.HSVsleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.HSVsleepSize.Lines = new string[0];
            this.HSVsleepSize.Location = new System.Drawing.Point(187, 423);
            this.HSVsleepSize.Margin = new System.Windows.Forms.Padding(4);
            this.HSVsleepSize.MaxLength = 32767;
            this.HSVsleepSize.Name = "HSVsleepSize";
            this.HSVsleepSize.PasswordChar = '\0';
            this.HSVsleepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.HSVsleepSize.SelectedText = "";
            this.HSVsleepSize.Size = new System.Drawing.Size(91, 25);
            this.HSVsleepSize.TabIndex = 22;
            this.HSVsleepSize.UseSelectable = true;
            this.HSVsleepSize.TextChanged += new System.EventHandler(this.HSVsleepSize_TextChanged);
            // 
            // ColorFadeSleepSize
            // 
            this.ColorFadeSleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ColorFadeSleepSize.Lines = new string[0];
            this.ColorFadeSleepSize.Location = new System.Drawing.Point(187, 449);
            this.ColorFadeSleepSize.Margin = new System.Windows.Forms.Padding(4);
            this.ColorFadeSleepSize.MaxLength = 32767;
            this.ColorFadeSleepSize.Name = "ColorFadeSleepSize";
            this.ColorFadeSleepSize.PasswordChar = '\0';
            this.ColorFadeSleepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ColorFadeSleepSize.SelectedText = "";
            this.ColorFadeSleepSize.Size = new System.Drawing.Size(91, 25);
            this.ColorFadeSleepSize.TabIndex = 23;
            this.ColorFadeSleepSize.UseSelectable = true;
            this.ColorFadeSleepSize.TextChanged += new System.EventHandler(this.ColorFadeSleepSize_TextChanged);
            // 
            // ManualSleepSize
            // 
            this.ManualSleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ManualSleepSize.Lines = new string[0];
            this.ManualSleepSize.Location = new System.Drawing.Point(187, 475);
            this.ManualSleepSize.Margin = new System.Windows.Forms.Padding(4);
            this.ManualSleepSize.MaxLength = 32767;
            this.ManualSleepSize.Name = "ManualSleepSize";
            this.ManualSleepSize.PasswordChar = '\0';
            this.ManualSleepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ManualSleepSize.SelectedText = "";
            this.ManualSleepSize.Size = new System.Drawing.Size(91, 25);
            this.ManualSleepSize.TabIndex = 24;
            this.ManualSleepSize.UseSelectable = true;
            this.ManualSleepSize.TextChanged += new System.EventHandler(this.ManualSleepSize_TextChanged);
            // 
            // screenStepSleepLabel
            // 
            this.screenStepSleepLabel.AutoSize = true;
            this.screenStepSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenStepSleepLabel.Location = new System.Drawing.Point(21, 506);
            this.screenStepSleepLabel.Name = "screenStepSleepLabel";
            this.screenStepSleepLabel.Size = new System.Drawing.Size(135, 17);
            this.screenStepSleepLabel.TabIndex = 25;
            this.screenStepSleepLabel.Text = "Screen Responsive:";
            // 
            // screenStepSleep
            // 
            this.screenStepSleep.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenStepSleep.Lines = new string[0];
            this.screenStepSleep.Location = new System.Drawing.Point(187, 501);
            this.screenStepSleep.Margin = new System.Windows.Forms.Padding(4);
            this.screenStepSleep.MaxLength = 32767;
            this.screenStepSleep.Name = "screenStepSleep";
            this.screenStepSleep.PasswordChar = '\0';
            this.screenStepSleep.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.screenStepSleep.SelectedText = "";
            this.screenStepSleep.Size = new System.Drawing.Size(91, 25);
            this.screenStepSleep.TabIndex = 26;
            this.screenStepSleep.UseSelectable = true;
            this.screenStepSleep.TextChanged += new System.EventHandler(this.screenStepSleep_TextChanged);
            // 
            // screenStepSize
            // 
            this.screenStepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenStepSize.Lines = new string[0];
            this.screenStepSize.Location = new System.Drawing.Point(187, 318);
            this.screenStepSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.screenStepSize.MaxLength = 32767;
            this.screenStepSize.Name = "screenStepSize";
            this.screenStepSize.PasswordChar = '\0';
            this.screenStepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.screenStepSize.SelectedText = "";
            this.screenStepSize.Size = new System.Drawing.Size(91, 25);
            this.screenStepSize.TabIndex = 28;
            this.screenStepSize.UseSelectable = true;
            this.screenStepSize.TextChanged += new System.EventHandler(this.screenStepSize_TextChanged);
            // 
            // screenStepSizeLabel
            // 
            this.screenStepSizeLabel.AutoSize = true;
            this.screenStepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenStepSizeLabel.Location = new System.Drawing.Point(24, 320);
            this.screenStepSizeLabel.Name = "screenStepSizeLabel";
            this.screenStepSizeLabel.Size = new System.Drawing.Size(135, 17);
            this.screenStepSizeLabel.TabIndex = 27;
            this.screenStepSizeLabel.Text = "Screen Responsive:";
            // 
            // displayLabel
            // 
            this.displayLabel.AutoSize = true;
            this.displayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.displayLabel.Location = new System.Drawing.Point(50, 87);
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size(58, 17);
            this.displayLabel.TabIndex = 29;
            this.displayLabel.Text = "Display:";
            // 
            // displayIndex
            // 
            this.displayIndex.ForeColor = System.Drawing.Color.Black;
            this.displayIndex.Location = new System.Drawing.Point(141, 87);
            this.displayIndex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.displayIndex.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.displayIndex.Name = "displayIndex";
            this.displayIndex.Size = new System.Drawing.Size(107, 22);
            this.displayIndex.TabIndex = 30;
            this.displayIndex.ValueChanged += new System.EventHandler(this.displayIndex_ValueChanged);
            // 
            // manualColorBtn
            // 
            this.manualColorBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.manualColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manualColorBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.manualColorBtn.Location = new System.Drawing.Point(82, 727);
            this.manualColorBtn.Margin = new System.Windows.Forms.Padding(4);
            this.manualColorBtn.Name = "manualColorBtn";
            this.manualColorBtn.Size = new System.Drawing.Size(216, 66);
            this.manualColorBtn.TabIndex = 31;
            this.manualColorBtn.UseSelectable = true;
            this.manualColorBtn.UseVisualStyleBackColor = true;
            this.manualColorBtn.Click += new System.EventHandler(this.manualColorBtn_Click);
            // 
            // maxBright
            // 
            this.maxBright.BackColor = System.Drawing.Color.Transparent;
            this.maxBright.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.maxBright.Location = new System.Drawing.Point(82, 801);
            this.maxBright.Margin = new System.Windows.Forms.Padding(4);
            this.maxBright.Name = "maxBright";
            this.maxBright.Size = new System.Drawing.Size(216, 28);
            this.maxBright.TabIndex = 32;
            this.maxBright.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.maxBright.Scroll += new System.Windows.Forms.ScrollEventHandler(this.maxBright_Scroll);
            // 
            // minBright
            // 
            this.minBright.BackColor = System.Drawing.Color.Transparent;
            this.minBright.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.minBright.Location = new System.Drawing.Point(82, 837);
            this.minBright.Margin = new System.Windows.Forms.Padding(4);
            this.minBright.Name = "minBright";
            this.minBright.Size = new System.Drawing.Size(216, 28);
            this.minBright.TabIndex = 33;
            this.minBright.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.minBright.Scroll += new System.Windows.Forms.ScrollEventHandler(this.minBright_Scroll);
            // 
            // settingsStyleManager
            // 
            this.settingsStyleManager.Owner = this;
            this.settingsStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // maxBrightLabel
            // 
            this.maxBrightLabel.AutoSize = true;
            this.maxBrightLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.maxBrightLabel.Location = new System.Drawing.Point(35, 807);
            this.maxBrightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.maxBrightLabel.Name = "maxBrightLabel";
            this.maxBrightLabel.Size = new System.Drawing.Size(37, 17);
            this.maxBrightLabel.TabIndex = 34;
            this.maxBrightLabel.Text = "Max:";
            // 
            // minBrightLabel
            // 
            this.minBrightLabel.AutoSize = true;
            this.minBrightLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.minBrightLabel.Location = new System.Drawing.Point(39, 841);
            this.minBrightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.minBrightLabel.Name = "minBrightLabel";
            this.minBrightLabel.Size = new System.Drawing.Size(34, 17);
            this.minBrightLabel.TabIndex = 35;
            this.minBrightLabel.Text = "Min:";
            // 
            // saturationAdditiveLabel
            // 
            this.saturationAdditiveLabel.AutoSize = true;
            this.saturationAdditiveLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.saturationAdditiveLabel.Location = new System.Drawing.Point(41, 696);
            this.saturationAdditiveLabel.Name = "saturationAdditiveLabel";
            this.saturationAdditiveLabel.Size = new System.Drawing.Size(131, 17);
            this.saturationAdditiveLabel.TabIndex = 36;
            this.saturationAdditiveLabel.Text = "Saturation Additive:";
            // 
            // saturationEnabledLabel
            // 
            this.saturationEnabledLabel.AutoSize = true;
            this.saturationEnabledLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.saturationEnabledLabel.Location = new System.Drawing.Point(37, 668);
            this.saturationEnabledLabel.Name = "saturationEnabledLabel";
            this.saturationEnabledLabel.Size = new System.Drawing.Size(137, 17);
            this.saturationEnabledLabel.TabIndex = 37;
            this.saturationEnabledLabel.Text = "Saturation Enabled?";
            // 
            // saturationAdditiveTxt
            // 
            this.saturationAdditiveTxt.Lines = new string[0];
            this.saturationAdditiveTxt.Location = new System.Drawing.Point(187, 693);
            this.saturationAdditiveTxt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.saturationAdditiveTxt.MaxLength = 32767;
            this.saturationAdditiveTxt.Name = "saturationAdditiveTxt";
            this.saturationAdditiveTxt.PasswordChar = '\0';
            this.saturationAdditiveTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.saturationAdditiveTxt.SelectedText = "";
            this.saturationAdditiveTxt.Size = new System.Drawing.Size(91, 25);
            this.saturationAdditiveTxt.TabIndex = 39;
            this.saturationAdditiveTxt.UseSelectable = true;
            this.saturationAdditiveTxt.TextChanged += new System.EventHandler(this.saturationAdditiveTxt_TextChanged);
            // 
            // saturationEnabledCheck
            // 
            this.saturationEnabledCheck.AutoSize = true;
            this.saturationEnabledCheck.BackColor = System.Drawing.Color.Transparent;
            this.saturationEnabledCheck.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.saturationEnabledCheck.Checked = true;
            this.saturationEnabledCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saturationEnabledCheck.FlatAppearance.BorderSize = 0;
            this.saturationEnabledCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saturationEnabledCheck.Location = new System.Drawing.Point(226, 669);
            this.saturationEnabledCheck.Margin = new System.Windows.Forms.Padding(0);
            this.saturationEnabledCheck.Name = "saturationEnabledCheck";
            this.saturationEnabledCheck.Size = new System.Drawing.Size(14, 13);
            this.saturationEnabledCheck.TabIndex = 40;
            this.saturationEnabledCheck.UseVisualStyleBackColor = false;
            this.saturationEnabledCheck.CheckedChanged += new System.EventHandler(this.saturationEnabledCheck_CheckedChanged);
            // 
            // sinSleepLabel
            // 
            this.sinSleepLabel.AutoSize = true;
            this.sinSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.sinSleepLabel.Location = new System.Drawing.Point(76, 532);
            this.sinSleepLabel.Name = "sinSleepLabel";
            this.sinSleepLabel.Size = new System.Drawing.Size(80, 17);
            this.sinSleepLabel.TabIndex = 41;
            this.sinSleepLabel.Text = "Sine Wave:";
            // 
            // sinStepLabel
            // 
            this.sinStepLabel.AutoSize = true;
            this.sinStepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.sinStepLabel.Location = new System.Drawing.Point(76, 348);
            this.sinStepLabel.Name = "sinStepLabel";
            this.sinStepLabel.Size = new System.Drawing.Size(80, 17);
            this.sinStepLabel.TabIndex = 42;
            this.sinStepLabel.Text = "Sine Wave:";
            this.sinStepLabel.Click += new System.EventHandler(this.label9_Click);
            // 
            // sinSleepSize
            // 
            this.sinSleepSize.Lines = new string[0];
            this.sinSleepSize.Location = new System.Drawing.Point(187, 527);
            this.sinSleepSize.MaxLength = 32767;
            this.sinSleepSize.Name = "sinSleepSize";
            this.sinSleepSize.PasswordChar = '\0';
            this.sinSleepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sinSleepSize.SelectedText = "";
            this.sinSleepSize.Size = new System.Drawing.Size(91, 25);
            this.sinSleepSize.TabIndex = 43;
            this.sinSleepSize.UseSelectable = true;
            // 
            // sinStepSize
            // 
            this.sinStepSize.Lines = new string[0];
            this.sinStepSize.Location = new System.Drawing.Point(187, 344);
            this.sinStepSize.MaxLength = 32767;
            this.sinStepSize.Name = "sinStepSize";
            this.sinStepSize.PasswordChar = '\0';
            this.sinStepSize.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sinStepSize.SelectedText = "";
            this.sinStepSize.Size = new System.Drawing.Size(91, 25);
            this.sinStepSize.TabIndex = 44;
            this.sinStepSize.UseSelectable = true;
            this.sinStepSize.TextChanged += new System.EventHandler(this.sinStepSize_TextChanged);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 914);
            this.Controls.Add(this.sinStepSize);
            this.Controls.Add(this.sinSleepSize);
            this.Controls.Add(this.sinStepLabel);
            this.Controls.Add(this.sinSleepLabel);
            this.Controls.Add(this.saturationEnabledCheck);
            this.Controls.Add(this.saturationAdditiveTxt);
            this.Controls.Add(this.saturationEnabledLabel);
            this.Controls.Add(this.saturationAdditiveLabel);
            this.Controls.Add(this.minBrightLabel);
            this.Controls.Add(this.maxBrightLabel);
            this.Controls.Add(this.minBright);
            this.Controls.Add(this.maxBright);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SettingsWindow";
            this.Opacity = 0.9D;
            this.Padding = new System.Windows.Forms.Padding(27, 74, 27, 25);
            this.Resizable = false;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Text = "Settings";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label stepSleepLabel;
        private System.Windows.Forms.Label HSVsleepSizeLabel;
        private System.Windows.Forms.Label ColorFadeSleepSizeLabel;
        private System.Windows.Forms.Label ManualSleepSizeLabel;
        private System.Windows.Forms.Label screenStepSleepLabel;
        private System.Windows.Forms.Label screenStepSizeLabel;
        private System.Windows.Forms.Label displayLabel;
        private System.Windows.Forms.NumericUpDown displayIndex;
        private MetroFramework.Controls.MetroTrackBar maxBright;
        private MetroFramework.Controls.MetroTrackBar minBright;
        private MetroFramework.Controls.MetroTextBox pollingX;
        private MetroFramework.Controls.MetroTextBox pollingY;
        private MetroFramework.Controls.MetroTextBox screenPollingWait;
        private MetroFramework.Controls.MetroTextBox ManualStepSize;
        private MetroFramework.Controls.MetroTextBox HSVstepSize;
        private MetroFramework.Controls.MetroTextBox colorFadeStepSize;
        private MetroFramework.Controls.MetroButton fullBtn;
        private MetroFramework.Controls.MetroTextBox HSVsleepSize;
        private MetroFramework.Controls.MetroTextBox ColorFadeSleepSize;
        private MetroFramework.Controls.MetroTextBox ManualSleepSize;
        private MetroFramework.Controls.MetroTextBox screenStepSleep;
        private MetroFramework.Controls.MetroTextBox screenStepSize;
        private MetroFramework.Controls.MetroButton manualColorBtn;
        private MetroFramework.Components.MetroStyleManager settingsStyleManager;
        private System.Windows.Forms.Label minBrightLabel;
        private System.Windows.Forms.Label maxBrightLabel;
        private System.Windows.Forms.Label saturationAdditiveLabel;
        private System.Windows.Forms.Label saturationEnabledLabel;
        private System.Windows.Forms.CheckBox saturationEnabledCheck;
        private System.Windows.Forms.Label sinStepLabel;
        private System.Windows.Forms.Label sinSleepLabel;
        private MetroFramework.Controls.MetroTextBox saturationAdditiveTxt;
        private MetroFramework.Controls.MetroTextBox sinStepSize;
        private MetroFramework.Controls.MetroTextBox sinSleepSize;
    }
}