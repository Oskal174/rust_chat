namespace chat_client {
    partial class ManageAccountForm {
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
            this.manage_button = new System.Windows.Forms.Button();
            this.pass_text_1 = new System.Windows.Forms.TextBox();
            this.login_text_1 = new System.Windows.Forms.TextBox();
            this.pass_text_2 = new System.Windows.Forms.TextBox();
            this.new_login_label = new System.Windows.Forms.Label();
            this.new_password_label = new System.Windows.Forms.Label();
            this.confirm_password_label = new System.Windows.Forms.Label();
            this.pass_text_3 = new System.Windows.Forms.TextBox();
            this.text_label_3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(140, 312);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(167, 44);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // manage_button
            // 
            this.manage_button.Location = new System.Drawing.Point(140, 265);
            this.manage_button.Name = "manage_button";
            this.manage_button.Size = new System.Drawing.Size(167, 41);
            this.manage_button.TabIndex = 4;
            this.manage_button.Text = "Register";
            this.manage_button.UseVisualStyleBackColor = true;
            this.manage_button.Click += new System.EventHandler(this.manage_button_Click);
            // 
            // pass_text_1
            // 
            this.pass_text_1.Location = new System.Drawing.Point(140, 96);
            this.pass_text_1.Name = "pass_text_1";
            this.pass_text_1.PasswordChar = '*';
            this.pass_text_1.Size = new System.Drawing.Size(167, 22);
            this.pass_text_1.TabIndex = 2;
            // 
            // login_text_1
            // 
            this.login_text_1.Location = new System.Drawing.Point(140, 55);
            this.login_text_1.Name = "login_text_1";
            this.login_text_1.Size = new System.Drawing.Size(167, 22);
            this.login_text_1.TabIndex = 1;
            // 
            // pass_text_2
            // 
            this.pass_text_2.Location = new System.Drawing.Point(140, 136);
            this.pass_text_2.Name = "pass_text_2";
            this.pass_text_2.PasswordChar = '*';
            this.pass_text_2.Size = new System.Drawing.Size(167, 22);
            this.pass_text_2.TabIndex = 3;
            // 
            // new_login_label
            // 
            this.new_login_label.AutoSize = true;
            this.new_login_label.Location = new System.Drawing.Point(16, 55);
            this.new_login_label.Name = "new_login_label";
            this.new_login_label.Size = new System.Drawing.Size(69, 17);
            this.new_login_label.TabIndex = 5;
            this.new_login_label.Text = "New login";
            // 
            // new_password_label
            // 
            this.new_password_label.AutoSize = true;
            this.new_password_label.Location = new System.Drawing.Point(16, 96);
            this.new_password_label.Name = "new_password_label";
            this.new_password_label.Size = new System.Drawing.Size(99, 17);
            this.new_password_label.TabIndex = 6;
            this.new_password_label.Text = "New password";
            // 
            // confirm_password_label
            // 
            this.confirm_password_label.AutoSize = true;
            this.confirm_password_label.Location = new System.Drawing.Point(16, 136);
            this.confirm_password_label.Name = "confirm_password_label";
            this.confirm_password_label.Size = new System.Drawing.Size(120, 17);
            this.confirm_password_label.TabIndex = 7;
            this.confirm_password_label.Text = "Confirm password";
            // 
            // pass_text_3
            // 
            this.pass_text_3.Location = new System.Drawing.Point(140, 173);
            this.pass_text_3.Name = "pass_text_3";
            this.pass_text_3.PasswordChar = '*';
            this.pass_text_3.Size = new System.Drawing.Size(167, 22);
            this.pass_text_3.TabIndex = 4;
            // 
            // text_label_3
            // 
            this.text_label_3.AutoSize = true;
            this.text_label_3.Location = new System.Drawing.Point(16, 173);
            this.text_label_3.Name = "text_label_3";
            this.text_label_3.Size = new System.Drawing.Size(84, 17);
            this.text_label_3.TabIndex = 9;
            this.text_label_3.Text = "text_label_3";
            // 
            // ManageAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 368);
            this.Controls.Add(this.text_label_3);
            this.Controls.Add(this.pass_text_3);
            this.Controls.Add(this.confirm_password_label);
            this.Controls.Add(this.new_password_label);
            this.Controls.Add(this.new_login_label);
            this.Controls.Add(this.pass_text_2);
            this.Controls.Add(this.login_text_1);
            this.Controls.Add(this.pass_text_1);
            this.Controls.Add(this.manage_button);
            this.Controls.Add(this.cancel_button);
            this.Name = "ManageAccountForm";
            this.Text = "RegistrationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.Button manage_button;
        private System.Windows.Forms.Label new_login_label;
        private System.Windows.Forms.Label new_password_label;
        private System.Windows.Forms.Label confirm_password_label;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.TextBox pass_text_1;
        private System.Windows.Forms.TextBox login_text_1;
        private System.Windows.Forms.TextBox pass_text_2;
        private System.Windows.Forms.TextBox pass_text_3;
        private System.Windows.Forms.Label text_label_3;
    }
}