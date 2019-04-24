using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace chat_client {
    public partial class LoginForm : Form {
        private Socket socket;
        private bool isConnected;

        public LoginForm() {
            isConnected = false;
            InitializeComponent();
            log_label.Text = "";
        }

        private void login_button_Click(object sender, EventArgs e) {
            if (!isConnected) {
                MessageBox.Show("No server connection!");
                return;
            }
            
            JsonWorker jw = new JsonWorker();
            socket.Send(jw.jsonAuthentication(login_text.Text, password_text.Text));

            try {
                byte[] serverResponce = new byte[512];
                socket.Receive(serverResponce);
                jw.serverResponceParse(serverResponce);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return;
            }
            
            ChatForm cf = new ChatForm(socket, this);
            cf.Show();
            Hide();
        }

        private void connect_button_Click(object sender, EventArgs e) {
            if (isConnected) {
                log_label.Text = "Already connected";
                return;
            }

            try {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(host_text.Text), Int32.Parse(port_text.Text));

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
            }
            catch (Exception ex) {
                isConnected = false;
                MessageBox.Show("Cannot connect to " + host_text.Text + ":" + port_text.Text + "\n" + ex.Message);
                return;
            }

            JsonWorker jw = new JsonWorker();
            socket.Send(jw.jsonHandshake());

            byte[] handshakeResponce = new byte[512];
            socket.Receive(handshakeResponce);

            try {
                jw.serverResponceParse(handshakeResponce);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                isConnected = false;
                return;
            }

            log_label.Text = "Connected";
            isConnected = true;
        }

        private void registration_button_Click(object sender, EventArgs e) {
            if (!isConnected) {
                log_label.Text = "Not connected to server!";
                return;
            }

            ManageAccountForm mf = new ManageAccountForm(socket, this);
            mf.StartPosition = FormStartPosition.Manual;
            mf.Location = Location;
            mf.Size = Size;
            mf.setTypeOfForm(ManageAccountForm.ManageFormTypes.Registration);
            mf.Show();
            Hide();
        }      

        private void update_button_Click(object sender, EventArgs e) {
            if (!isConnected) {
                log_label.Text = "Not connected to server!";
                return;
            }

            ManageAccountForm mf = new ManageAccountForm(socket, this);
            mf.StartPosition = FormStartPosition.Manual;
            mf.Location = Location;
            mf.Size = Size;
            mf.setTypeOfForm(ManageAccountForm.ManageFormTypes.Update);
            mf.Show();
            Hide();
        }

        private void delete_button_Click(object sender, EventArgs e) {
            if(!isConnected) {
                log_label.Text = "Not connected to server!";
                return;
            }

            ManageAccountForm mf = new ManageAccountForm(socket, this);
            mf.StartPosition = FormStartPosition.Manual;
            mf.Location = Location;
            mf.Size = Size;
            mf.setTypeOfForm(ManageAccountForm.ManageFormTypes.Delete);
            mf.Show();
            Hide();
        }

        private void disconnect_button_Click(object sender, EventArgs e) {
            if (!isConnected) {
                log_label.Text = "Not connected!";
                return;
            }

            isConnected = false;
            socket.Close();
            log_label.Text = "Disconnected from server.";
        }
    }
}
