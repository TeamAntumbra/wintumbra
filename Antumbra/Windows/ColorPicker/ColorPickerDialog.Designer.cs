using System;

namespace Antumbra.Glow
{
  public partial class ColorPickerDialog
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
      if (disposing && (components != null))
      {
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
      this.okButton = new MetroFramework.Controls.MetroButton();
      this.cancelButton = new MetroFramework.Controls.MetroButton();
      this.previewPanel = new System.Windows.Forms.Panel();
      this.loadPaletteButton = new MetroFramework.Controls.MetroButton();
      this.savePaletteButton = new MetroFramework.Controls.MetroButton();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.screenColorPicker = new ScreenColorPicker();
      this.colorWheel = new ColorWheel();
      this.colorEditor = new ColorEditor();
      this.colorGrid = new ColorGrid();
      this.colorEditorManager = new ColorEditorManager();
      this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
      this.ForeColor = System.Drawing.Color.White;
      ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
      this.SuspendLayout();
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.Location = new System.Drawing.Point(453, 32);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 1;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      this.okButton.Theme = MetroFramework.MetroThemeStyle.Dark;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(453, 61);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      this.cancelButton.Theme = MetroFramework.MetroThemeStyle.Dark;
      // 
      // previewPanel
      // 
      this.previewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.previewPanel.Location = new System.Drawing.Point(453, 223);
      this.previewPanel.Name = "previewPanel";
      this.previewPanel.Size = new System.Drawing.Size(75, 47);
      this.previewPanel.TabIndex = 3;
      // 
      // loadPaletteButton
      // 
      this.loadPaletteButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.loadPaletteButton.Image = global::Antumbra.Glow.Properties.Resources.LoadPalette;
      this.loadPaletteButton.Location = new System.Drawing.Point(12, 167);
      this.loadPaletteButton.Name = "loadPaletteButton";
      this.loadPaletteButton.Size = new System.Drawing.Size(23, 23);
      this.loadPaletteButton.TabIndex = 5;
      this.toolTip.SetToolTip(this.loadPaletteButton, "Load Palette");
      this.loadPaletteButton.UseVisualStyleBackColor = false;
      this.loadPaletteButton.Click += new System.EventHandler(this.loadPaletteButton_Click);
      // 
      // savePaletteButton
      // 
      this.savePaletteButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.savePaletteButton.Image = global::Antumbra.Glow.Properties.Resources.SavePalette;
      this.savePaletteButton.Location = new System.Drawing.Point(34, 167);
      this.savePaletteButton.Name = "savePaletteButton";
      this.savePaletteButton.Size = new System.Drawing.Size(23, 23);
      this.savePaletteButton.TabIndex = 6;
      this.toolTip.SetToolTip(this.savePaletteButton, "Save Palette");
      this.savePaletteButton.UseVisualStyleBackColor = false;
      this.savePaletteButton.Click += new System.EventHandler(this.savePaletteButton_Click);
      // 
      // screenColorPicker
      // 
      this.screenColorPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.screenColorPicker.Color = System.Drawing.Color.Black;
      this.screenColorPicker.Image = global::Antumbra.Glow.Properties.Resources.eyedropper;
      this.screenColorPicker.Location = new System.Drawing.Point(453, 103);
      this.screenColorPicker.Name = "screenColorPicker";
      this.screenColorPicker.Size = new System.Drawing.Size(73, 85);
      this.toolTip.SetToolTip(this.screenColorPicker, "Click and drag to select screen color");
      this.screenColorPicker.Zoom = 6;
      // 
      // colorWheel
      // 
      this.colorWheel.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this.colorWheel.Location = new System.Drawing.Point(12, 32);
      this.colorWheel.Name = "colorWheel";
      this.colorWheel.Size = new System.Drawing.Size(192, 147);
      this.colorWheel.TabIndex = 4;
      // 
      // colorEditor
      // 
      this.colorEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.colorEditor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this.colorEditor.Location = new System.Drawing.Point(210, 32);
      this.colorEditor.Name = "colorEditor";
      this.colorEditor.Size = new System.Drawing.Size(230, 238);
      this.colorEditor.TabIndex = 0;
      // 
      // colorGrid
      // 
      this.colorGrid.AutoAddColors = false;
      this.colorGrid.CellBorderStyle = ColorCellBorderStyle.None;
      this.colorGrid.EditMode = ColorEditingMode.Both;
      this.colorGrid.Location = new System.Drawing.Point(12, 196);
      this.colorGrid.Name = "colorGrid";
      this.colorGrid.Padding = new System.Windows.Forms.Padding(0);
      this.colorGrid.Palette = ColorPalette.Paint;
      this.colorGrid.SelectedCellStyle = ColorGridSelectedCellStyle.Standard;
      this.colorGrid.ShowCustomColors = false;
      this.colorGrid.Size = new System.Drawing.Size(192, 72);
      this.colorGrid.Spacing = new System.Drawing.Size(0, 0);
      this.colorGrid.TabIndex = 7;
      this.colorGrid.EditingColor += new EventHandler<EditColorCancelEventArgs>(this.colorGrid_EditingColor);
      // 
      // colorEditorManager
      // 
      this.colorEditorManager.ColorEditor = this.colorEditor;
      this.colorEditorManager.ColorGrid = this.colorGrid;
      this.colorEditorManager.ColorWheel = this.colorWheel;
      this.colorEditorManager.ScreenColorPicker = this.screenColorPicker;
      this.colorEditorManager.ColorChanged += new System.EventHandler(this.colorEditorManager_ColorChanged);
      // 
      // metroStyleManager1
      // 
      this.metroStyleManager1.Owner = this;
      this.metroStyleManager1.Style = MetroFramework.MetroColorStyle.Magenta;
      this.metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Dark;
      // 
      // ColorPickerDialog
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(540, 292);
      this.Controls.Add(this.savePaletteButton);
      this.Controls.Add(this.loadPaletteButton);
      this.Controls.Add(this.previewPanel);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.screenColorPicker);
      this.Controls.Add(this.colorWheel);
      this.Controls.Add(this.colorEditor);
      this.Controls.Add(this.colorGrid);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Resizable = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Theme = MetroFramework.MetroThemeStyle.Dark;
      this.Name = "ColorPickerDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
      this.Text = "";
      this.Opacity = .9;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
      
    private ColorGrid colorGrid;
    private ColorEditor colorEditor;
    private ColorWheel colorWheel;
    private ColorEditorManager colorEditorManager;
    private ScreenColorPicker screenColorPicker;
    private MetroFramework.Controls.MetroButton okButton;
    private MetroFramework.Controls.MetroButton cancelButton;
    public System.Windows.Forms.Panel previewPanel;
    private MetroFramework.Controls.MetroButton loadPaletteButton;
    private MetroFramework.Controls.MetroButton savePaletteButton;
    private System.Windows.Forms.ToolTip toolTip;
    private MetroFramework.Components.MetroStyleManager metroStyleManager1;
  }
}