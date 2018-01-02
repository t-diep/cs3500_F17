///
/// @authors Tony Diep, Sona Torosyan
///
/// <summary>
using System.Windows.Forms;

/// Provides the visuals of our SpaceWars game
/// </summary>
namespace View
{
    /// <summary>
    /// The blueprint of our spacewars interface
    /// </summary>
    partial class SpaceWarsGUI
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
            this.serverNameTextBox = new System.Windows.Forms.TextBox();
            this.playerNameTextBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.scoreboardFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.clientMenuStrip = new System.Windows.Forms.MenuStrip();
            this.serverNameLabel = new System.Windows.Forms.Label();
            this.playerNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverNameTextBox
            // 
            this.serverNameTextBox.Location = new System.Drawing.Point(12, 2);
            this.serverNameTextBox.Name = "serverNameTextBox";
            this.serverNameTextBox.Size = new System.Drawing.Size(149, 22);
            this.serverNameTextBox.TabIndex = 1;
            this.serverNameTextBox.Text = "localhost";
            // 
            // playerNameTextBox
            // 
            this.playerNameTextBox.Location = new System.Drawing.Point(187, 2);
            this.playerNameTextBox.Name = "playerNameTextBox";
            this.playerNameTextBox.Size = new System.Drawing.Size(131, 22);
            this.playerNameTextBox.TabIndex = 0;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(355, 1);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(113, 23);
            this.connectButton.TabIndex = 2;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // scoreboardFlowPanel
            // 
            this.scoreboardFlowPanel.Font = new System.Drawing.Font("Verdana", 9.5F);
            this.scoreboardFlowPanel.Location = new System.Drawing.Point(690, 35);
            this.scoreboardFlowPanel.Margin = new System.Windows.Forms.Padding(1);
            this.scoreboardFlowPanel.Name = "scoreboardFlowPanel";
            this.scoreboardFlowPanel.Size = new System.Drawing.Size(231, 548);
            this.scoreboardFlowPanel.TabIndex = 5;
            this.scoreboardFlowPanel.Visible = false;
            // 
            // clientMenuStrip
            // 
            this.clientMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.clientMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.clientMenuStrip.Name = "clientMenuStrip";
            this.clientMenuStrip.Size = new System.Drawing.Size(931, 24);
            this.clientMenuStrip.TabIndex = 6;
            this.clientMenuStrip.Text = "menuStrip1";
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.AutoSize = true;
            this.serverNameLabel.Location = new System.Drawing.Point(33, 35);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(91, 17);
            this.serverNameLabel.TabIndex = 7;
            this.serverNameLabel.Text = "Server Name";
            // 
            // playerNameLabel
            // 
            this.playerNameLabel.AutoSize = true;
            this.playerNameLabel.Location = new System.Drawing.Point(204, 35);
            this.playerNameLabel.Name = "playerNameLabel";
            this.playerNameLabel.Size = new System.Drawing.Size(89, 17);
            this.playerNameLabel.TabIndex = 9;
            this.playerNameLabel.Text = "Player Name";
            // 
            // SpaceWarsGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 549);
            this.Controls.Add(this.playerNameLabel);
            this.Controls.Add(this.serverNameLabel);
            this.Controls.Add(this.scoreboardFlowPanel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.playerNameTextBox);
            this.Controls.Add(this.serverNameTextBox);
            this.Controls.Add(this.clientMenuStrip);
            this.MainMenuStrip = this.clientMenuStrip;
            this.Name = "SpaceWarsGUI";
            this.Text = "SpaceWars~";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpaceWarsGUI_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SpaceWarsGUI_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.SpaceWarsGUI_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox serverNameTextBox;
        private System.Windows.Forms.TextBox playerNameTextBox;
        private System.Windows.Forms.Button connectButton;
        private FlowLayoutPanel scoreboardFlowPanel;
        private MenuStrip clientMenuStrip;
        private Label serverNameLabel;
        private Label playerNameLabel;
    }
}