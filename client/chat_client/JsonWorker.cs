using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace chat_client {
    class JsonWorker {
        private static string handshake_secret = "VXp8v8rF7YefA1hqOX51Wl7g";

        [DataContract]
        class Message {
            [DataMember]
            protected string action { get; set; }

            public Message() { }

            public Message(string a) {
                action = a;
            }

            public string ToJson() {
                using (MemoryStream stream = new MemoryStream()) {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(GetType());
                    serializer.WriteObject(stream, this);
                    stream.Flush();
                    
                    stream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(stream)) {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        [DataContract]
        class ServerResponse : Message {
            [DataMember]
            public int code { get; set; }
            [DataMember]
            public string message { get; set; }
        }

        [DataContract]
        class HandshakeMessage : Message {
            [DataMember]
            private string secret { get; set; }

            public HandshakeMessage(string s) {
                action = "handshake";
                secret = s;
            }      
        }

        [DataContract]
        class AuthenticationMessage : Message {
            [DataMember]
            private string login { get; set; }
            [DataMember]
            private string password_hash { get; set; }

            public AuthenticationMessage(string l, string p) {
                action = "authentication";
                login = l;
                password_hash = p;
            }
        }

        [DataContract]
        class RegistrationMessage : Message {
            [DataMember]
            private string login { get; set; }
            [DataMember]
            private string password_hash { get; set; }

            public RegistrationMessage(string l, string p) {
                action = "registration";
                login = l;
                password_hash = p;
            }
        }

        [DataContract]
        class UpdateMessage : Message {
            [DataMember]
            private string login { get; set; }
            [DataMember]
            private string password_hash { get; set; }
            [DataMember]
            private string new_password_hash { get; set; }

            public UpdateMessage(string l, string p, string np) {
                action = "update_password";
                login = l;
                password_hash = p;
                new_password_hash = np;
            }
        }

        [DataContract]
        class DeleteMessage : Message {
            [DataMember]
            private string login { get; set; }
            [DataMember]
            private string password_hash { get; set; }

            public DeleteMessage(string l, string p) {
                action = "delete_account";
                login = l;
                password_hash = p;
            }
        }

        [DataContract]
        class SendMessage : Message {
            [DataMember]
            private string text { get; set; }
            [DataMember]
            private int timestamp { get; set; }

            public SendMessage(string t, int uts) {
                action = "send_message";
                timestamp = uts;
                text = t;
            }
        }

        public JsonWorker() {}

        public byte[] jsonHandshake() {
            HandshakeMessage hm = new HandshakeMessage(handshake_secret);
            return Encoding.ASCII.GetBytes(hm.ToJson());
        }

        public byte[] jsonAuthentication(string login, string password) {
            AuthenticationMessage am = new AuthenticationMessage(login, createMD5(password));
            return Encoding.ASCII.GetBytes(am.ToJson());
        }

        public byte[] jsonLogout() {
            Message m = new Message("logout");
            return Encoding.ASCII.GetBytes(m.ToJson());
        }

        public byte[] jsonCloseConnection() {
            Message m = new Message("close_connection");
            return Encoding.ASCII.GetBytes(m.ToJson());
        }

        public byte[] jsonRegistration(string login, string password) {
            RegistrationMessage rm = new RegistrationMessage(login, createMD5(password));
            return Encoding.ASCII.GetBytes(rm.ToJson());
        }

        public byte[] jsonUpdate(string login, string password, string new_password) {
            UpdateMessage rm = new UpdateMessage(login, createMD5(password), createMD5(new_password));
            return Encoding.ASCII.GetBytes(rm.ToJson());
        }

        public byte[] jsonDelete(string login, string password) {
            DeleteMessage rm = new DeleteMessage(login, createMD5(password));
            return Encoding.ASCII.GetBytes(rm.ToJson());
        }

        public byte[] jsonSendMessage(string text, int unixTimestamp) {
            SendMessage sm = new SendMessage(text, unixTimestamp);
            return Encoding.ASCII.GetBytes(sm.ToJson());
        }

        public byte[] jsonGetMessages() {
            Message m = new Message("get_messages");
            return Encoding.ASCII.GetBytes(m.ToJson());
        }

        public void serverResponceParse(byte[] jsonResponce) {
            string json = Encoding.ASCII.GetString(jsonResponce).TrimEnd('\0');
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json))) {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(ServerResponse));
                ServerResponse response = (ServerResponse)deserializer.ReadObject(ms);

                if (response.code != 200) {
                    throw new System.Exception(response.message);
                }
            }
        }

        private string createMD5(string input) {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
