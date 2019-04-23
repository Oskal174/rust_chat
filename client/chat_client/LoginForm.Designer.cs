namespace chat_client {
    partial class LoginForm {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.login_button = new System.Windows.Forms.Button();
            this.login_text = new System.Windows.Forms.TextBox();
            this.password_text = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.port_text = new System.Windows.Forms.TextBox();
            this.host_text = new System.Windows.Forms.TextBox();
            this.host_label = new System.Windows.Forms.Label();
            this.port_label = new System.Windows.Forms.Label();
            this.connect_button = new System.Windows.Forms.Button();
            this.log_label = new System.Windows.Forms.Label();
            this.registration_button = new System.Windows.Forms.Button();
            this.disconnect_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // login_button
            // 
            this.login_button.Location = new System.Drawing.Point(38, 250);
            this.login_button.Name = "login_button";
            this.login_button.Size = new System.Drawing.Size(325, 38);
            this.login_button.TabIndex = 0;
            this.login_button.Text = "login to server";
            this.login_button.UseVisualStyleBackColor = true;
            this.login_button.Click += new System.EventHandler(this.login_button_Click);
            // 
            // login_text
            // 
            this.login_text.Location = new System.Drawing.Point(109, 177);
            this.login_text.Name = "login_text";
            this.login_text.Size = new System.Drawing.Size(254, 22);
            this.login_text.TabIndex = 1;
            // 
            // password_text
            // 
            this.password_text.Location = new System.Drawing.Point(109, 205);
            this.password_text.Name = "password_text";
            this.password_text.PasswordChar = '*';
            this.password_text.Size = new System.Drawing.Size(254, 22);
            this.password_text.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "login:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "password:";
            // 
            // port_text
            // 
            this.port_text.Location = new System.Drawing.Point(109, 53);
            this.port_text.Name = "port_text";
            this.port_text.Size = new System.Drawing.Size(254, 22);
            this.port_text.TabIndex = 5;
            this.port_text.Text = "9090";
            // 
            // host_text
            // 
            this.host_text.Location = new System.Drawing.Point(109, 25);
            this.host_text.Name = "host_text";
            this.host_text.Size = new System.Drawing.Size(254, 22);
            this.host_text.TabIndex = 6;
            this.host_text.Text = "127.0.0.1";
            // 
            // host_label
            // 
            this.host_label.AutoSize = true;
            this.host_label.Location = new System.Drawing.Point(22, 25);
            this.host_label.Name = "host_label";
            this.host_label.Size = new System.Drawing.Size(85, 17);
            this.host_label.TabIndex = 7;
            this.host_label.Text = "Server host:";
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(24, 53);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(83, 17);
            this.port_label.TabIndex = 8;
            this.port_label.Text = "Server port:";
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(109, 94);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(254, 30);
            this.connect_button.TabIndex = 9;
            this.connect_button.Text = "connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // log_label
            // 
            this.log_label.AutoSize = true;
            this.log_label.Location = new System.Drawing.Point(12, 350);
            this.log_label.Name = "log_label";
            this.log_label.Size = new System.Drawing.Size(0, 17);
            this.log_label.TabIndex = 10;
            // 
            // registration_button
            // 
            this.registration_button.Location = new System.Drawing.Point(38, 294);
            this.registration_button.Name = "registration_button";
            this.registration_button.Size = new System.Drawing.Size(325, 36);
            this.registration_button.TabIndex = 11;
            this.registration_button.Text = "registration";
            this.registration_button.UseVisualStyleBackColor = true;
            this.registration_button.Click += new System.EventHandler(this.registration_button_Click);
            // 
            // disconnect_button
            // 
            this.disconnect_button.Location = new System.Drawing.Point(109, 131);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(254, 27);
            this.disconnect_button.TabIndex = 12;
            this.disconnect_button.Text = "disconnect";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 374);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.registration_button);
            this.Controls.Add(this.log_label);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.port_label);
            this.Controls.Add(this.host_label);
            this.Controls.Add(this.host_text);
            this.Controls.Add(this.port_text);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.password_text);
            this.Controls.Add(this.login_text);
            this.Controls.Add(this.login_button);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.Button login_button;
        private System.Windows.Forms.TextBox login_text;
        private System.Windows.Forms.TextBox password_text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox port_text;
        private System.Windows.Forms.TextBox host_text;
        private System.Windows.Forms.Label host_label;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.Label log_label;
        private System.Windows.Forms.Button registration_button;
        private System.Windows.Forms.Button disconnect_button;
    }
}

