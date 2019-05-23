using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Collections.Generic;


namespace chat_client {
    public partial class ChatForm : Form {
        private List<UserMessage> chatMessages;
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
                socket.Send(jsonWorker.jsonGetMessages(10));

                // todo: update chat form with received messages
                List<UserMessage> roomMessages = new List<UserMessage>();
                try {
                    byte[] serverResponce = new byte[512];
                    socket.Receive(serverResponce);
                    roomMessages = jsonWorker.getMessagesResponceParse(serverResponce);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.ToString());
                }

                if (validateMessages(roomMessages)) {
                    chatMessages = roomMessages;
                    redisplayMessages(chatMessages);
                }

                Thread.Sleep(5000);
            }
        }

        private bool validateMessages(List<UserMessage> messages) {
            // Сравнивать с текущими месагами и понимать нужно ли добавлять новые к существующим в chatMessages
            return true;
        }

        private void redisplayMessages(List<UserMessage> messages) {
            chatText.Text = "";
            foreach (UserMessage msg in messages) {
                showMessage(msg);
            }
        }

        private void showMessage(UserMessage msg) {
            chatText.Text += msg.author + ":";
            chatText.Text += Environment.NewLine;
            chatText.Text += msg.text;
            chatText.Text += Environment.NewLine;
        }

        private void sendMessageButton_Click(object sender, EventArgs e) {
            sendMessage();
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

        private void messageText_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Return:
                    sendMessage();
                    break;
            }
        }

        private void sendMessage() {
            if (messageText.Text == "") {
                return;
            } 

            socket.Send(jsonWorker.jsonSendMessage(messageText.Text, dateTimeWorker.getCurrentUnixTimestamp()));
            messageText.Text = "";
        }
    }
}
