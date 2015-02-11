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
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.stepSize = new System.Windows.Forms.TextBox();
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
            this.speedLabel = new System.Windows.Forms.Label();
            this.speed = new System.Windows.Forms.Label();
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
            this.shouldChangeLabel = new System.Windows.Forms.Label();
            this.changeSensitivity = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.glowStatus = new System.Windows.Forms.Label();
            this.startBtn = new System.Windows.Forms.Button();
            this.offBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.maxStepsLabel = new System.Windows.Forms.Label();
            this.maxFadeSteps = new System.Windows.Forms.TextBox();
            this.fadeEnabledCheck = new System.Windows.Forms.CheckBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.settingsTitle = new System.Windows.Forms.Label();
            this.colorSwatch = new System.Windows.Forms.Button();
            this.currentColorLabel = new System.Windows.Forms.Label();
            this.newColorWeight = new System.Windows.Forms.TextBox();
            this.weightingLabel = new System.Windows.Forms.Label();
            this.weightingEnabled = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSizeLabel.Location = new System.Drawing.Point(79, 428);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(55, 13);
            this.stepSizeLabel.TabIndex = 7;
            this.stepSizeLabel.Text = "Step Size:";
            // 
            // stepSize
            // 
            this.stepSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.stepSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSize.Location = new System.Drawing.Point(204, 423);
            this.stepSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stepSize.Name = "stepSize";
            this.stepSize.Size = new System.Drawing.Size(102, 20);
            this.stepSize.TabIndex = 13;
            this.stepSize.TextChanged += new System.EventHandler(this.stepSize_TextChanged);
            // 
            // stepSleepLabel
            // 
            this.stepSleepLabel.AutoSize = true;
            this.stepSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSleepLabel.Location = new System.Drawing.Point(55, 463);
            this.stepSleepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stepSleepLabel.Name = "stepSleepLabel";
            this.stepSleepLabel.Size = new System.Drawing.Size(84, 13);
            this.stepSleepLabel.TabIndex = 17;
            this.stepSleepLabel.Text = "Step Sleep: (ms)";
            // 
            // sleepSize
            // 
            this.sleepSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.sleepSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.sleepSize.Location = new System.Drawing.Point(204, 457);
            this.sleepSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sleepSize.Name = "sleepSize";
            this.sleepSize.Size = new System.Drawing.Size(102, 20);
            this.sleepSize.TabIndex = 22;
            this.sleepSize.TextChanged += new System.EventHandler(this.sleepSize_TextChanged);
            // 
            // DriverLabel
            // 
            this.DriverLabel.AutoSize = true;
            this.DriverLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DriverLabel.Location = new System.Drawing.Point(51, 228);
            this.DriverLabel.Name = "DriverLabel";
            this.DriverLabel.Size = new System.Drawing.Size(87, 13);
            this.DriverLabel.TabIndex = 32;
            this.DriverLabel.Text = "Driver Extension:";
            // 
            // driverExtensions
            // 
            this.driverExtensions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.driverExtensions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverExtensions.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.driverExtensions.FormattingEnabled = true;
            this.driverExtensions.ItemHeight = 13;
            this.driverExtensions.Location = new System.Drawing.Point(186, 225);
            this.driverExtensions.Name = "driverExtensions";
            this.driverExtensions.Size = new System.Drawing.Size(348, 21);
            this.driverExtensions.TabIndex = 33;
            // 
            // screenGrabbers
            // 
            this.screenGrabbers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.screenGrabbers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screenGrabbers.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenGrabbers.FormattingEnabled = true;
            this.screenGrabbers.ItemHeight = 13;
            this.screenGrabbers.Location = new System.Drawing.Point(186, 259);
            this.screenGrabbers.Name = "screenGrabbers";
            this.screenGrabbers.Size = new System.Drawing.Size(348, 21);
            this.screenGrabbers.TabIndex = 35;
            // 
            // screenGrabberLabel
            // 
            this.screenGrabberLabel.AutoSize = true;
            this.screenGrabberLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenGrabberLabel.Location = new System.Drawing.Point(51, 262);
            this.screenGrabberLabel.Name = "screenGrabberLabel";
            this.screenGrabberLabel.Size = new System.Drawing.Size(85, 13);
            this.screenGrabberLabel.TabIndex = 34;
            this.screenGrabberLabel.Text = "Screen Grabber:";
            // 
            // screenProcessors
            // 
            this.screenProcessors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.screenProcessors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screenProcessors.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenProcessors.FormattingEnabled = true;
            this.screenProcessors.ItemHeight = 13;
            this.screenProcessors.Location = new System.Drawing.Point(186, 293);
            this.screenProcessors.Name = "screenProcessors";
            this.screenProcessors.Size = new System.Drawing.Size(348, 21);
            this.screenProcessors.TabIndex = 37;
            // 
            // screenProcessorLabel
            // 
            this.screenProcessorLabel.AutoSize = true;
            this.screenProcessorLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenProcessorLabel.Location = new System.Drawing.Point(43, 296);
            this.screenProcessorLabel.Name = "screenProcessorLabel";
            this.screenProcessorLabel.Size = new System.Drawing.Size(94, 13);
            this.screenProcessorLabel.TabIndex = 36;
            this.screenProcessorLabel.Text = "Screen Processor:";
            // 
            // notifiersLabel
            // 
            this.notifiersLabel.AutoSize = true;
            this.notifiersLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.notifiersLabel.Location = new System.Drawing.Point(96, 363);
            this.notifiersLabel.Name = "notifiersLabel";
            this.notifiersLabel.Size = new System.Drawing.Size(48, 13);
            this.notifiersLabel.TabIndex = 40;
            this.notifiersLabel.Text = "Notifiers:";
            // 
            // decoratorsLabel
            // 
            this.decoratorsLabel.AutoSize = true;
            this.decoratorsLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decoratorsLabel.Location = new System.Drawing.Point(80, 329);
            this.decoratorsLabel.Name = "decoratorsLabel";
            this.decoratorsLabel.Size = new System.Drawing.Size(62, 13);
            this.decoratorsLabel.TabIndex = 38;
            this.decoratorsLabel.Text = "Decorators:";
            // 
            // apply
            // 
            this.apply.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.apply.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.apply.Location = new System.Drawing.Point(375, 399);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(159, 23);
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
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.speedLabel.Location = new System.Drawing.Point(281, 114);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(75, 13);
            this.speedLabel.TabIndex = 44;
            this.speedLabel.Text = "Polling Speed:";
            // 
            // speed
            // 
            this.speed.AutoSize = true;
            this.speed.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.speed.Location = new System.Drawing.Point(380, 117);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(13, 13);
            this.speed.TabIndex = 45;
            this.speed.Text = "0";
            // 
            // notifiers
            // 
            this.notifiers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.notifiers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.notifiers.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.notifiers.FormattingEnabled = true;
            this.notifiers.ItemHeight = 13;
            this.notifiers.Location = new System.Drawing.Point(186, 363);
            this.notifiers.Name = "notifiers";
            this.notifiers.Size = new System.Drawing.Size(236, 21);
            this.notifiers.TabIndex = 47;
            // 
            // decoratorToggle
            // 
            this.decoratorToggle.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.decoratorToggle.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.decoratorToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.decoratorToggle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decoratorToggle.Location = new System.Drawing.Point(428, 328);
            this.decoratorToggle.Name = "decoratorToggle";
            this.decoratorToggle.Size = new System.Drawing.Size(106, 29);
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
            this.notifierToggle.Location = new System.Drawing.Point(428, 363);
            this.notifierToggle.Name = "notifierToggle";
            this.notifierToggle.Size = new System.Drawing.Size(106, 29);
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
            this.pollingArea.Location = new System.Drawing.Point(280, 149);
            this.pollingArea.Name = "pollingArea";
            this.pollingArea.Size = new System.Drawing.Size(138, 35);
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
            this.label1.Location = new System.Drawing.Point(79, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 17);
            this.label1.TabIndex = 52;
            this.label1.Text = "Polling Location:";
            // 
            // pollingXLabel
            // 
            this.pollingXLabel.AutoSize = true;
            this.pollingXLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingXLabel.Location = new System.Drawing.Point(33, 114);
            this.pollingXLabel.Name = "pollingXLabel";
            this.pollingXLabel.Size = new System.Drawing.Size(17, 13);
            this.pollingXLabel.TabIndex = 53;
            this.pollingXLabel.Text = "X:";
            // 
            // pollingX
            // 
            this.pollingX.AutoSize = true;
            this.pollingX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingX.Location = new System.Drawing.Point(56, 114);
            this.pollingX.Name = "pollingX";
            this.pollingX.Size = new System.Drawing.Size(0, 13);
            this.pollingX.TabIndex = 54;
            // 
            // pollingYLabel
            // 
            this.pollingYLabel.AutoSize = true;
            this.pollingYLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingYLabel.Location = new System.Drawing.Point(33, 146);
            this.pollingYLabel.Name = "pollingYLabel";
            this.pollingYLabel.Size = new System.Drawing.Size(17, 13);
            this.pollingYLabel.TabIndex = 55;
            this.pollingYLabel.Text = "Y:";
            // 
            // pollingY
            // 
            this.pollingY.AutoSize = true;
            this.pollingY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingY.Location = new System.Drawing.Point(56, 146);
            this.pollingY.Name = "pollingY";
            this.pollingY.Size = new System.Drawing.Size(0, 13);
            this.pollingY.TabIndex = 56;
            // 
            // pollingWidthLabel
            // 
            this.pollingWidthLabel.AutoSize = true;
            this.pollingWidthLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingWidthLabel.Location = new System.Drawing.Point(122, 114);
            this.pollingWidthLabel.Name = "pollingWidthLabel";
            this.pollingWidthLabel.Size = new System.Drawing.Size(38, 13);
            this.pollingWidthLabel.TabIndex = 57;
            this.pollingWidthLabel.Text = "Width:";
            // 
            // pollingHeightLabel
            // 
            this.pollingHeightLabel.AutoSize = true;
            this.pollingHeightLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingHeightLabel.Location = new System.Drawing.Point(122, 146);
            this.pollingHeightLabel.Name = "pollingHeightLabel";
            this.pollingHeightLabel.Size = new System.Drawing.Size(41, 13);
            this.pollingHeightLabel.TabIndex = 58;
            this.pollingHeightLabel.Text = "Height:";
            // 
            // pollingHeight
            // 
            this.pollingHeight.AutoSize = true;
            this.pollingHeight.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingHeight.Location = new System.Drawing.Point(169, 146);
            this.pollingHeight.Name = "pollingHeight";
            this.pollingHeight.Size = new System.Drawing.Size(0, 13);
            this.pollingHeight.TabIndex = 60;
            // 
            // decorators
            // 
            this.decorators.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.decorators.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.decorators.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decorators.FormattingEnabled = true;
            this.decorators.ItemHeight = 13;
            this.decorators.Location = new System.Drawing.Point(186, 328);
            this.decorators.Name = "decorators";
            this.decorators.Size = new System.Drawing.Size(236, 21);
            this.decorators.TabIndex = 46;
            // 
            // pollingWidth
            // 
            this.pollingWidth.AutoSize = true;
            this.pollingWidth.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.pollingWidth.Location = new System.Drawing.Point(166, 114);
            this.pollingWidth.Name = "pollingWidth";
            this.pollingWidth.Size = new System.Drawing.Size(0, 13);
            this.pollingWidth.TabIndex = 61;
            // 
            // shouldChangeLabel
            // 
            this.shouldChangeLabel.AutoSize = true;
            this.shouldChangeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.shouldChangeLabel.Location = new System.Drawing.Point(22, 500);
            this.shouldChangeLabel.Name = "shouldChangeLabel";
            this.shouldChangeLabel.Size = new System.Drawing.Size(124, 13);
            this.shouldChangeLabel.TabIndex = 62;
            this.shouldChangeLabel.Text = "Color Change Sensitivity:";
            // 
            // changeSensitivity
            // 
            this.changeSensitivity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.changeSensitivity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.changeSensitivity.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.changeSensitivity.Location = new System.Drawing.Point(204, 493);
            this.changeSensitivity.Name = "changeSensitivity";
            this.changeSensitivity.Size = new System.Drawing.Size(102, 20);
            this.changeSensitivity.TabIndex = 63;
            this.changeSensitivity.TextChanged += new System.EventHandler(this.changeSensitivity_TextChanged);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.statusLabel.Location = new System.Drawing.Point(317, 493);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(77, 13);
            this.statusLabel.TabIndex = 64;
            this.statusLabel.Text = "System Status:";
            // 
            // glowStatus
            // 
            this.glowStatus.AutoSize = true;
            this.glowStatus.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.glowStatus.Location = new System.Drawing.Point(397, 493);
            this.glowStatus.Name = "glowStatus";
            this.glowStatus.Size = new System.Drawing.Size(0, 13);
            this.glowStatus.TabIndex = 65;
            // 
            // startBtn
            // 
            this.startBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.startBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.startBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.startBtn.Location = new System.Drawing.Point(166, 615);
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
            this.offBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.offBtn.Location = new System.Drawing.Point(330, 615);
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
            this.stopBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stopBtn.Location = new System.Drawing.Point(248, 615);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(76, 29);
            this.stopBtn.TabIndex = 68;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = false;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // maxStepsLabel
            // 
            this.maxStepsLabel.AutoSize = true;
            this.maxStepsLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.maxStepsLabel.Location = new System.Drawing.Point(57, 573);
            this.maxStepsLabel.Name = "maxStepsLabel";
            this.maxStepsLabel.Size = new System.Drawing.Size(87, 13);
            this.maxStepsLabel.TabIndex = 69;
            this.maxStepsLabel.Text = "Max Fade Steps:";
            // 
            // maxFadeSteps
            // 
            this.maxFadeSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.maxFadeSteps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxFadeSteps.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.maxFadeSteps.Location = new System.Drawing.Point(204, 566);
            this.maxFadeSteps.Name = "maxFadeSteps";
            this.maxFadeSteps.Size = new System.Drawing.Size(102, 20);
            this.maxFadeSteps.TabIndex = 70;
            this.maxFadeSteps.TextChanged += new System.EventHandler(this.maxFadeSteps_TextChanged);
            // 
            // fadeEnabledCheck
            // 
            this.fadeEnabledCheck.AutoSize = true;
            this.fadeEnabledCheck.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.fadeEnabledCheck.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.fadeEnabledCheck.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.fadeEnabledCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fadeEnabledCheck.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.fadeEnabledCheck.Location = new System.Drawing.Point(326, 566);
            this.fadeEnabledCheck.Name = "fadeEnabledCheck";
            this.fadeEnabledCheck.Size = new System.Drawing.Size(95, 17);
            this.fadeEnabledCheck.TabIndex = 71;
            this.fadeEnabledCheck.Text = "Fade Enabled?";
            this.fadeEnabledCheck.UseVisualStyleBackColor = false;
            this.fadeEnabledCheck.CheckedChanged += new System.EventHandler(this.fadeEnabledCheck_CheckedChanged);
            // 
            // closeBtn
            // 
            this.closeBtn.AutoSize = true;
            this.closeBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.closeBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Firebrick;
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.closeBtn.Location = new System.Drawing.Point(508, -2);
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
            this.settingsTitle.Location = new System.Drawing.Point(238, 24);
            this.settingsTitle.Name = "settingsTitle";
            this.settingsTitle.Size = new System.Drawing.Size(91, 26);
            this.settingsTitle.TabIndex = 73;
            this.settingsTitle.Text = "Settings";
            this.settingsTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // colorSwatch
            // 
            this.colorSwatch.Enabled = false;
            this.colorSwatch.FlatAppearance.BorderSize = 0;
            this.colorSwatch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.colorSwatch.Location = new System.Drawing.Point(372, 69);
            this.colorSwatch.Name = "colorSwatch";
            this.colorSwatch.Size = new System.Drawing.Size(21, 21);
            this.colorSwatch.TabIndex = 74;
            this.colorSwatch.UseVisualStyleBackColor = false;
            // 
            // currentColorLabel
            // 
            this.currentColorLabel.AutoSize = true;
            this.currentColorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentColorLabel.Location = new System.Drawing.Point(260, 71);
            this.currentColorLabel.Name = "currentColorLabel";
            this.currentColorLabel.Size = new System.Drawing.Size(96, 17);
            this.currentColorLabel.TabIndex = 75;
            this.currentColorLabel.Text = "Current Color:";
            // 
            // newColorWeight
            // 
            this.newColorWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.newColorWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newColorWeight.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.newColorWeight.Location = new System.Drawing.Point(204, 529);
            this.newColorWeight.Name = "newColorWeight";
            this.newColorWeight.Size = new System.Drawing.Size(102, 20);
            this.newColorWeight.TabIndex = 77;
            this.newColorWeight.TextChanged += new System.EventHandler(this.newColorWeight_TextChanged);
            // 
            // weightingLabel
            // 
            this.weightingLabel.AutoSize = true;
            this.weightingLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.weightingLabel.Location = new System.Drawing.Point(22, 536);
            this.weightingLabel.Name = "weightingLabel";
            this.weightingLabel.Size = new System.Drawing.Size(140, 13);
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
            this.weightingEnabled.Location = new System.Drawing.Point(326, 529);
            this.weightingEnabled.Name = "weightingEnabled";
            this.weightingEnabled.Size = new System.Drawing.Size(160, 17);
            this.weightingEnabled.TabIndex = 78;
            this.weightingEnabled.Text = "Weighted Average Enabled?";
            this.weightingEnabled.UseVisualStyleBackColor = false;
            this.weightingEnabled.CheckedChanged += new System.EventHandler(this.weightingEnabled_CheckedChanged);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(567, 707);
            this.Controls.Add(this.weightingEnabled);
            this.Controls.Add(this.newColorWeight);
            this.Controls.Add(this.weightingLabel);
            this.Controls.Add(this.currentColorLabel);
            this.Controls.Add(this.colorSwatch);
            this.Controls.Add(this.settingsTitle);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.fadeEnabledCheck);
            this.Controls.Add(this.maxFadeSteps);
            this.Controls.Add(this.maxStepsLabel);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.offBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.glowStatus);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.changeSensitivity);
            this.Controls.Add(this.shouldChangeLabel);
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
            this.Controls.Add(this.speed);
            this.Controls.Add(this.speedLabel);
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
            this.Controls.Add(this.stepSize);
            this.Controls.Add(this.stepSizeLabel);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SettingsWindow";
            this.Padding = new System.Windows.Forms.Padding(30, 92, 30, 31);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsWindow_MouseDown);
            this.MouseEnter += new System.EventHandler(this.SettingsWindow_MouseEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox stepSize;
        private System.Windows.Forms.TextBox sleepSize;
        //private MetroFramework.Components.MetroStyleManager settingsStyleManager;
        private System.Windows.Forms.Label stepSizeLabel;
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
        private System.Windows.Forms.Label speedLabel;
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
        private System.Windows.Forms.TextBox changeSensitivity;
        private System.Windows.Forms.Label shouldChangeLabel;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button offBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Label statusLabel;
        public System.Windows.Forms.Label speed;
        public System.Windows.Forms.Label glowStatus;
        private System.Windows.Forms.TextBox maxFadeSteps;
        private System.Windows.Forms.Label maxStepsLabel;
        private System.Windows.Forms.CheckBox fadeEnabledCheck;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label settingsTitle;
        private System.Windows.Forms.Label currentColorLabel;
        private System.Windows.Forms.Button colorSwatch;
        private System.Windows.Forms.TextBox newColorWeight;
        private System.Windows.Forms.Label weightingLabel;
        private System.Windows.Forms.CheckBox weightingEnabled;
    }
}