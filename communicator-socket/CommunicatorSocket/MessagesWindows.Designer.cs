namespace CommunicatorSocket
{
    partial class MessagesWindows
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
            this.HistoryRichTextBox = new System.Windows.Forms.RichTextBox();
            this.HistoryLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HistoryRichTextBox
            // 
            this.HistoryRichTextBox.Location = new System.Drawing.Point(12, 50);
            this.HistoryRichTextBox.Name = "HistoryRichTextBox";
            this.HistoryRichTextBox.Size = new System.Drawing.Size(560, 399);
            this.HistoryRichTextBox.TabIndex = 0;
            this.HistoryRichTextBox.Text = "";
            // 
            // HistoryLabel
            // 
            this.HistoryLabel.AutoSize = true;
            this.HistoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HistoryLabel.Location = new System.Drawing.Point(215, 9);
            this.HistoryLabel.Name = "HistoryLabel";
            this.HistoryLabel.Size = new System.Drawing.Size(107, 31);
            this.HistoryLabel.TabIndex = 1;
            this.HistoryLabel.Text = "Historia";
            // 
            // MessagesWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.HistoryLabel);
            this.Controls.Add(this.HistoryRichTextBox);
            this.Name = "MessagesWindows";
            this.Text = "MessagesWindows";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox HistoryRichTextBox;
        private System.Windows.Forms.Label HistoryLabel;
    }
}