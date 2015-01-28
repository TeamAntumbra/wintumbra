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
            this.stepSizeLabel = new MetroFramework.Controls.MetroLabel();
            this.stepSize = new MetroFramework.Controls.MetroTextBox();
            this.stepSleepLabel = new MetroFramework.Controls.MetroLabel();
            this.sleepSize = new MetroFramework.Controls.MetroTextBox();
            this.settingsStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.DriverLabel = new MetroFramework.Controls.MetroLabel();
            this.driverExtensions = new MetroFramework.Controls.MetroComboBox();
            this.screenGrabbers = new MetroFramework.Controls.MetroComboBox();
            this.screenGrabberLabel = new MetroFramework.Controls.MetroLabel();
            this.screenProcessors = new MetroFramework.Controls.MetroComboBox();
            this.screenProcessorLabel = new MetroFramework.Controls.MetroLabel();
            this.notifiersLabel = new MetroFramework.Controls.MetroLabel();
            this.decoratorsLabel = new MetroFramework.Controls.MetroLabel();
            this.apply = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.speedLabel = new System.Windows.Forms.Label();
            this.speed = new System.Windows.Forms.Label();
            this.notifiers = new MetroFramework.Controls.MetroComboBox();
            this.decoratorToggle = new MetroFramework.Controls.MetroButton();
            this.notifierToggle = new MetroFramework.Controls.MetroButton();
            this.pollingArea = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pollingXLabel = new System.Windows.Forms.Label();
            this.pollingX = new System.Windows.Forms.Label();
            this.pollingYLabel = new System.Windows.Forms.Label();
            this.pollingY = new System.Windows.Forms.Label();
            this.pollingWidthLabel = new System.Windows.Forms.Label();
            this.pollingHeightLabel = new System.Windows.Forms.Label();
            this.pollingHeight = new System.Windows.Forms.Label();
            this.decorators = new MetroFramework.Controls.MetroComboBox();
            this.pollingWidth = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.settingsStyleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSizeLabel.Location = new System.Drawing.Point(77, 433);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(65, 19);
            this.stepSizeLabel.TabIndex = 7;
            this.stepSizeLabel.Text = "Step Size:";
            // 
            // stepSize
            // 
            this.stepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSize.Lines = new string[0];
            this.stepSize.Location = new System.Drawing.Point(204, 430);
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
            // stepSleepLabel
            // 
            this.stepSleepLabel.AutoSize = true;
            this.stepSleepLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.stepSleepLabel.Location = new System.Drawing.Point(55, 482);
            this.stepSleepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stepSleepLabel.Name = "stepSleepLabel";
            this.stepSleepLabel.Size = new System.Drawing.Size(103, 19);
            this.stepSleepLabel.TabIndex = 17;
            this.stepSleepLabel.Text = "Step Sleep: (ms)";
            // 
            // sleepSize
            // 
            this.sleepSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.sleepSize.Lines = new string[0];
            this.sleepSize.Location = new System.Drawing.Point(204, 482);
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
            // settingsStyleManager
            // 
            this.settingsStyleManager.Owner = this;
            this.settingsStyleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // DriverLabel
            // 
            this.DriverLabel.AutoSize = true;
            this.DriverLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DriverLabel.Location = new System.Drawing.Point(51, 228);
            this.DriverLabel.Name = "DriverLabel";
            this.DriverLabel.Size = new System.Drawing.Size(105, 19);
            this.DriverLabel.TabIndex = 32;
            this.DriverLabel.Text = "Driver Extension:";
            // 
            // driverExtensions
            // 
            this.driverExtensions.FormattingEnabled = true;
            this.driverExtensions.ItemHeight = 23;
            this.driverExtensions.Location = new System.Drawing.Point(186, 225);
            this.driverExtensions.Name = "driverExtensions";
            this.driverExtensions.Size = new System.Drawing.Size(348, 29);
            this.driverExtensions.TabIndex = 33;
            this.driverExtensions.UseSelectable = true;
            // 
            // screenGrabbers
            // 
            this.screenGrabbers.FormattingEnabled = true;
            this.screenGrabbers.ItemHeight = 23;
            this.screenGrabbers.Location = new System.Drawing.Point(186, 259);
            this.screenGrabbers.Name = "screenGrabbers";
            this.screenGrabbers.Size = new System.Drawing.Size(348, 29);
            this.screenGrabbers.TabIndex = 35;
            this.screenGrabbers.UseSelectable = true;
            // 
            // screenGrabberLabel
            // 
            this.screenGrabberLabel.AutoSize = true;
            this.screenGrabberLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenGrabberLabel.Location = new System.Drawing.Point(51, 262);
            this.screenGrabberLabel.Name = "screenGrabberLabel";
            this.screenGrabberLabel.Size = new System.Drawing.Size(104, 19);
            this.screenGrabberLabel.TabIndex = 34;
            this.screenGrabberLabel.Text = "Screen Grabber:";
            // 
            // screenProcessors
            // 
            this.screenProcessors.FormattingEnabled = true;
            this.screenProcessors.ItemHeight = 23;
            this.screenProcessors.Location = new System.Drawing.Point(186, 293);
            this.screenProcessors.Name = "screenProcessors";
            this.screenProcessors.Size = new System.Drawing.Size(348, 29);
            this.screenProcessors.TabIndex = 37;
            this.screenProcessors.UseSelectable = true;
            // 
            // screenProcessorLabel
            // 
            this.screenProcessorLabel.AutoSize = true;
            this.screenProcessorLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.screenProcessorLabel.Location = new System.Drawing.Point(43, 296);
            this.screenProcessorLabel.Name = "screenProcessorLabel";
            this.screenProcessorLabel.Size = new System.Drawing.Size(112, 19);
            this.screenProcessorLabel.TabIndex = 36;
            this.screenProcessorLabel.Text = "Screen Processor:";
            // 
            // notifiersLabel
            // 
            this.notifiersLabel.AutoSize = true;
            this.notifiersLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.notifiersLabel.Location = new System.Drawing.Point(96, 363);
            this.notifiersLabel.Name = "notifiersLabel";
            this.notifiersLabel.Size = new System.Drawing.Size(61, 19);
            this.notifiersLabel.TabIndex = 40;
            this.notifiersLabel.Text = "Notifiers:";
            // 
            // decoratorsLabel
            // 
            this.decoratorsLabel.AutoSize = true;
            this.decoratorsLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.decoratorsLabel.Location = new System.Drawing.Point(80, 329);
            this.decoratorsLabel.Name = "decoratorsLabel";
            this.decoratorsLabel.Size = new System.Drawing.Size(76, 19);
            this.decoratorsLabel.TabIndex = 38;
            this.decoratorsLabel.Text = "Decorators:";
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(375, 399);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(159, 23);
            this.apply.TabIndex = 43;
            this.apply.Text = "Apply Extension Changes";
            this.apply.UseSelectable = true;
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.metroLabel1.Location = new System.Drawing.Point(96, 363);
            this.metroLabel1.Name = "notifiersLabel";
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
            this.speed.Location = new System.Drawing.Point(380, 114);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(13, 13);
            this.speed.TabIndex = 45;
            this.speed.Text = "0";
            // 
            // notifiers
            // 
            this.notifiers.FormattingEnabled = true;
            this.notifiers.ItemHeight = 23;
            this.notifiers.Location = new System.Drawing.Point(186, 363);
            this.notifiers.Name = "notifiers";
            this.notifiers.Size = new System.Drawing.Size(236, 29);
            this.notifiers.TabIndex = 47;
            this.notifiers.UseSelectable = true;
            // 
            // decoratorToggle
            // 
            this.decoratorToggle.Location = new System.Drawing.Point(428, 328);
            this.decoratorToggle.Name = "decoratorToggle";
            this.decoratorToggle.Size = new System.Drawing.Size(106, 29);
            this.decoratorToggle.TabIndex = 48;
            this.decoratorToggle.Text = "Toggle Selected";
            this.decoratorToggle.UseSelectable = true;
            this.decoratorToggle.UseVisualStyleBackColor = true;
            this.decoratorToggle.Click += new System.EventHandler(this.decoratorToggle_Click);
            // 
            // notifierToggle
            // 
            this.notifierToggle.Location = new System.Drawing.Point(428, 363);
            this.notifierToggle.Name = "notifierToggle";
            this.notifierToggle.Size = new System.Drawing.Size(106, 29);
            this.notifierToggle.TabIndex = 49;
            this.notifierToggle.Text = "Toggle Selected";
            this.notifierToggle.UseSelectable = true;
            this.notifierToggle.UseVisualStyleBackColor = true;
            this.notifierToggle.Click += new System.EventHandler(this.notifierToggle_Click);
            // 
            // pollingArea
            // 
            this.pollingArea.Location = new System.Drawing.Point(284, 173);
            this.pollingArea.Name = "pollingArea";
            this.pollingArea.Size = new System.Drawing.Size(138, 35);
            this.pollingArea.TabIndex = 51;
            this.pollingArea.Text = "Set Screen Grabber Area";
            this.pollingArea.UseVisualStyleBackColor = true;
            this.pollingArea.Click += new System.EventHandler(this.pollingArea_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(101, 71);
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
            this.decorators.FormattingEnabled = true;
            this.decorators.ItemHeight = 23;
            this.decorators.Location = new System.Drawing.Point(186, 328);
            this.decorators.Name = "decorators";
            this.decorators.Size = new System.Drawing.Size(236, 29);
            this.decorators.TabIndex = 46;
            this.decorators.UseSelectable = true;
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
            // SettingsWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(567, 707);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SettingsWindow";
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
            this.MouseEnter += new System.EventHandler(this.SettingsWindow_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.settingsStyleManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox stepSize;
        private MetroFramework.Controls.MetroTextBox sleepSize;
        private MetroFramework.Components.MetroStyleManager settingsStyleManager;
        private MetroFramework.Controls.MetroLabel stepSizeLabel;
        private MetroFramework.Controls.MetroLabel stepSleepLabel;
        private MetroFramework.Controls.MetroComboBox driverExtensions;
        private MetroFramework.Controls.MetroLabel DriverLabel;
        private MetroFramework.Controls.MetroComboBox screenProcessors;
        private MetroFramework.Controls.MetroLabel screenProcessorLabel;
        private MetroFramework.Controls.MetroComboBox screenGrabbers;
        private MetroFramework.Controls.MetroLabel screenGrabberLabel;
        private MetroFramework.Controls.MetroLabel notifiersLabel;
        private MetroFramework.Controls.MetroLabel decoratorsLabel;
        private MetroFramework.Controls.MetroButton apply;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private System.Windows.Forms.Label speedLabel;
        public System.Windows.Forms.Label speed;
        private MetroFramework.Controls.MetroButton notifierToggle;
        private MetroFramework.Controls.MetroButton decoratorToggle;
        private MetroFramework.Controls.MetroComboBox notifiers;
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
        private MetroFramework.Controls.MetroComboBox decorators;
    }
}