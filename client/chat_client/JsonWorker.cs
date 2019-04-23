using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace chat_client {
    class JsonWorker {
        private static string handshake_secret = "VXp8v8rF7YefA1hqOX51Wl7g";

        [DataContract]
        class Message {
            public string ToJson() {
                // Make a stream to serialize into.
                using (MemoryStream stream = new MemoryStream()) {
                    // Serialize into the stream.
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(GetType());
                    serializer.WriteObject(stream, this);
                    stream.Flush();

                    // Get the result as text.
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
            public string action { get; set; }
            [DataMember]
            public int code { get; set; }
            [DataMember]
            public string message { get; set; }
        }

        [DataContract]
        class HandshakeMessage : Message {
            [DataMember]
            private string action { get; set; }
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
            private string action { get; set; }
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
            private string action { get; set; }
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

        public JsonWorker() {}

        public byte[] jsonHandshake() {
            HandshakeMessage hm = new HandshakeMessage(handshake_secret);
            return Encoding.ASCII.GetBytes(hm.ToJson());
        }

        public byte[] jsonAuthentication(string login, string password) {
            AuthenticationMessage am = new AuthenticationMessage(login, сreateMD5(password));
            return Encoding.ASCII.GetBytes(am.ToJson());
        }

        public byte[] jsonRegistration(string login, string password) {
            RegistrationMessage rm = new RegistrationMessage(login, сreateMD5(password));
            return Encoding.ASCII.GetBytes(rm.ToJson());
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


        private string сreateMD5(string input) {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
