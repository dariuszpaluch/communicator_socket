namespace CommunicatorSocket
{
    partial class MainWindow
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
            this.ExtendedOptionPanel = new System.Windows.Forms.Panel();
            this.HistoryButton = new System.Windows.Forms.Button();
            this.UsersPanel = new System.Windows.Forms.Panel();
            this.MessageButton = new System.Windows.Forms.Button();
            this.UsersLabel = new System.Windows.Forms.Label();
            this.UsersRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ExtendOptionLabel = new System.Windows.Forms.Label();
            this.ExtendedOptionPanel.SuspendLayout();
            this.UsersPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExtendedOptionPanel
            // 
            this.ExtendedOptionPanel.Controls.Add(this.ExtendOptionLabel);
            this.ExtendedOptionPanel.Controls.Add(this.HistoryButton);
            this.ExtendedOptionPanel.Location = new System.Drawing.Point(15, 12);
            this.ExtendedOptionPanel.Name = "ExtendedOptionPanel";
            this.ExtendedOptionPanel.Size = new System.Drawing.Size(277, 60);
            this.ExtendedOptionPanel.TabIndex = 4;
            // 
            // HistoryButton
            // 
            this.HistoryButton.Location = new System.Drawing.Point(3, 25);
            this.HistoryButton.Name = "HistoryButton";
            this.HistoryButton.Size = new System.Drawing.Size(271, 23);
            this.HistoryButton.TabIndex = 5;
            this.HistoryButton.Text = "Wyświetl historię";
            this.HistoryButton.UseVisualStyleBackColor = true;
            // 
            // UsersPanel
            // 
            this.UsersPanel.Controls.Add(this.MessageButton);
            this.UsersPanel.Controls.Add(this.UsersLabel);
            this.UsersPanel.Controls.Add(this.UsersRichTextBox);
            this.UsersPanel.Location = new System.Drawing.Point(15, 78);
            this.UsersPanel.Name = "UsersPanel";
            this.UsersPanel.Size = new System.Drawing.Size(277, 439);
            this.UsersPanel.TabIndex = 5;
            // 
            // MessageButton
            // 
            this.MessageButton.Location = new System.Drawing.Point(3, 405);
            this.MessageButton.Name = "MessageButton";
            this.MessageButton.Size = new System.Drawing.Size(268, 31);
            this.MessageButton.TabIndex = 5;
            this.MessageButton.Text = "Otwórz okno rozmowy";
            this.MessageButton.UseVisualStyleBackColor = true;
            // 
            // UsersLabel
            // 
            this.UsersLabel.AutoSize = true;
            this.UsersLabel.Location = new System.Drawing.Point(3, 16);
            this.UsersLabel.Name = "UsersLabel";
            this.UsersLabel.Size = new System.Drawing.Size(157, 13);
            this.UsersLabel.TabIndex = 4;
            this.UsersLabel.Text = "Lista dostępnych użytkowników";
            // 
            // UsersRichTextBox
            // 
            this.UsersRichTextBox.Location = new System.Drawing.Point(3, 32);
            this.UsersRichTextBox.Name = "UsersRichTextBox";
            this.UsersRichTextBox.Size = new System.Drawing.Size(268, 367);
            this.UsersRichTextBox.TabIndex = 3;
            this.UsersRichTextBox.Text = "";
            // 
            // ExtendOptionLabel
            // 
            this.ExtendOptionLabel.AutoSize = true;
            this.ExtendOptionLabel.Location = new System.Drawing.Point(3, 9);
            this.ExtendOptionLabel.Name = "ExtendOptionLabel";
            this.ExtendOptionLabel.Size = new System.Drawing.Size(89, 13);
            this.ExtendOptionLabel.TabIndex = 6;
            this.ExtendOptionLabel.Text = "Dodatkowe okna";
            this.ExtendOptionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 527);
            this.Controls.Add(this.UsersPanel);
            this.Controls.Add(this.ExtendedOptionPanel);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ExtendedOptionPanel.ResumeLayout(false);
            this.ExtendedOptionPanel.PerformLayout();
            this.UsersPanel.ResumeLayout(false);
            this.UsersPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ExtendedOptionPanel;
        private System.Windows.Forms.Label ExtendOptionLabel;
        private System.Windows.Forms.Button HistoryButton;
        private System.Windows.Forms.Panel UsersPanel;
        private System.Windows.Forms.Button MessageButton;
        private System.Windows.Forms.Label UsersLabel;
        private System.Windows.Forms.RichTextBox UsersRichTextBox;
    }
}