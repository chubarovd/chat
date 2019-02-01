using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolChat;

namespace Events {
    public class LoginEventArgs : EventArgs {
        bool privateIsLoginAllow;

        public LoginEventArgs (bool isLoginAllow) {
            privateIsLoginAllow = isLoginAllow;
        }

        public bool isLoginAllow {
            get {
                return privateIsLoginAllow;
            }
        }
    }

    public class ConnectEventArgs : EventArgs {
        bool privateIsConnect;
        string privateName;

        public ConnectEventArgs (bool isConnect, string name) {
            privateIsConnect = isConnect;
            privateName = name;
        }

        public bool isConnect {
            get { return privateIsConnect; }
        }

        public string name {
            get { return privateName; }
        }
    }

    public class MessageEventArgs : EventArgs {
        string privateName;
        string privateText;

        public MessageEventArgs (Package package) {
            package.Extract (out privateName, out privateText);
        }
        public string name {
            get {
                return privateName;
            }
        }
        public string text {
            get {
                return privateText;
            }
        }
    }
}
