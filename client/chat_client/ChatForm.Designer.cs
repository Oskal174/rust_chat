namespace chat_client {
    partial class ChatForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.chatText = new System.Windows.Forms.TextBox();
            this.logout = new System.Windows.Forms.Button();
            this.updateChatWorker = new System.ComponentModel.BackgroundWorker();
            this.messageText = new System.Windows.Forms.TextBox();
            this.sendMessageButton = new System.Windows.Forms.Button();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chatText
            // 
            this.chatText.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.chatText.Location = new System.Drawing.Point(13, 13);
            this.chatText.Multiline = true;
            this.chatText.Name = "chatText";
            this.chatText.ReadOnly = true;
            this.chatText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.chatText.Size = new System.Drawing.Size(775, 284);
            this.chatText.TabIndex = 0;
            // 
            // logout
            // 
            this.logout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logout.Location = new System.Drawing.Point(12, 446);
            this.logout.Name = "logout";
            this.logout.Size = new System.Drawing.Size(146, 33);
            this.logout.TabIndex = 1;
            this.logout.Text = "logout";
            this.logout.UseVisualStyleBackColor = true;
            this.logout.Click += new System.EventHandler(this.logout_Click);
            // 
            // updateChatWorker
            // 
            this.updateChatWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateChatWorker_DoWork);
            // 
            // messageText
            // 
            this.messageText.Location = new System.Drawing.Point(12, 333);
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(567, 22);
            this.messageText.TabIndex = 2;
            this.messageText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageText_KeyDown);
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.Location = new System.Drawing.Point(594, 333);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(166, 31);
            this.sendMessageButton.TabIndex = 3;
            this.sendMessageButton.Text = "Отправить";
            this.sendMessageButton.UseVisualStyleBackColor = true;
            this.sendMessageButton.Click += new System.EventHandler(this.sendMessageButton_Click);
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(13, 310);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(87, 17);
            this.usernameLabel.TabIndex = 4;
            this.usernameLabel.Text = "Current user";
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 491);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.sendMessageButton);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.logout);
            this.Controls.Add(this.chatText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox chatText;
        private System.Windows.Forms.Button logout;
        private System.ComponentModel.BackgroundWorker updateChatWorker;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.Button sendMessageButton;
        private System.Windows.Forms.Label usernameLabel;
    }
}