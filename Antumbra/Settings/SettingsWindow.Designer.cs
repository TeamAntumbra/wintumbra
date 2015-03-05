namespace Antumbra.Glow.Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.stepSleepLabel = new System.Windows.Forms.Label();
            this.sleepSize = new System.Windows.Forms.TextBox();
            this.metroLabel1 = new System.Windows.Forms.Label();
            this.pollingArea = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pollingXLabel = new System.Windows.Forms.Label();
            this.pollingX = new System.Windows.Forms.Label();
            this.pollingYLabel = new System.Windows.Forms.Label();
            this.pollingY = new System.Windows.Forms.Label();
            this.pollingWidthLabel = new System.Windows.Forms.Label();
            this.pollingHeightLabel = new System.Windows.Forms.Label();
            this.pollingHeight = new System.Windows.Forms.Label();
            this.pollingWidth = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.glowStatus = new System.Windows.Forms.Label();
            this.startBtn = new System.Windows.Forms.Button();
            this.offBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.settingsTitle = new System.Windows.Forms.Label();
            this.newColorWeight = new System.Windows.Forms.TextBox();
            this.weightingLabel = new System.Windows.Forms.Label();
            this.weightingEnabled = new System.Windows.Forms.CheckBox();
            this.deviceNameLabel = new System.Windows.Forms.Label();
            this.deviceName = new System.Windows.Forms.Label();
            this.driverRecBtn = new System.Windows.Forms.Button();
            this.compoundDecorationCheck = new System.Windows.Forms.CheckBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.aboutPage = new FlatTabControl.FlatTabControl();
            this.driverPage = new System.Windows.Forms.TabPage();
            this.instructions = new System.Windows.Forms.Label();
            this.processorSettingsBtn = new System.Windows.Forms.PictureBox();
            this.processorComboBx = new System.Windows.Forms.ComboBox();
            this.processorLabel = new System.Windows.Forms.Label();
            this.grabberSettingsBtn = new System.Windows.Forms.PictureBox();
            this.grabberComboBx = new System.Windows.Forms.ComboBox();
            this.grabberLabel = new System.Windows.Forms.Label();
            this.driverSettingsBtn = new System.Windows.Forms.PictureBox();
            this.driverComboBox = new System.Windows.Forms.ComboBox();
            this.driverLabel = new System.Windows.Forms.Label();
            this.decoratorPage = new System.Windows.Forms.TabPage();
            this.toggleActiveBtn = new System.Windows.Forms.Button();
            this.currentDecStatus = new System.Windows.Forms.Label();
            this.currentDecStatusLabel = new System.Windows.Forms.Label();
            this.decoratorSettingsBtn = new System.Windows.Forms.PictureBox();
            this.decoratorComboBx = new System.Windows.Forms.ComboBox();
            this.decoratorLabel = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.aboutPage.SuspendLayout();
            this.driverPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processorSettingsBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grabberSettingsBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driverSettingsBtn)).BeginInit();
            this.decoratorPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decoratorSettingsBtn)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // stepSleepLabel
            // 
            this.stepSleepLabel.AutoSize = true;
            this.stepSleepLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSleepLabel.Location = new System.Drawing.Point(340, 595);
            this.stepSleepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stepSleepLabel.Name = "stepSleepLabel";
            this.stepSleepLabel.Size = new System.Drawing.Size(107, 16);
            this.stepSleepLabel.TabIndex = 17;
            this.stepSleepLabel.Text = "Step Sleep: (ms)";
            // 
            // sleepSize
            // 
            this.sleepSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.sleepSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.sleepSize.Location = new System.Drawing.Point(455, 593);
            this.sleepSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sleepSize.Name = "sleepSize";
            this.sleepSize.Size = new System.Drawing.Size(102, 20);
            this.sleepSize.TabIndex = 22;
            this.sleepSize.TextChanged += new System.EventHandler(this.sleepSize_TextChanged);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.metroLabel1.Location = new System.Drawing.Point(96, 363);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(61, 19);
            this.metroLabel1.TabIndex = 40;
            this.metroLabel1.Text = "Notifiers:";
            // 
            // pollingArea
            // 
            this.pollingArea.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.pollingArea.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pollingArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pollingArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingArea.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingArea.Location = new System.Drawing.Point(632, 126);
            this.pollingArea.Name = "pollingArea";
            this.pollingArea.Size = new System.Drawing.Size(181, 35);
            this.pollingArea.TabIndex = 51;
            this.pollingArea.Text = "Set Screen Grabber Area";
            this.pollingArea.UseVisualStyleBackColor = false;
            this.pollingArea.Click += new System.EventHandler(this.pollingArea_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(235, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 17);
            this.label1.TabIndex = 52;
            this.label1.Text = "Polling Location:";
            // 
            // pollingXLabel
            // 
            this.pollingXLabel.AutoSize = true;
            this.pollingXLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingXLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingXLabel.Location = new System.Drawing.Point(189, 112);
            this.pollingXLabel.Name = "pollingXLabel";
            this.pollingXLabel.Size = new System.Drawing.Size(19, 16);
            this.pollingXLabel.TabIndex = 53;
            this.pollingXLabel.Text = "X:";
            // 
            // pollingX
            // 
            this.pollingX.AutoSize = true;
            this.pollingX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingX.Location = new System.Drawing.Point(211, 112);
            this.pollingX.Name = "pollingX";
            this.pollingX.Size = new System.Drawing.Size(0, 16);
            this.pollingX.TabIndex = 54;
            // 
            // pollingYLabel
            // 
            this.pollingYLabel.AutoSize = true;
            this.pollingYLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingYLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingYLabel.Location = new System.Drawing.Point(189, 145);
            this.pollingYLabel.Name = "pollingYLabel";
            this.pollingYLabel.Size = new System.Drawing.Size(20, 16);
            this.pollingYLabel.TabIndex = 55;
            this.pollingYLabel.Text = "Y:";
            // 
            // pollingY
            // 
            this.pollingY.AutoSize = true;
            this.pollingY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingY.Location = new System.Drawing.Point(211, 145);
            this.pollingY.Name = "pollingY";
            this.pollingY.Size = new System.Drawing.Size(0, 16);
            this.pollingY.TabIndex = 56;
            // 
            // pollingWidthLabel
            // 
            this.pollingWidthLabel.AutoSize = true;
            this.pollingWidthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingWidthLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingWidthLabel.Location = new System.Drawing.Point(278, 112);
            this.pollingWidthLabel.Name = "pollingWidthLabel";
            this.pollingWidthLabel.Size = new System.Drawing.Size(45, 16);
            this.pollingWidthLabel.TabIndex = 57;
            this.pollingWidthLabel.Text = "Width:";
            // 
            // pollingHeightLabel
            // 
            this.pollingHeightLabel.AutoSize = true;
            this.pollingHeightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingHeightLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingHeightLabel.Location = new System.Drawing.Point(278, 145);
            this.pollingHeightLabel.Name = "pollingHeightLabel";
            this.pollingHeightLabel.Size = new System.Drawing.Size(50, 16);
            this.pollingHeightLabel.TabIndex = 58;
            this.pollingHeightLabel.Text = "Height:";
            // 
            // pollingHeight
            // 
            this.pollingHeight.AutoSize = true;
            this.pollingHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingHeight.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingHeight.Location = new System.Drawing.Point(325, 145);
            this.pollingHeight.Name = "pollingHeight";
            this.pollingHeight.Size = new System.Drawing.Size(0, 16);
            this.pollingHeight.TabIndex = 60;
            // 
            // pollingWidth
            // 
            this.pollingWidth.AutoSize = true;
            this.pollingWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.pollingWidth.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingWidth.Location = new System.Drawing.Point(322, 112);
            this.pollingWidth.Name = "pollingWidth";
            this.pollingWidth.Size = new System.Drawing.Size(0, 16);
            this.pollingWidth.TabIndex = 61;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.statusLabel.Location = new System.Drawing.Point(575, 595);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(81, 16);
            this.statusLabel.TabIndex = 64;
            this.statusLabel.Text = "Glow Status:";
            // 
            // glowStatus
            // 
            this.glowStatus.AutoSize = true;
            this.glowStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.glowStatus.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.glowStatus.Location = new System.Drawing.Point(662, 595);
            this.glowStatus.Name = "glowStatus";
            this.glowStatus.Size = new System.Drawing.Size(0, 16);
            this.glowStatus.TabIndex = 65;
            // 
            // startBtn
            // 
            this.startBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.startBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.startBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.startBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.startBtn.Location = new System.Drawing.Point(371, 644);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(76, 29);
            this.startBtn.TabIndex = 66;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = false;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // offBtn
            // 
            this.offBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.offBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.offBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.offBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.offBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.offBtn.Location = new System.Drawing.Point(537, 644);
            this.offBtn.Name = "offBtn";
            this.offBtn.Size = new System.Drawing.Size(76, 29);
            this.offBtn.TabIndex = 67;
            this.offBtn.Text = "Off";
            this.offBtn.UseVisualStyleBackColor = false;
            this.offBtn.Click += new System.EventHandler(this.offBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.stopBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.stopBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.stopBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stopBtn.Location = new System.Drawing.Point(455, 644);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(76, 29);
            this.stopBtn.TabIndex = 68;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = false;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.AutoSize = true;
            this.closeBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.closeBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.closeBtn.Location = new System.Drawing.Point(873, -2);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(26, 25);
            this.closeBtn.TabIndex = 72;
            this.closeBtn.Text = "X";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // settingsTitle
            // 
            this.settingsTitle.AutoSize = true;
            this.settingsTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.settingsTitle.Location = new System.Drawing.Point(462, 22);
            this.settingsTitle.Name = "settingsTitle";
            this.settingsTitle.Size = new System.Drawing.Size(91, 26);
            this.settingsTitle.TabIndex = 73;
            this.settingsTitle.Text = "Settings";
            this.settingsTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // newColorWeight
            // 
            this.newColorWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.newColorWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newColorWeight.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.newColorWeight.Location = new System.Drawing.Point(228, 595);
            this.newColorWeight.Name = "newColorWeight";
            this.newColorWeight.Size = new System.Drawing.Size(102, 20);
            this.newColorWeight.TabIndex = 77;
            this.newColorWeight.TextChanged += new System.EventHandler(this.newColorWeight_TextChanged);
            // 
            // weightingLabel
            // 
            this.weightingLabel.AutoSize = true;
            this.weightingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weightingLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.weightingLabel.Location = new System.Drawing.Point(49, 597);
            this.weightingLabel.Name = "weightingLabel";
            this.weightingLabel.Size = new System.Drawing.Size(173, 16);
            this.weightingLabel.TabIndex = 76;
            this.weightingLabel.Text = "New Color Weight: (0-100%)";
            // 
            // weightingEnabled
            // 
            this.weightingEnabled.AutoSize = true;
            this.weightingEnabled.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.weightingEnabled.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.weightingEnabled.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.weightingEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.weightingEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weightingEnabled.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.weightingEnabled.Location = new System.Drawing.Point(632, 621);
            this.weightingEnabled.Name = "weightingEnabled";
            this.weightingEnabled.Size = new System.Drawing.Size(198, 20);
            this.weightingEnabled.TabIndex = 78;
            this.weightingEnabled.Text = "Weighted Average Enabled?";
            this.weightingEnabled.UseVisualStyleBackColor = false;
            this.weightingEnabled.CheckedChanged += new System.EventHandler(this.weightingEnabled_CheckedChanged);
            // 
            // deviceNameLabel
            // 
            this.deviceNameLabel.AutoSize = true;
            this.deviceNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.deviceNameLabel.Location = new System.Drawing.Point(655, 92);
            this.deviceNameLabel.Name = "deviceNameLabel";
            this.deviceNameLabel.Size = new System.Drawing.Size(70, 16);
            this.deviceNameLabel.TabIndex = 79;
            this.deviceNameLabel.Text = "Device ID:";
            // 
            // deviceName
            // 
            this.deviceName.AutoSize = true;
            this.deviceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.deviceName.Location = new System.Drawing.Point(735, 92);
            this.deviceName.Name = "deviceName";
            this.deviceName.Size = new System.Drawing.Size(0, 16);
            this.deviceName.TabIndex = 80;
            // 
            // driverRecBtn
            // 
            this.driverRecBtn.AutoSize = true;
            this.driverRecBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.driverRecBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.driverRecBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverRecBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.driverRecBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.driverRecBtn.Location = new System.Drawing.Point(80, 644);
            this.driverRecBtn.Margin = new System.Windows.Forms.Padding(0);
            this.driverRecBtn.Name = "driverRecBtn";
            this.driverRecBtn.Size = new System.Drawing.Size(242, 28);
            this.driverRecBtn.TabIndex = 81;
            this.driverRecBtn.Text = "Apply Driver Recommended Settings";
            this.driverRecBtn.UseVisualStyleBackColor = false;
            this.driverRecBtn.Click += new System.EventHandler(this.driverRecBtn_Click);
            // 
            // compoundDecorationCheck
            // 
            this.compoundDecorationCheck.AutoSize = true;
            this.compoundDecorationCheck.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.compoundDecorationCheck.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.compoundDecorationCheck.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.compoundDecorationCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.compoundDecorationCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.compoundDecorationCheck.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.compoundDecorationCheck.Location = new System.Drawing.Point(632, 647);
            this.compoundDecorationCheck.Name = "compoundDecorationCheck";
            this.compoundDecorationCheck.Size = new System.Drawing.Size(326, 36);
            this.compoundDecorationCheck.TabIndex = 85;
            this.compoundDecorationCheck.Text = "Compound Decoration \r\n(Each Decorator Acts on the Output of the Previous)";
            this.compoundDecorationCheck.UseVisualStyleBackColor = false;
            this.compoundDecorationCheck.CheckedChanged += new System.EventHandler(this.compoundDecorationCheck_CheckedChanged);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(552, 34);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 13);
            this.versionLabel.TabIndex = 91;
            // 
            // aboutPage
            // 
            this.aboutPage.Controls.Add(this.driverPage);
            this.aboutPage.Controls.Add(this.decoratorPage);
            this.aboutPage.Controls.Add(this.tabPage3);
            this.aboutPage.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.aboutPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.aboutPage.HotTrack = true;
            this.aboutPage.Location = new System.Drawing.Point(55, 192);
            this.aboutPage.Margin = new System.Windows.Forms.Padding(2);
            this.aboutPage.myBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.aboutPage.Name = "aboutPage";
            this.aboutPage.SelectedIndex = 0;
            this.aboutPage.Size = new System.Drawing.Size(839, 380);
            this.aboutPage.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.aboutPage.TabIndex = 92;
            // 
            // driverPage
            // 
            this.driverPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.driverPage.Controls.Add(this.instructions);
            this.driverPage.Controls.Add(this.processorSettingsBtn);
            this.driverPage.Controls.Add(this.processorComboBx);
            this.driverPage.Controls.Add(this.processorLabel);
            this.driverPage.Controls.Add(this.grabberSettingsBtn);
            this.driverPage.Controls.Add(this.grabberComboBx);
            this.driverPage.Controls.Add(this.grabberLabel);
            this.driverPage.Controls.Add(this.driverSettingsBtn);
            this.driverPage.Controls.Add(this.driverComboBox);
            this.driverPage.Controls.Add(this.driverLabel);
            this.driverPage.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.driverPage.Location = new System.Drawing.Point(4, 25);
            this.driverPage.Margin = new System.Windows.Forms.Padding(2);
            this.driverPage.Name = "driverPage";
            this.driverPage.Padding = new System.Windows.Forms.Padding(2);
            this.driverPage.Size = new System.Drawing.Size(831, 351);
            this.driverPage.TabIndex = 0;
            this.driverPage.Text = "Driver";
            // 
            // instructions
            // 
            this.instructions.AutoSize = true;
            this.instructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructions.Location = new System.Drawing.Point(196, 32);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(395, 16);
            this.instructions.TabIndex = 9;
            this.instructions.Text = "Select which extension you would like to control your Glow device.";
            // 
            // processorSettingsBtn
            // 
            this.processorSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.processorSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.processorSettingsBtn.Location = new System.Drawing.Point(573, 167);
            this.processorSettingsBtn.Name = "processorSettingsBtn";
            this.processorSettingsBtn.Size = new System.Drawing.Size(24, 21);
            this.processorSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.processorSettingsBtn.TabIndex = 8;
            this.processorSettingsBtn.TabStop = false;
            // 
            // processorComboBx
            // 
            this.processorComboBx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.processorComboBx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.processorComboBx.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.processorComboBx.FormattingEnabled = true;
            this.processorComboBx.Location = new System.Drawing.Point(222, 167);
            this.processorComboBx.Name = "processorComboBx";
            this.processorComboBx.Size = new System.Drawing.Size(345, 23);
            this.processorComboBx.TabIndex = 7;
            this.processorComboBx.SelectedIndexChanged += new System.EventHandler(this.UpdateGrabberProcessorChoice);
            // 
            // processorLabel
            // 
            this.processorLabel.AutoSize = true;
            this.processorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processorLabel.Location = new System.Drawing.Point(97, 168);
            this.processorLabel.Name = "processorLabel";
            this.processorLabel.Size = new System.Drawing.Size(119, 16);
            this.processorLabel.TabIndex = 6;
            this.processorLabel.Text = "Screen Processor:";
            // 
            // grabberSettingsBtn
            // 
            this.grabberSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.grabberSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.grabberSettingsBtn.Location = new System.Drawing.Point(573, 125);
            this.grabberSettingsBtn.Name = "grabberSettingsBtn";
            this.grabberSettingsBtn.Size = new System.Drawing.Size(24, 21);
            this.grabberSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.grabberSettingsBtn.TabIndex = 5;
            this.grabberSettingsBtn.TabStop = false;
            // 
            // grabberComboBx
            // 
            this.grabberComboBx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.grabberComboBx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grabberComboBx.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.grabberComboBx.FormattingEnabled = true;
            this.grabberComboBx.Location = new System.Drawing.Point(222, 125);
            this.grabberComboBx.Name = "grabberComboBx";
            this.grabberComboBx.Size = new System.Drawing.Size(345, 23);
            this.grabberComboBx.TabIndex = 4;
            this.grabberComboBx.SelectedIndexChanged += new System.EventHandler(this.UpdateGrabberProcessorChoice);
            // 
            // grabberLabel
            // 
            this.grabberLabel.AutoSize = true;
            this.grabberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grabberLabel.Location = new System.Drawing.Point(109, 126);
            this.grabberLabel.Name = "grabberLabel";
            this.grabberLabel.Size = new System.Drawing.Size(107, 16);
            this.grabberLabel.TabIndex = 3;
            this.grabberLabel.Text = "Screen Grabber:";
            // 
            // driverSettingsBtn
            // 
            this.driverSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.driverSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.driverSettingsBtn.Location = new System.Drawing.Point(573, 84);
            this.driverSettingsBtn.Name = "driverSettingsBtn";
            this.driverSettingsBtn.Size = new System.Drawing.Size(24, 21);
            this.driverSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.driverSettingsBtn.TabIndex = 2;
            this.driverSettingsBtn.TabStop = false;
            // 
            // driverComboBox
            // 
            this.driverComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.driverComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverComboBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.driverComboBox.FormattingEnabled = true;
            this.driverComboBox.Location = new System.Drawing.Point(222, 84);
            this.driverComboBox.Name = "driverComboBox";
            this.driverComboBox.Size = new System.Drawing.Size(345, 23);
            this.driverComboBox.TabIndex = 1;
            this.driverComboBox.SelectedIndexChanged += new System.EventHandler(this.driverComboBox_SelectedIndexChanged);
            // 
            // driverLabel
            // 
            this.driverLabel.AutoSize = true;
            this.driverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.driverLabel.Location = new System.Drawing.Point(166, 85);
            this.driverLabel.Name = "driverLabel";
            this.driverLabel.Size = new System.Drawing.Size(50, 16);
            this.driverLabel.TabIndex = 0;
            this.driverLabel.Text = "Driver: ";
            // 
            // decoratorPage
            // 
            this.decoratorPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.decoratorPage.Controls.Add(this.toggleActiveBtn);
            this.decoratorPage.Controls.Add(this.currentDecStatus);
            this.decoratorPage.Controls.Add(this.currentDecStatusLabel);
            this.decoratorPage.Controls.Add(this.decoratorSettingsBtn);
            this.decoratorPage.Controls.Add(this.decoratorComboBx);
            this.decoratorPage.Controls.Add(this.decoratorLabel);
            this.decoratorPage.Location = new System.Drawing.Point(4, 25);
            this.decoratorPage.Margin = new System.Windows.Forms.Padding(2);
            this.decoratorPage.Name = "decoratorPage";
            this.decoratorPage.Padding = new System.Windows.Forms.Padding(2);
            this.decoratorPage.Size = new System.Drawing.Size(831, 351);
            this.decoratorPage.TabIndex = 1;
            this.decoratorPage.Text = "Decorators";
            // 
            // toggleActiveBtn
            // 
            this.toggleActiveBtn.AutoSize = true;
            this.toggleActiveBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toggleActiveBtn.Location = new System.Drawing.Point(435, 142);
            this.toggleActiveBtn.Name = "toggleActiveBtn";
            this.toggleActiveBtn.Size = new System.Drawing.Size(102, 26);
            this.toggleActiveBtn.TabIndex = 8;
            this.toggleActiveBtn.Text = "Toggle Active";
            this.toggleActiveBtn.UseVisualStyleBackColor = true;
            this.toggleActiveBtn.Click += new System.EventHandler(this.ToggleDecorator);
            // 
            // currentDecStatus
            // 
            this.currentDecStatus.AutoSize = true;
            this.currentDecStatus.Location = new System.Drawing.Point(244, 148);
            this.currentDecStatus.Name = "currentDecStatus";
            this.currentDecStatus.Size = new System.Drawing.Size(0, 16);
            this.currentDecStatus.TabIndex = 7;
            // 
            // currentDecStatusLabel
            // 
            this.currentDecStatusLabel.AutoSize = true;
            this.currentDecStatusLabel.Location = new System.Drawing.Point(187, 148);
            this.currentDecStatusLabel.Name = "currentDecStatusLabel";
            this.currentDecStatusLabel.Size = new System.Drawing.Size(58, 16);
            this.currentDecStatusLabel.TabIndex = 6;
            this.currentDecStatusLabel.Text = "Active?: ";
            // 
            // decoratorSettingsBtn
            // 
            this.decoratorSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.decoratorSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.decoratorSettingsBtn.Location = new System.Drawing.Point(599, 81);
            this.decoratorSettingsBtn.Name = "decoratorSettingsBtn";
            this.decoratorSettingsBtn.Size = new System.Drawing.Size(24, 21);
            this.decoratorSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.decoratorSettingsBtn.TabIndex = 5;
            this.decoratorSettingsBtn.TabStop = false;
            // 
            // decoratorComboBx
            // 
            this.decoratorComboBx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.decoratorComboBx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.decoratorComboBx.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decoratorComboBx.FormattingEnabled = true;
            this.decoratorComboBx.Location = new System.Drawing.Point(244, 79);
            this.decoratorComboBx.Name = "decoratorComboBx";
            this.decoratorComboBx.Size = new System.Drawing.Size(345, 23);
            this.decoratorComboBx.TabIndex = 4;
            this.decoratorComboBx.SelectedIndexChanged += new System.EventHandler(this.decoratorComboBx_SelectedIndexChanged);
            // 
            // decoratorLabel
            // 
            this.decoratorLabel.AutoSize = true;
            this.decoratorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decoratorLabel.Location = new System.Drawing.Point(110, 82);
            this.decoratorLabel.Name = "decoratorLabel";
            this.decoratorLabel.Size = new System.Drawing.Size(128, 16);
            this.decoratorLabel.TabIndex = 3;
            this.decoratorLabel.Text = "Selected Decorator:";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(831, 351);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Notifiers";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(380, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Coming Soon!";
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(962, 707);
            this.Controls.Add(this.aboutPage);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.compoundDecorationCheck);
            this.Controls.Add(this.driverRecBtn);
            this.Controls.Add(this.deviceName);
            this.Controls.Add(this.deviceNameLabel);
            this.Controls.Add(this.weightingEnabled);
            this.Controls.Add(this.newColorWeight);
            this.Controls.Add(this.weightingLabel);
            this.Controls.Add(this.settingsTitle);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.offBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.glowStatus);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.pollingWidth);
            this.Controls.Add(this.pollingHeight);
            this.Controls.Add(this.pollingHeightLabel);
            this.Controls.Add(this.pollingWidthLabel);
            this.Controls.Add(this.pollingY);
            this.Controls.Add(this.pollingYLabel);
            this.Controls.Add(this.pollingX);
            this.Controls.Add(this.pollingXLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pollingArea);
            this.Controls.Add(this.sleepSize);
            this.Controls.Add(this.stepSleepLabel);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SettingsWindow";
            this.Padding = new System.Windows.Forms.Padding(30, 92, 30, 31);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsWindow_MouseDown);
            this.aboutPage.ResumeLayout(false);
            this.driverPage.ResumeLayout(false);
            this.driverPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processorSettingsBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grabberSettingsBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driverSettingsBtn)).EndInit();
            this.decoratorPage.ResumeLayout(false);
            this.decoratorPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decoratorSettingsBtn)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sleepSize;
        //private MetroFramework.Components.MetroStyleManager settingsStyleManager;
        private System.Windows.Forms.Label stepSleepLabel;
        private System.Windows.Forms.Label metroLabel1;
        private System.Windows.Forms.Button pollingArea;
        private System.Windows.Forms.Label pollingWidth;
        private System.Windows.Forms.Label pollingHeight;
        private System.Windows.Forms.Label pollingHeightLabel;
        private System.Windows.Forms.Label pollingWidthLabel;
        private System.Windows.Forms.Label pollingY;
        private System.Windows.Forms.Label pollingYLabel;
        private System.Windows.Forms.Label pollingX;
        private System.Windows.Forms.Label pollingXLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button offBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Label statusLabel;
        public System.Windows.Forms.Label glowStatus;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label settingsTitle;
        private System.Windows.Forms.TextBox newColorWeight;
        private System.Windows.Forms.Label weightingLabel;
        private System.Windows.Forms.CheckBox weightingEnabled;
        private System.Windows.Forms.Label deviceNameLabel;
        private System.Windows.Forms.Label deviceName;
        private System.Windows.Forms.Button driverRecBtn;
        private System.Windows.Forms.CheckBox compoundDecorationCheck;
        private System.Windows.Forms.Label versionLabel;
        private FlatTabControl.FlatTabControl aboutPage;
        private System.Windows.Forms.TabPage driverPage;
        private System.Windows.Forms.TabPage decoratorPage;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label driverLabel;
        private System.Windows.Forms.ComboBox driverComboBox;
        private System.Windows.Forms.PictureBox processorSettingsBtn;
        private System.Windows.Forms.ComboBox processorComboBx;
        private System.Windows.Forms.Label processorLabel;
        private System.Windows.Forms.PictureBox grabberSettingsBtn;
        private System.Windows.Forms.ComboBox grabberComboBx;
        private System.Windows.Forms.Label grabberLabel;
        private System.Windows.Forms.PictureBox driverSettingsBtn;
        private System.Windows.Forms.Label instructions;
        private System.Windows.Forms.PictureBox decoratorSettingsBtn;
        private System.Windows.Forms.ComboBox decoratorComboBx;
        private System.Windows.Forms.Label decoratorLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label currentDecStatus;
        private System.Windows.Forms.Label currentDecStatusLabel;
        private System.Windows.Forms.Button toggleActiveBtn;
    }
}