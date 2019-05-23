using System.Runtime.Serialization;

namespace chat_client {
    [DataContract]
    public class UserMessage {
        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string text { get; set; }
        [DataMember]
        public int timestamp { get; set; }

        public UserMessage() { }
    }
}
