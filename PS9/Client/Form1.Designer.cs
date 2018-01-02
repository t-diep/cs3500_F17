namespace Client
{
    partial class Form1
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
            this.serverAddress = new System.Windows.Forms.TextBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.host = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverAddress
            // 
            this.serverAddress.Location = new System.Drawing.Point(140, 18);
            this.serverAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.serverAddress.Name = "serverAddress";
            this.serverAddress.Size = new System.Drawing.Size(148, 26);
            this.serverAddress.TabIndex = 2;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(382, 20);
            this.nameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(148, 26);
            this.nameBox.TabIndex = 3;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(538, 16);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(112, 35);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // host
            // 
            this.host.AutoSize = true;
            this.host.Location = new System.Drawing.Point(18, 23);
            this.host.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(118, 20);
            this.host.TabIndex = 5;
            this.host.Text = "Server Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(325, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 782);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.host);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.serverAddress);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "SpaceWars (GOTY with DLC) ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox serverAddress;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label host;
        private System.Windows.Forms.Label label1;
    }
}

