using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

using ProtocolChat;
using Events;

namespace ClientChat {
    delegate void DelegateLogin (LoginEventArgs args);
    delegate void DelegateConnect (ConnectEventArgs args);
    delegate void DelegateMessage (MessageEventArgs args);

    class Client {
        Socket socket;
        string privateName;
        public string name {
            get {
                return privateName;
            }
            set {
                privateName = value;
            }
        }
        Thread ReceiveThread;

        public event DelegateLogin OnLogin;
        public event DelegateConnect OnConnect;
        public event DelegateMessage OnMessage;

        public Client () {
            privateName = null;
        }

        public void Awake () {
            try {
                socket 
                    = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect (Protocol.endPoint);

                ReceiveThread = new Thread (Receive);
                ReceiveThread.Start ();

                Console.WriteLine ("Server started successfully");
            } catch (SocketException) {

            }
        }

        public void Send (object package) {
            socket.Send ((package as Package).ConvertToBytes ());
        }

        void Receive () {
            while (true) {
                byte[] buf = new byte[4096];
                int lenght = socket.Receive (buf);
                if (lenght > 0) {
                    byte[] exactBuf = new byte[lenght];
                    Buffer.BlockCopy (buf, 0, exactBuf, 0, lenght);
                    ParseMessage (exactBuf);
                }
            }
        }

        void ParseMessage (byte[] bytes) {
            Package package = Package.Build (bytes);
            string str1, str2;
            package.Extract (out str1, out str2);

            switch (package.type) {
                case Protocol.PackageType.LOGIN:
                    LoginCallback ((str1 == "Yes"));
                    break;

                case Protocol.PackageType.MESSAGE:
                    MessageCallBack (package);
                    break;

                case Protocol.PackageType.CONNECT:
                    ConnectCallback ((str1 == "connect"), str2);
                    break;

                default:
                    throw new Exception ("Receive package with incorrect type\n");
            }
        }

        void LoginCallback (bool isLoginAllow) {
            if (OnLogin != null) OnLogin (new LoginEventArgs (isLoginAllow));
        }
        
        void MessageCallBack (Package package) {
            if (OnMessage != null) OnMessage (new MessageEventArgs (package));
        }

        void ConnectCallback (bool isConnect, string name) {
            if (OnConnect != null) OnConnect (new ConnectEventArgs (isConnect, name));
        }
    }
}
