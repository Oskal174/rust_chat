using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace chat_client {
    public partial class ChatForm : Form {
        private Socket socket;
        private LoginForm loginForm;

        public ChatForm(Socket socket, LoginForm loginForm) {
            this.socket = socket;
            this.loginForm = loginForm;
            InitializeComponent();
        }

        private void logout_Click(object sender, EventArgs e) {
            loginForm.Show();
            Hide();
        }
    }
}
