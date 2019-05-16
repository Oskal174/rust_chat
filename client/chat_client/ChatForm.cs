using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;


namespace chat_client {
    public partial class ChatForm : Form {
        private DateTimeWorker dateTimeWorker = new DateTimeWorker();
        private JsonWorker jsonWorker = new JsonWorker();
        private Socket socket;
        private LoginForm loginForm;
        private bool isNeedStopWorker;

        public ChatForm(Socket socket, LoginForm loginForm) {
            this.socket = socket;
            this.loginForm = loginForm;
            InitializeComponent();
            updateChatWorker.RunWorkerAsync();
            isNeedStopWorker = false;
        }

        public void setUsername(string login) {
            usernameLabel.Text = "Hello, " + login;
        }

        private void updateChatWorker_DoWork(object sender, DoWorkEventArgs e) {
            while (isNeedStopWorker == false) {
                socket.Send(jsonWorker.jsonGetMessages());
                Thread.Sleep(5000);
            }
        }

        private void sendMessageButton_Click(object sender, EventArgs e) {
            socket.Send(jsonWorker.jsonSendMessage(messageText.Text, dateTimeWorker.getCurrentUnixTimestamp()));
            messageText.Text = "";
        }

        private void logout_Click(object sender, EventArgs e) {
            loginForm.StartPosition = StartPosition;
            loginForm.Location = Location;
            loginForm.Show();
            Close();
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e) {
            isNeedStopWorker = true;
            socket.Send(jsonWorker.jsonLogout());

            try {
                byte[] serverResponce = new byte[512];
                socket.Receive(serverResponce);
                jsonWorker.serverResponceParse(serverResponce);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }

            loginForm.StartPosition = StartPosition;
            loginForm.Location = Location;
            loginForm.Show();
        }      
    }
}
