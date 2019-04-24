using System.Windows.Forms;
using System.Net.Sockets;
using System;

namespace chat_client {
    public partial class ManageAccountForm : Form {
        public enum ManageFormTypes {
            Registration = 0,
            Update       = 1,
            Delete       = 2
        }

        private Socket socket;
        private LoginForm loginForm;
        private ManageFormTypes form_type;

        public ManageAccountForm(Socket socket, LoginForm loginForm) {
            this.socket = socket;
            this.loginForm = loginForm;
            InitializeComponent();
        }

        public void setTypeOfForm(ManageFormTypes type) {
            form_type = type;
            switch (type) {
                case ManageFormTypes.Registration:
                    new_login_label.Text = "New login";
                    new_password_label.Text = "Password";
                    confirm_password_label.Text = "Confirm password";
                    pass_text_2.Visible = true;
                    text_label_3.Text = "";
                    pass_text_3.Visible = false;
                    manage_button.Text = "registration";
                    break;

                case ManageFormTypes.Update:
                    new_login_label.Text = "Login";
                    new_password_label.Text = "Accoutn password";
                    confirm_password_label.Text = "New password";
                    pass_text_2.Visible = true;
                    text_label_3.Text = "Confirm password";
                    pass_text_3.Visible = true;
                    manage_button.Text = "update";
                    break;

                case ManageFormTypes.Delete:
                    new_login_label.Text = "Login";
                    new_password_label.Text = "Confirm password";
                    confirm_password_label.Text = "";
                    pass_text_2.Visible = false;
                    text_label_3.Text = "";
                    pass_text_3.Visible = false;
                    manage_button.Text = "delete";
                    break;

                default:
                    break;
            }
        }

        private void cancel_button_Click(object sender, System.EventArgs e) {
            loginForm.StartPosition = FormStartPosition.Manual;
            loginForm.Location = Location;
            loginForm.Size = Size;
            loginForm.Show();
            Close();
        }

        private void manage_button_Click(object sender, System.EventArgs e) {
            JsonWorker jw = new JsonWorker();

            switch (form_type) {
                case ManageFormTypes.Registration:
                    if (pass_text_1.Text != pass_text_2.Text) {
                        MessageBox.Show("Passwords do not match");
                        return;
                    }

                    socket.Send(jw.jsonRegistration(login_text_1.Text, pass_text_1.Text));

                    try {
                        byte[] serverResponce = new byte[512];
                        socket.Receive(serverResponce);
                        jw.serverResponceParse(serverResponce);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    
                    break;

                case ManageFormTypes.Update:
                    if (pass_text_2.Text != pass_text_3.Text) {
                        MessageBox.Show("Passwords do not match");
                        return;
                    }

                    socket.Send(jw.jsonUpdate(login_text_1.Text, pass_text_1.Text, pass_text_2.Text));

                    try {
                        byte[] serverResponce = new byte[512];
                        socket.Receive(serverResponce);
                        jw.serverResponceParse(serverResponce);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.ToString());
                        return;
                    }

                    break;

                case ManageFormTypes.Delete:
                    socket.Send(jw.jsonDelete(login_text_1.Text, pass_text_1.Text));

                    try {
                        byte[] serverResponce = new byte[512];
                        socket.Receive(serverResponce);
                        jw.serverResponceParse(serverResponce);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.ToString());
                        return;
                    }

                    break;

                default:
                    break;
            }

            loginForm.StartPosition = FormStartPosition.Manual;
            loginForm.Location = Location;
            loginForm.Size = Size;
            loginForm.Show();
            Close();
        }
    }
}
