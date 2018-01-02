///
/// @author Tony Diep and Sona Torosyan 
///
using System;
using System.Drawing;

namespace SpreadsheetGUI
{
    partial class SpreadsheetForm
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
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpreadsheetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeForegroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sonaEditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tonyEditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addModifyContentsOfCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openingAnExistingSpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savingYourSpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closingThisProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraFeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoRedoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watermarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changingFocusToTextBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.valueLabel = new System.Windows.Forms.Label();
            this.contentsTextBox = new System.Windows.Forms.TextBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.cellNameLabel = new System.Windows.Forms.Label();
            this.cellNameTextBox = new System.Windows.Forms.TextBox();
            this.evaluateButton = new System.Windows.Forms.Button();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(12, 101);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(956, 560);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.SpreadsheetPanel1_Load);
            this.spreadsheetPanel1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpreadsheetPanel1_KeyDown);
            this.spreadsheetPanel1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.SpreadsheetPanel1_PreviewKeyDown);
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.otherToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1004, 28);
            this.menuStrip2.TabIndex = 2;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.saveSpreadsheetMenuItem,
            this.closeMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.Size = new System.Drawing.Size(120, 26);
            this.newMenuItem.Text = "New";
            this.newMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(120, 26);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // saveSpreadsheetMenuItem
            // 
            this.saveSpreadsheetMenuItem.Name = "saveSpreadsheetMenuItem";
            this.saveSpreadsheetMenuItem.Size = new System.Drawing.Size(120, 26);
            this.saveSpreadsheetMenuItem.Text = "Save";
            this.saveSpreadsheetMenuItem.Click += new System.EventHandler(this.SaveSpreadsheetMenuItem_Click);
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(120, 26);
            this.closeMenuItem.Text = "Close";
            this.closeMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(124, 26);
            this.undoToolStripMenuItem.Text = "Undo ";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(124, 26);
            this.redoToolStripMenuItem.Text = "Redo ";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeForegroundColorToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // changeForegroundColorToolStripMenuItem
            // 
            this.changeForegroundColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem,
            this.excelToolStripMenuItem,
            this.sonaEditionToolStripMenuItem,
            this.tonyEditionToolStripMenuItem});
            this.changeForegroundColorToolStripMenuItem.Name = "changeForegroundColorToolStripMenuItem";
            this.changeForegroundColorToolStripMenuItem.Size = new System.Drawing.Size(255, 26);
            this.changeForegroundColorToolStripMenuItem.Text = "Change Foreground Color";
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Checked = true;
            this.defaultToolStripMenuItem.CheckOnClick = true;
            this.defaultToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.defaultToolStripMenuItem.Text = "Default";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.DefaultToolStripMenuItem_Click);
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.CheckOnClick = true;
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.excelToolStripMenuItem.Text = "Excel";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.ExcelToolStripMenuItem_Click);
            // 
            // sonaEditionToolStripMenuItem
            // 
            this.sonaEditionToolStripMenuItem.CheckOnClick = true;
            this.sonaEditionToolStripMenuItem.Name = "sonaEditionToolStripMenuItem";
            this.sonaEditionToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.sonaEditionToolStripMenuItem.Text = "Sona Edition";
            this.sonaEditionToolStripMenuItem.Click += new System.EventHandler(this.SonaEditionToolStripMenuItem_Click);
            // 
            // tonyEditionToolStripMenuItem
            // 
            this.tonyEditionToolStripMenuItem.CheckOnClick = true;
            this.tonyEditionToolStripMenuItem.Name = "tonyEditionToolStripMenuItem";
            this.tonyEditionToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.tonyEditionToolStripMenuItem.Text = "Tony Edition";
            this.tonyEditionToolStripMenuItem.Click += new System.EventHandler(this.TonyEditionToolStripMenuItem_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.extraFeaturesToolStripMenuItem});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.otherToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addModifyContentsOfCellToolStripMenuItem,
            this.openingAnExistingSpreadsheetToolStripMenuItem,
            this.savingYourSpreadsheetToolStripMenuItem,
            this.closingThisProgramToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.helpToolStripMenuItem.Text = "Directions";
            // 
            // addModifyContentsOfCellToolStripMenuItem
            // 
            this.addModifyContentsOfCellToolStripMenuItem.Name = "addModifyContentsOfCellToolStripMenuItem";
            this.addModifyContentsOfCellToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.addModifyContentsOfCellToolStripMenuItem.Text = "Add/Modify Contents of Cell";
            this.addModifyContentsOfCellToolStripMenuItem.Click += new System.EventHandler(this.AddModifyContentsOfCellToolStripMenuItem_Click);
            // 
            // openingAnExistingSpreadsheetToolStripMenuItem
            // 
            this.openingAnExistingSpreadsheetToolStripMenuItem.Name = "openingAnExistingSpreadsheetToolStripMenuItem";
            this.openingAnExistingSpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.openingAnExistingSpreadsheetToolStripMenuItem.Text = "Opening an Existing Spreadsheet";
            this.openingAnExistingSpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.OpeningAnExistingSpreadsheetToolStripMenuItem_Click);
            // 
            // savingYourSpreadsheetToolStripMenuItem
            // 
            this.savingYourSpreadsheetToolStripMenuItem.Name = "savingYourSpreadsheetToolStripMenuItem";
            this.savingYourSpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.savingYourSpreadsheetToolStripMenuItem.Text = "Saving Your Spreadsheet";
            this.savingYourSpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.SavingYourSpreadsheetToolStripMenuItem_Click);
            // 
            // closingThisProgramToolStripMenuItem
            // 
            this.closingThisProgramToolStripMenuItem.Name = "closingThisProgramToolStripMenuItem";
            this.closingThisProgramToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.closingThisProgramToolStripMenuItem.Text = "Closing This Program";
            this.closingThisProgramToolStripMenuItem.Click += new System.EventHandler(this.ClosingThisProgramToolStripMenuItem_Click);
            // 
            // extraFeaturesToolStripMenuItem
            // 
            this.extraFeaturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeBackgroundToolStripMenuItem,
            this.undoRedoToolStripMenuItem,
            this.keyboardToolStripMenuItem,
            this.watermarkToolStripMenuItem,
            this.changingFocusToTextBoxToolStripMenuItem});
            this.extraFeaturesToolStripMenuItem.Name = "extraFeaturesToolStripMenuItem";
            this.extraFeaturesToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.extraFeaturesToolStripMenuItem.Text = "Extra Features :)";
            // 
            // changeBackgroundToolStripMenuItem
            // 
            this.changeBackgroundToolStripMenuItem.Name = "changeBackgroundToolStripMenuItem";
            this.changeBackgroundToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.changeBackgroundToolStripMenuItem.Text = "Change Background";
            this.changeBackgroundToolStripMenuItem.Click += new System.EventHandler(this.ChangeBackgroundToolStripMenuItem_Click);
            // 
            // undoRedoToolStripMenuItem
            // 
            this.undoRedoToolStripMenuItem.Name = "undoRedoToolStripMenuItem";
            this.undoRedoToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.undoRedoToolStripMenuItem.Text = "Undo/Redo";
            this.undoRedoToolStripMenuItem.Click += new System.EventHandler(this.UndoRedoToolStripMenuItem_Click);
            // 
            // keyboardToolStripMenuItem
            // 
            this.keyboardToolStripMenuItem.Name = "keyboardToolStripMenuItem";
            this.keyboardToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.keyboardToolStripMenuItem.Text = "Keyboard Arrow Shortcuts";
            this.keyboardToolStripMenuItem.Click += new System.EventHandler(this.KeyboardToolStripMenuItem_Click);
            // 
            // watermarkToolStripMenuItem
            // 
            this.watermarkToolStripMenuItem.Name = "watermarkToolStripMenuItem";
            this.watermarkToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.watermarkToolStripMenuItem.Text = "Watermark";
            this.watermarkToolStripMenuItem.Click += new System.EventHandler(this.WatermarkToolStripMenuItem_Click);
            // 
            // changingFocusToTextBoxToolStripMenuItem
            // 
            this.changingFocusToTextBoxToolStripMenuItem.Name = "changingFocusToTextBoxToolStripMenuItem";
            this.changingFocusToTextBoxToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.changingFocusToTextBoxToolStripMenuItem.Text = "Changing Cell Contents Without Click";
            this.changingFocusToTextBoxToolStripMenuItem.Click += new System.EventHandler(this.ChangingFocusToTextBoxToolStripMenuItem_Click);
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(256, 71);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(44, 17);
            this.valueLabel.TabIndex = 4;
            this.valueLabel.Text = "Value";
            // 
            // contentsTextBox
            // 
            this.contentsTextBox.Location = new System.Drawing.Point(580, 37);
            this.contentsTextBox.Name = "contentsTextBox";
            this.contentsTextBox.Size = new System.Drawing.Size(187, 22);
            this.contentsTextBox.TabIndex = 5;
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(347, 68);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.ReadOnly = true;
            this.valueTextBox.Size = new System.Drawing.Size(156, 22);
            this.valueTextBox.TabIndex = 6;
            // 
            // cellNameLabel
            // 
            this.cellNameLabel.AutoSize = true;
            this.cellNameLabel.Location = new System.Drawing.Point(256, 37);
            this.cellNameLabel.Name = "cellNameLabel";
            this.cellNameLabel.Size = new System.Drawing.Size(72, 17);
            this.cellNameLabel.TabIndex = 7;
            this.cellNameLabel.Text = "Cell Name";
            // 
            // cellNameTextBox
            // 
            this.cellNameTextBox.Location = new System.Drawing.Point(347, 37);
            this.cellNameTextBox.Name = "cellNameTextBox";
            this.cellNameTextBox.ReadOnly = true;
            this.cellNameTextBox.Size = new System.Drawing.Size(64, 22);
            this.cellNameTextBox.TabIndex = 8;
            // 
            // evaluateButton
            // 
            this.evaluateButton.Location = new System.Drawing.Point(623, 68);
            this.evaluateButton.Name = "evaluateButton";
            this.evaluateButton.Size = new System.Drawing.Size(92, 23);
            this.evaluateButton.TabIndex = 9;
            this.evaluateButton.Text = "Evaluate";
            this.evaluateButton.UseVisualStyleBackColor = true;
            this.evaluateButton.Click += new System.EventHandler(this.EvaluateButton_Click);
            // 
            // SpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 682);
            this.Controls.Add(this.evaluateButton);
            this.Controls.Add(this.cellNameTextBox);
            this.Controls.Add(this.cellNameLabel);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.contentsTextBox);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip2);
            this.MainMenuStrip = this.menuStrip2;
            this.Name = "SpreadsheetForm";
            this.Text = "Spreadsheet";
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       
        
        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.TextBox contentsTextBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.Label cellNameLabel;
        private System.Windows.Forms.TextBox cellNameTextBox;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSpreadsheetMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addModifyContentsOfCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openingAnExistingSpreadsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savingYourSpreadsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraFeaturesToolStripMenuItem;
        private System.Windows.Forms.Button evaluateButton;
        private System.Windows.Forms.ToolStripMenuItem changeBackgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeForegroundColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sonaEditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tonyEditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoRedoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem watermarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closingThisProgramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changingFocusToTextBoxToolStripMenuItem;
    }
}

