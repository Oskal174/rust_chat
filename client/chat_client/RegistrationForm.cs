using System.Windows.Forms;
using System.Net.Sockets;
using System;

namespace chat_client {
    public partial class RegistrationForm : Form {
        private Socket socket;
        private LoginForm loginForm;

        public RegistrationForm(Socket socket, LoginForm loginForm) {
            this.socket = socket;
            this.loginForm = loginForm;
            InitializeComponent();
        }

        private void cancel_button_Click(object sender, System.EventArgs e) {
            loginForm.StartPosition = FormStartPosition.Manual;
            loginForm.Location = Location;
            loginForm.Size = Size;
            loginForm.Show();
            Hide();
        }

        private void register_button_Click(object sender, System.EventArgs e) {
            if (new_password.Text != again_password.Text) {
                MessageBox.Show("Passwords do not match");
                return;
            }

            JsonWorker jw = new JsonWorker();
            socket.Send(jw.jsonRegistration(new_login.Text, new_password.Text));

            try {
                byte[] serverResponce = new byte[512];
                socket.Receive(serverResponce);
                jw.serverResponceParse(serverResponce);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return;
            }

            MessageBox.Show("Registration successful");
            loginForm.StartPosition = FormStartPosition.Manual;
            loginForm.Location = Location;
            loginForm.Size = Size;
            loginForm.Show();
            Hide();
        }
    }
}
