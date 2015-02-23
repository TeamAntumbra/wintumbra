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
            this.DriverLabel = new System.Windows.Forms.Label();
            this.driverExtensions = new System.Windows.Forms.ComboBox();
            this.screenGrabbers = new System.Windows.Forms.ComboBox();
            this.screenGrabberLabel = new System.Windows.Forms.Label();
            this.screenProcessors = new System.Windows.Forms.ComboBox();
            this.screenProcessorLabel = new System.Windows.Forms.Label();
            this.notifiersLabel = new System.Windows.Forms.Label();
            this.decoratorsLabel = new System.Windows.Forms.Label();
            this.apply = new System.Windows.Forms.Button();
            this.metroLabel1 = new System.Windows.Forms.Label();
            this.notifiers = new System.Windows.Forms.ComboBox();
            this.decoratorToggle = new System.Windows.Forms.Button();
            this.notifierToggle = new System.Windows.Forms.Button();
            this.pollingArea = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pollingXLabel = new System.Windows.Forms.Label();
            this.pollingX = new System.Windows.Forms.Label();
            this.pollingYLabel = new System.Windows.Forms.Label();
            this.pollingY = new System.Windows.Forms.Label();
            this.pollingWidthLabel = new System.Windows.Forms.Label();
            this.pollingHeightLabel = new System.Windows.Forms.Label();
            this.pollingHeight = new System.Windows.Forms.Label();
            this.decorators = new System.Windows.Forms.ComboBox();
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
            this.grabberRecBtn = new System.Windows.Forms.Button();
            this.currentSetupLabel = new System.Windows.Forms.Label();
            this.currentSetup = new System.Windows.Forms.Label();
            this.compoundDecorationCheck = new System.Windows.Forms.CheckBox();
            this.driverSettingsBtn = new System.Windows.Forms.PictureBox();
            this.grabberSettingsBtn = new System.Windows.Forms.PictureBox();
            this.processorSettingsBtn = new System.Windows.Forms.PictureBox();
            this.currentDecSettingsBtn = new System.Windows.Forms.PictureBox();
            this.currentNotfSettingsBtn = new System.Windows.Forms.PictureBox();
            this.versionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.driverSettingsBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grabberSettingsBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processorSettingsBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentDecSettingsBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentNotfSettingsBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // stepSleepLabel
            // 
            this.stepSleepLabel.AutoSize = true;
            this.stepSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSleepLabel.Location = new System.Drawing.Point(82, 711);
            this.stepSleepLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.stepSleepLabel.Name = "stepSleepLabel";
            this.stepSleepLabel.Size = new System.Drawing.Size(127, 20);
            this.stepSleepLabel.TabIndex = 17;
            this.stepSleepLabel.Text = "Step Sleep: (ms)";
            // 
            // sleepSize
            // 
            this.sleepSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.sleepSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.sleepSize.Location = new System.Drawing.Point(306, 702);
            this.sleepSize.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.sleepSize.Name = "sleepSize";
            this.sleepSize.Size = new System.Drawing.Size(152, 26);
            this.sleepSize.TabIndex = 22;
            this.sleepSize.TextChanged += new System.EventHandler(this.sleepSize_TextChanged);
            // 
            // DriverLabel
            // 
            this.DriverLabel.AutoSize = true;
            this.DriverLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DriverLabel.Location = new System.Drawing.Point(76, 351);
            this.DriverLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DriverLabel.Name = "DriverLabel";
            this.DriverLabel.Size = new System.Drawing.Size(128, 20);
            this.DriverLabel.TabIndex = 32;
            this.DriverLabel.Text = "Driver Extension:";
            // 
            // driverExtensions
            // 
            this.driverExtensions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.driverExtensions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverExtensions.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.driverExtensions.FormattingEnabled = true;
            this.driverExtensions.ItemHeight = 20;
            this.driverExtensions.Location = new System.Drawing.Point(279, 346);
            this.driverExtensions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.driverExtensions.Name = "driverExtensions";
            this.driverExtensions.Size = new System.Drawing.Size(406, 28);
            this.driverExtensions.TabIndex = 33;
            // 
            // screenGrabbers
            // 
            this.screenGrabbers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.screenGrabbers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screenGrabbers.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenGrabbers.FormattingEnabled = true;
            this.screenGrabbers.ItemHeight = 20;
            this.screenGrabbers.Location = new System.Drawing.Point(279, 398);
            this.screenGrabbers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.screenGrabbers.Name = "screenGrabbers";
            this.screenGrabbers.Size = new System.Drawing.Size(406, 28);
            this.screenGrabbers.TabIndex = 35;
            // 
            // screenGrabberLabel
            // 
            this.screenGrabberLabel.AutoSize = true;
            this.screenGrabberLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenGrabberLabel.Location = new System.Drawing.Point(76, 403);
            this.screenGrabberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.screenGrabberLabel.Name = "screenGrabberLabel";
            this.screenGrabberLabel.Size = new System.Drawing.Size(127, 20);
            this.screenGrabberLabel.TabIndex = 34;
            this.screenGrabberLabel.Text = "Screen Grabber:";
            // 
            // screenProcessors
            // 
            this.screenProcessors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.screenProcessors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screenProcessors.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenProcessors.FormattingEnabled = true;
            this.screenProcessors.ItemHeight = 20;
            this.screenProcessors.Location = new System.Drawing.Point(279, 451);
            this.screenProcessors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.screenProcessors.Name = "screenProcessors";
            this.screenProcessors.Size = new System.Drawing.Size(406, 28);
            this.screenProcessors.TabIndex = 37;
            // 
            // screenProcessorLabel
            // 
            this.screenProcessorLabel.AutoSize = true;
            this.screenProcessorLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenProcessorLabel.Location = new System.Drawing.Point(64, 455);
            this.screenProcessorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.screenProcessorLabel.Name = "screenProcessorLabel";
            this.screenProcessorLabel.Size = new System.Drawing.Size(139, 20);
            this.screenProcessorLabel.TabIndex = 36;
            this.screenProcessorLabel.Text = "Screen Processor:";
            // 
            // notifiersLabel
            // 
            this.notifiersLabel.AutoSize = true;
            this.notifiersLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.notifiersLabel.Location = new System.Drawing.Point(144, 558);
            this.notifiersLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.notifiersLabel.Name = "notifiersLabel";
            this.notifiersLabel.Size = new System.Drawing.Size(71, 20);
            this.notifiersLabel.TabIndex = 40;
            this.notifiersLabel.Text = "Notifiers:";
            // 
            // decoratorsLabel
            // 
            this.decoratorsLabel.AutoSize = true;
            this.decoratorsLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decoratorsLabel.Location = new System.Drawing.Point(120, 506);
            this.decoratorsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.decoratorsLabel.Name = "decoratorsLabel";
            this.decoratorsLabel.Size = new System.Drawing.Size(92, 20);
            this.decoratorsLabel.TabIndex = 38;
            this.decoratorsLabel.Text = "Decorators:";
            // 
            // apply
            // 
            this.apply.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.apply.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.apply.Location = new System.Drawing.Point(675, 635);
            this.apply.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(238, 35);
            this.apply.TabIndex = 43;
            this.apply.Text = "Apply Extension Changes";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
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
            // notifiers
            // 
            this.notifiers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.notifiers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.notifiers.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.notifiers.FormattingEnabled = true;
            this.notifiers.ItemHeight = 20;
            this.notifiers.Location = new System.Drawing.Point(279, 558);
            this.notifiers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.notifiers.Name = "notifiers";
            this.notifiers.Size = new System.Drawing.Size(406, 28);
            this.notifiers.TabIndex = 47;
            // 
            // decoratorToggle
            // 
            this.decoratorToggle.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.decoratorToggle.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.decoratorToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.decoratorToggle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decoratorToggle.Location = new System.Drawing.Point(759, 494);
            this.decoratorToggle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.decoratorToggle.Name = "decoratorToggle";
            this.decoratorToggle.Size = new System.Drawing.Size(154, 45);
            this.decoratorToggle.TabIndex = 48;
            this.decoratorToggle.Text = "Toggle Selected";
            this.decoratorToggle.UseVisualStyleBackColor = false;
            this.decoratorToggle.Click += new System.EventHandler(this.decoratorToggle_Click);
            // 
            // notifierToggle
            // 
            this.notifierToggle.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.notifierToggle.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.notifierToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.notifierToggle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.notifierToggle.Location = new System.Drawing.Point(759, 551);
            this.notifierToggle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.notifierToggle.Name = "notifierToggle";
            this.notifierToggle.Size = new System.Drawing.Size(154, 45);
            this.notifierToggle.TabIndex = 49;
            this.notifierToggle.Text = "Toggle Selected";
            this.notifierToggle.UseVisualStyleBackColor = false;
            this.notifierToggle.Click += new System.EventHandler(this.notifierToggle_Click);
            // 
            // pollingArea
            // 
            this.pollingArea.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.pollingArea.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pollingArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pollingArea.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingArea.Location = new System.Drawing.Point(572, 191);
            this.pollingArea.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pollingArea.Name = "pollingArea";
            this.pollingArea.Size = new System.Drawing.Size(207, 54);
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
            this.label1.Location = new System.Drawing.Point(183, 109);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 25);
            this.label1.TabIndex = 52;
            this.label1.Text = "Polling Location:";
            // 
            // pollingXLabel
            // 
            this.pollingXLabel.AutoSize = true;
            this.pollingXLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingXLabel.Location = new System.Drawing.Point(114, 175);
            this.pollingXLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingXLabel.Name = "pollingXLabel";
            this.pollingXLabel.Size = new System.Drawing.Size(24, 20);
            this.pollingXLabel.TabIndex = 53;
            this.pollingXLabel.Text = "X:";
            // 
            // pollingX
            // 
            this.pollingX.AutoSize = true;
            this.pollingX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingX.Location = new System.Drawing.Point(148, 175);
            this.pollingX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingX.Name = "pollingX";
            this.pollingX.Size = new System.Drawing.Size(0, 20);
            this.pollingX.TabIndex = 54;
            // 
            // pollingYLabel
            // 
            this.pollingYLabel.AutoSize = true;
            this.pollingYLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingYLabel.Location = new System.Drawing.Point(114, 225);
            this.pollingYLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingYLabel.Name = "pollingYLabel";
            this.pollingYLabel.Size = new System.Drawing.Size(24, 20);
            this.pollingYLabel.TabIndex = 55;
            this.pollingYLabel.Text = "Y:";
            // 
            // pollingY
            // 
            this.pollingY.AutoSize = true;
            this.pollingY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingY.Location = new System.Drawing.Point(148, 225);
            this.pollingY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingY.Name = "pollingY";
            this.pollingY.Size = new System.Drawing.Size(0, 20);
            this.pollingY.TabIndex = 56;
            // 
            // pollingWidthLabel
            // 
            this.pollingWidthLabel.AutoSize = true;
            this.pollingWidthLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingWidthLabel.Location = new System.Drawing.Point(248, 175);
            this.pollingWidthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingWidthLabel.Name = "pollingWidthLabel";
            this.pollingWidthLabel.Size = new System.Drawing.Size(54, 20);
            this.pollingWidthLabel.TabIndex = 57;
            this.pollingWidthLabel.Text = "Width:";
            // 
            // pollingHeightLabel
            // 
            this.pollingHeightLabel.AutoSize = true;
            this.pollingHeightLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingHeightLabel.Location = new System.Drawing.Point(248, 225);
            this.pollingHeightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingHeightLabel.Name = "pollingHeightLabel";
            this.pollingHeightLabel.Size = new System.Drawing.Size(60, 20);
            this.pollingHeightLabel.TabIndex = 58;
            this.pollingHeightLabel.Text = "Height:";
            // 
            // pollingHeight
            // 
            this.pollingHeight.AutoSize = true;
            this.pollingHeight.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingHeight.Location = new System.Drawing.Point(318, 225);
            this.pollingHeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingHeight.Name = "pollingHeight";
            this.pollingHeight.Size = new System.Drawing.Size(0, 20);
            this.pollingHeight.TabIndex = 60;
            // 
            // decorators
            // 
            this.decorators.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.decorators.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.decorators.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decorators.FormattingEnabled = true;
            this.decorators.ItemHeight = 20;
            this.decorators.Location = new System.Drawing.Point(279, 505);
            this.decorators.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.decorators.Name = "decorators";
            this.decorators.Size = new System.Drawing.Size(406, 28);
            this.decorators.TabIndex = 46;
            // 
            // pollingWidth
            // 
            this.pollingWidth.AutoSize = true;
            this.pollingWidth.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingWidth.Location = new System.Drawing.Point(314, 175);
            this.pollingWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pollingWidth.Name = "pollingWidth";
            this.pollingWidth.Size = new System.Drawing.Size(0, 20);
            this.pollingWidth.TabIndex = 61;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.statusLabel.Location = new System.Drawing.Point(476, 697);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(100, 20);
            this.statusLabel.TabIndex = 64;
            this.statusLabel.Text = "Glow Status:";
            // 
            // glowStatus
            // 
            this.glowStatus.AutoSize = true;
            this.glowStatus.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.glowStatus.Location = new System.Drawing.Point(580, 698);
            this.glowStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.glowStatus.Name = "glowStatus";
            this.glowStatus.Size = new System.Drawing.Size(0, 20);
            this.glowStatus.TabIndex = 65;
            // 
            // startBtn
            // 
            this.startBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.startBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.startBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.startBtn.Location = new System.Drawing.Point(249, 946);
            this.startBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(114, 45);
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
            this.offBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.offBtn.Location = new System.Drawing.Point(495, 946);
            this.offBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.offBtn.Name = "offBtn";
            this.offBtn.Size = new System.Drawing.Size(114, 45);
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
            this.stopBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stopBtn.Location = new System.Drawing.Point(372, 946);
            this.stopBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(114, 45);
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
            this.closeBtn.Location = new System.Drawing.Point(874, -5);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(32, 32);
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
            this.settingsTitle.Location = new System.Drawing.Point(418, 37);
            this.settingsTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.settingsTitle.Name = "settingsTitle";
            this.settingsTitle.Size = new System.Drawing.Size(135, 38);
            this.settingsTitle.TabIndex = 73;
            this.settingsTitle.Text = "Settings";
            this.settingsTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // newColorWeight
            // 
            this.newColorWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.newColorWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newColorWeight.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.newColorWeight.Location = new System.Drawing.Point(306, 755);
            this.newColorWeight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.newColorWeight.Name = "newColorWeight";
            this.newColorWeight.Size = new System.Drawing.Size(152, 26);
            this.newColorWeight.TabIndex = 77;
            this.newColorWeight.TextChanged += new System.EventHandler(this.newColorWeight_TextChanged);
            // 
            // weightingLabel
            // 
            this.weightingLabel.AutoSize = true;
            this.weightingLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.weightingLabel.Location = new System.Drawing.Point(33, 766);
            this.weightingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.weightingLabel.Name = "weightingLabel";
            this.weightingLabel.Size = new System.Drawing.Size(208, 20);
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
            this.weightingEnabled.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.weightingEnabled.Location = new System.Drawing.Point(489, 738);
            this.weightingEnabled.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.weightingEnabled.Name = "weightingEnabled";
            this.weightingEnabled.Size = new System.Drawing.Size(233, 24);
            this.weightingEnabled.TabIndex = 78;
            this.weightingEnabled.Text = "Weighted Average Enabled?";
            this.weightingEnabled.UseVisualStyleBackColor = false;
            this.weightingEnabled.CheckedChanged += new System.EventHandler(this.weightingEnabled_CheckedChanged);
            // 
            // deviceNameLabel
            // 
            this.deviceNameLabel.AutoSize = true;
            this.deviceNameLabel.Location = new System.Drawing.Point(567, 142);
            this.deviceNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.deviceNameLabel.Name = "deviceNameLabel";
            this.deviceNameLabel.Size = new System.Drawing.Size(82, 20);
            this.deviceNameLabel.TabIndex = 79;
            this.deviceNameLabel.Text = "Device ID:";
            // 
            // deviceName
            // 
            this.deviceName.AutoSize = true;
            this.deviceName.Location = new System.Drawing.Point(688, 142);
            this.deviceName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.deviceName.Name = "deviceName";
            this.deviceName.Size = new System.Drawing.Size(0, 20);
            this.deviceName.TabIndex = 80;
            // 
            // driverRecBtn
            // 
            this.driverRecBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.driverRecBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.driverRecBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverRecBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.driverRecBtn.Location = new System.Drawing.Point(759, 332);
            this.driverRecBtn.Margin = new System.Windows.Forms.Padding(0);
            this.driverRecBtn.Name = "driverRecBtn";
            this.driverRecBtn.Size = new System.Drawing.Size(154, 55);
            this.driverRecBtn.TabIndex = 81;
            this.driverRecBtn.Text = "Recommended Settings";
            this.driverRecBtn.UseVisualStyleBackColor = false;
            this.driverRecBtn.Click += new System.EventHandler(this.driverRecBtn_Click);
            // 
            // grabberRecBtn
            // 
            this.grabberRecBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.grabberRecBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grabberRecBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grabberRecBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.grabberRecBtn.Location = new System.Drawing.Point(759, 388);
            this.grabberRecBtn.Margin = new System.Windows.Forms.Padding(0);
            this.grabberRecBtn.Name = "grabberRecBtn";
            this.grabberRecBtn.Size = new System.Drawing.Size(154, 55);
            this.grabberRecBtn.TabIndex = 82;
            this.grabberRecBtn.Text = "Recommended Settings";
            this.grabberRecBtn.UseVisualStyleBackColor = false;
            this.grabberRecBtn.Click += new System.EventHandler(this.grabberRecBtn_Click);
            // 
            // currentSetupLabel
            // 
            this.currentSetupLabel.AutoSize = true;
            this.currentSetupLabel.Location = new System.Drawing.Point(104, 820);
            this.currentSetupLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentSetupLabel.Name = "currentSetupLabel";
            this.currentSetupLabel.Size = new System.Drawing.Size(113, 20);
            this.currentSetupLabel.TabIndex = 83;
            this.currentSetupLabel.Text = "Current Setup:";
            // 
            // currentSetup
            // 
            this.currentSetup.AutoSize = true;
            this.currentSetup.Location = new System.Drawing.Point(225, 820);
            this.currentSetup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentSetup.Name = "currentSetup";
            this.currentSetup.Size = new System.Drawing.Size(0, 20);
            this.currentSetup.TabIndex = 84;
            // 
            // compoundDecorationCheck
            // 
            this.compoundDecorationCheck.AutoSize = true;
            this.compoundDecorationCheck.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.compoundDecorationCheck.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.compoundDecorationCheck.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.compoundDecorationCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.compoundDecorationCheck.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.compoundDecorationCheck.Location = new System.Drawing.Point(489, 774);
            this.compoundDecorationCheck.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.compoundDecorationCheck.Name = "compoundDecorationCheck";
            this.compoundDecorationCheck.Size = new System.Drawing.Size(399, 44);
            this.compoundDecorationCheck.TabIndex = 85;
            this.compoundDecorationCheck.Text = "Compound Decoration \r\n(Each Decorator Acts on the Output of the Previous)";
            this.compoundDecorationCheck.UseVisualStyleBackColor = false;
            this.compoundDecorationCheck.CheckedChanged += new System.EventHandler(this.compoundDecorationCheck_CheckedChanged);
            // 
            // driverSettingsBtn
            // 
            this.driverSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.driverSettingsBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.driverSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.driverSettingsBtn.Location = new System.Drawing.Point(696, 346);
            this.driverSettingsBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.driverSettingsBtn.Name = "driverSettingsBtn";
            this.driverSettingsBtn.Size = new System.Drawing.Size(30, 31);
            this.driverSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.driverSettingsBtn.TabIndex = 86;
            this.driverSettingsBtn.TabStop = false;
            this.driverSettingsBtn.Click += new System.EventHandler(this.driverSettingsBtn_Click);
            // 
            // grabberSettingsBtn
            // 
            this.grabberSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.grabberSettingsBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grabberSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.grabberSettingsBtn.Location = new System.Drawing.Point(696, 398);
            this.grabberSettingsBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grabberSettingsBtn.Name = "grabberSettingsBtn";
            this.grabberSettingsBtn.Size = new System.Drawing.Size(30, 31);
            this.grabberSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.grabberSettingsBtn.TabIndex = 87;
            this.grabberSettingsBtn.TabStop = false;
            this.grabberSettingsBtn.Click += new System.EventHandler(this.grabberSettingsBtn_Click);
            // 
            // processorSettingsBtn
            // 
            this.processorSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.processorSettingsBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.processorSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.processorSettingsBtn.Location = new System.Drawing.Point(696, 451);
            this.processorSettingsBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.processorSettingsBtn.Name = "processorSettingsBtn";
            this.processorSettingsBtn.Size = new System.Drawing.Size(30, 31);
            this.processorSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.processorSettingsBtn.TabIndex = 88;
            this.processorSettingsBtn.TabStop = false;
            this.processorSettingsBtn.Click += new System.EventHandler(this.processorSettingsBtn_Click);
            // 
            // currentDecSettingsBtn
            // 
            this.currentDecSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.currentDecSettingsBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentDecSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.currentDecSettingsBtn.Location = new System.Drawing.Point(696, 505);
            this.currentDecSettingsBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.currentDecSettingsBtn.Name = "currentDecSettingsBtn";
            this.currentDecSettingsBtn.Size = new System.Drawing.Size(30, 31);
            this.currentDecSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.currentDecSettingsBtn.TabIndex = 89;
            this.currentDecSettingsBtn.TabStop = false;
            this.currentDecSettingsBtn.Click += new System.EventHandler(this.currentDecSettingsBtn_Click);
            // 
            // currentNotfSettingsBtn
            // 
            this.currentNotfSettingsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.currentNotfSettingsBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentNotfSettingsBtn.Image = global::Antumbra.Glow.Properties.Resources.gear;
            this.currentNotfSettingsBtn.Location = new System.Drawing.Point(696, 558);
            this.currentNotfSettingsBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.currentNotfSettingsBtn.Name = "currentNotfSettingsBtn";
            this.currentNotfSettingsBtn.Size = new System.Drawing.Size(30, 31);
            this.currentNotfSettingsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.currentNotfSettingsBtn.TabIndex = 90;
            this.currentNotfSettingsBtn.TabStop = false;
            this.currentNotfSettingsBtn.Click += new System.EventHandler(this.currentNotfSettingsBtn_Click);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(568, 52);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(0, 20);
            this.versionLabel.TabIndex = 91;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(963, 1088);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.currentNotfSettingsBtn);
            this.Controls.Add(this.currentDecSettingsBtn);
            this.Controls.Add(this.processorSettingsBtn);
            this.Controls.Add(this.grabberSettingsBtn);
            this.Controls.Add(this.driverSettingsBtn);
            this.Controls.Add(this.compoundDecorationCheck);
            this.Controls.Add(this.currentSetup);
            this.Controls.Add(this.currentSetupLabel);
            this.Controls.Add(this.grabberRecBtn);
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
            this.Controls.Add(this.notifierToggle);
            this.Controls.Add(this.decoratorToggle);
            this.Controls.Add(this.notifiers);
            this.Controls.Add(this.decorators);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.notifiersLabel);
            this.Controls.Add(this.decoratorsLabel);
            this.Controls.Add(this.screenProcessors);
            this.Controls.Add(this.screenProcessorLabel);
            this.Controls.Add(this.screenGrabbers);
            this.Controls.Add(this.screenGrabberLabel);
            this.Controls.Add(this.driverExtensions);
            this.Controls.Add(this.DriverLabel);
            this.Controls.Add(this.sleepSize);
            this.Controls.Add(this.stepSleepLabel);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SettingsWindow";
            this.Padding = new System.Windows.Forms.Padding(45, 142, 45, 48);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsWindow_MouseDown);
            this.MouseEnter += new System.EventHandler(this.SettingsWindow_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.driverSettingsBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grabberSettingsBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processorSettingsBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentDecSettingsBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentNotfSettingsBtn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sleepSize;
        //private MetroFramework.Components.MetroStyleManager settingsStyleManager;
        private System.Windows.Forms.Label stepSleepLabel;
        private System.Windows.Forms.ComboBox driverExtensions;
        private System.Windows.Forms.Label DriverLabel;
        private System.Windows.Forms.ComboBox screenProcessors;
        private System.Windows.Forms.Label screenProcessorLabel;
        private System.Windows.Forms.ComboBox screenGrabbers;
        private System.Windows.Forms.Label screenGrabberLabel;
        private System.Windows.Forms.Label notifiersLabel;
        private System.Windows.Forms.Label decoratorsLabel;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Label metroLabel1;
        private System.Windows.Forms.Button notifierToggle;
        private System.Windows.Forms.Button decoratorToggle;
        private System.Windows.Forms.ComboBox notifiers;
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
        private System.Windows.Forms.ComboBox decorators;
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
        private System.Windows.Forms.Button grabberRecBtn;
        private System.Windows.Forms.Label currentSetupLabel;
        private System.Windows.Forms.Label currentSetup;
        private System.Windows.Forms.CheckBox compoundDecorationCheck;
        private System.Windows.Forms.PictureBox driverSettingsBtn;
        private System.Windows.Forms.PictureBox grabberSettingsBtn;
        private System.Windows.Forms.PictureBox processorSettingsBtn;
        private System.Windows.Forms.PictureBox currentDecSettingsBtn;
        private System.Windows.Forms.PictureBox currentNotfSettingsBtn;
        private System.Windows.Forms.Label versionLabel;
    }
}