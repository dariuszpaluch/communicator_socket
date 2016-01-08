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
            this.UsersPanel = new System.Windows.Forms.Panel();
            this.ContactsListBox = new System.Windows.Forms.ListBox();
            this.MessageButton = new System.Windows.Forms.Button();
            this.UsersLabel = new System.Windows.Forms.Label();
            this.UsersPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsersPanel
            // 
            this.UsersPanel.Controls.Add(this.ContactsListBox);
            this.UsersPanel.Controls.Add(this.MessageButton);
            this.UsersPanel.Controls.Add(this.UsersLabel);
            this.UsersPanel.Location = new System.Drawing.Point(15, 12);
            this.UsersPanel.Name = "UsersPanel";
            this.UsersPanel.Size = new System.Drawing.Size(277, 447);
            this.UsersPanel.TabIndex = 5;
            // 
            // ContactsListBox
            // 
            this.ContactsListBox.FormattingEnabled = true;
            this.ContactsListBox.Location = new System.Drawing.Point(6, 32);
            this.ContactsListBox.Name = "ContactsListBox";
            this.ContactsListBox.Size = new System.Drawing.Size(265, 355);
            this.ContactsListBox.TabIndex = 6;
            this.ContactsListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ContactsListBox_MouseDoubleClick);
            // 
            // MessageButton
            // 
            this.MessageButton.Location = new System.Drawing.Point(3, 405);
            this.MessageButton.Name = "MessageButton";
            this.MessageButton.Size = new System.Drawing.Size(268, 31);
            this.MessageButton.TabIndex = 5;
            this.MessageButton.Text = "Otwórz okno rozmowy";
            this.MessageButton.UseVisualStyleBackColor = true;
            this.MessageButton.Click += new System.EventHandler(this.MessageButton_Click);
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 469);
            this.Controls.Add(this.UsersPanel);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.UsersPanel.ResumeLayout(false);
            this.UsersPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel UsersPanel;
        private System.Windows.Forms.Button MessageButton;
        private System.Windows.Forms.Label UsersLabel;
        private System.Windows.Forms.ListBox ContactsListBox;
    }
}