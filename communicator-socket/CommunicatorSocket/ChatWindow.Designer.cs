﻿namespace CommunicatorSocket
{
    partial class ChatWindow
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
            this.MessagesRichTextBox = new System.Windows.Forms.RichTextBox();
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MessagesRichTextBox
            // 
            this.MessagesRichTextBox.Location = new System.Drawing.Point(12, 12);
            this.MessagesRichTextBox.Name = "MessagesRichTextBox";
            this.MessagesRichTextBox.ReadOnly = true;
            this.MessagesRichTextBox.Size = new System.Drawing.Size(560, 350);
            this.MessagesRichTextBox.TabIndex = 0;
            this.MessagesRichTextBox.Text = "";
            this.MessagesRichTextBox.Click += new System.EventHandler(this.MessagesRichTextBox_Click);
            this.MessagesRichTextBox.EnabledChanged += new System.EventHandler(this.MessagesRichTextBox_EnabledChanged);
            this.MessagesRichTextBox.TextChanged += new System.EventHandler(this.MessagesRichTextBox_TextChanged);
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.Location = new System.Drawing.Point(12, 388);
            this.MessageTextBox.Multiline = true;
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(425, 54);
            this.MessageTextBox.TabIndex = 1;
            this.MessageTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MessageTextBox_KeyPress);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(443, 388);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(129, 55);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Location = new System.Drawing.Point(12, 372);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(50, 13);
            this.MessageLabel.TabIndex = 3;
            this.MessageLabel.Text = "Message";
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.MessagesRichTextBox);
            this.Name = "ChatWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Okno wiadomości - Darek";
            this.Activated += new System.EventHandler(this.ChatWindow_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatWindow_FormClosing);
            this.Shown += new System.EventHandler(this.ChatWindow_Shown);
            this.Enter += new System.EventHandler(this.ChatWindow_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox MessagesRichTextBox;
        private System.Windows.Forms.TextBox MessageTextBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Label MessageLabel;

    }
}