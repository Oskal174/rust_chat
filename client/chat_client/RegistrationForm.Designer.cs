namespace chat_client {
    partial class RegistrationForm {
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
            this.cancel_button = new System.Windows.Forms.Button();
            this.register_button = new System.Windows.Forms.Button();
            this.new_password = new System.Windows.Forms.TextBox();
            this.new_login = new System.Windows.Forms.TextBox();
            this.again_password = new System.Windows.Forms.TextBox();
            this.new_login_label = new System.Windows.Forms.Label();
            this.new_password_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(140, 261);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(167, 44);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // register_button
            // 
            this.register_button.Location = new System.Drawing.Point(140, 194);
            this.register_button.Name = "register_button";
            this.register_button.Size = new System.Drawing.Size(167, 41);
            this.register_button.TabIndex = 4;
            this.register_button.Text = "Register";
            this.register_button.UseVisualStyleBackColor = true;
            this.register_button.Click += new System.EventHandler(this.register_button_Click);
            // 
            // new_password
            // 
            this.new_password.Location = new System.Drawing.Point(140, 96);
            this.new_password.Name = "new_password";
            this.new_password.PasswordChar = '*';
            this.new_password.Size = new System.Drawing.Size(167, 22);
            this.new_password.TabIndex = 2;
            // 
            // new_login
            // 
            this.new_login.Location = new System.Drawing.Point(140, 55);
            this.new_login.Name = "new_login";
            this.new_login.Size = new System.Drawing.Size(167, 22);
            this.new_login.TabIndex = 1;
            // 
            // again_password
            // 
            this.again_password.Location = new System.Drawing.Point(140, 136);
            this.again_password.Name = "again_password";
            this.again_password.PasswordChar = '*';
            this.again_password.Size = new System.Drawing.Size(167, 22);
            this.again_password.TabIndex = 3;
            // 
            // new_login_label
            // 
            this.new_login_label.AutoSize = true;
            this.new_login_label.Location = new System.Drawing.Point(65, 55);
            this.new_login_label.Name = "new_login_label";
            this.new_login_label.Size = new System.Drawing.Size(69, 17);
            this.new_login_label.TabIndex = 5;
            this.new_login_label.Text = "New login";
            // 
            // new_password_label
            // 
            this.new_password_label.AutoSize = true;
            this.new_password_label.Location = new System.Drawing.Point(35, 96);
            this.new_password_label.Name = "new_password_label";
            this.new_password_label.Size = new System.Drawing.Size(99, 17);
            this.new_password_label.TabIndex = 6;
            this.new_password_label.Text = "New password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Confirm password";
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 376);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.new_password_label);
            this.Controls.Add(this.new_login_label);
            this.Controls.Add(this.again_password);
            this.Controls.Add(this.new_login);
            this.Controls.Add(this.new_password);
            this.Controls.Add(this.register_button);
            this.Controls.Add(this.cancel_button);
            this.Name = "RegistrationForm";
            this.Text = "RegistrationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button register_button;
        private System.Windows.Forms.TextBox new_password;
        private System.Windows.Forms.TextBox new_login;
        private System.Windows.Forms.TextBox again_password;
        private System.Windows.Forms.Label new_login_label;
        private System.Windows.Forms.Label new_password_label;
        private System.Windows.Forms.Label label3;
    }
}