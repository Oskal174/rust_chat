using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_client {
    class DateTimeWorker {
        public DateTimeWorker() { }

        public int getCurrentUnixTimestamp() {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public string getCurrentDateTime(int unixTimestamp) {
            DateTime pDate = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(unixTimestamp);
            return pDate.ToString("dd.MM.yyyy | HH:mm:ss");
        }
    }
}
