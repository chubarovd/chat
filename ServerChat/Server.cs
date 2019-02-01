using System;
using System.Collections.Generic;
using System.Threading;

using System.Net;
using System.Net.Sockets;

using ProtocolChat;

namespace Server {
    class Program {
        static void Main (string[] args) {
            (new Server ()).Awake ();
        }
    }

    class User {
        Socket socket;
        Server server;

        string privateName;
        public string name {
            get {
                return privateName;
            }
            set {
                if (privateName == null) privateName = value;
            }
        }

        Thread ReceiveThread;

        public User (Socket socket, Server server) {
            privateName = null;
            this.socket = socket;
            this.server = server;
        }

        public void Awake () {
            ReceiveThread = new Thread (Receive);
            ReceiveThread.Start ();
        }

        public void Send (object package) {
            socket.Send ((package as Package).ConvertToBytes ());
        }

        void Receive () {
            while (true) {
                try {
                    byte[] buf = new byte[4096];
                    int lenght = socket.Receive (buf);
                    if (lenght > 0) {
                        byte[] exactBuf = new byte[lenght];
                        Buffer.BlockCopy (buf, 0, exactBuf, 0, lenght);
                        ParseMessage (exactBuf);
                    }
                } catch (SocketException) {
                    foreach (User user in server) {
                        if (user.name != privateName)
                            user.Send (new Package ("disconnect", privateName, Protocol.PackageType.CONNECT));
                    }

                    Console.WriteLine (privateName + " broke connection");
                    server.Remove (this);
                    this.ReceiveThread.Abort ();
                    break;
                }
            }
        }

        void ParseMessage (byte[] bytes) {
            Package package = Package.Build (bytes);
            string str1, str2;
            package.Extract (out str1, out str2);

            switch (package.type) {
                case Protocol.PackageType.LOGIN:
                    Console.WriteLine ("login from " + str1);
                    if (privateName == null) {
                        bool allowLogin = true;
                        foreach (User user in server) {
                            if (str1 == user.name) {
                                Send (new Package ("No", privateName, Protocol.PackageType.LOGIN));
                                allowLogin = false;
                                privateName = null;
                                break;
                            }
                        }
                        if (allowLogin) {
                            privateName = str1;
                            Send (new Package ("Yes", privateName, Protocol.PackageType.LOGIN));

                            foreach (User user in server) {
                                user.Send (new Package ("connect", privateName, Protocol.PackageType.CONNECT));
                            }
                        }
                    } else throw new Exception ("Try to reloginning\n");
                    break;

                case Protocol.PackageType.MESSAGE:
                    foreach (User user in server) {
                        if (user.name == str1) {
                            user.Send (new Package (privateName, str2));
                        }
                    }
                    break;

                default:
                    throw new Exception ("Receive package with incorrect type\n");
            }
        }
    }

    class Server : List<User> {
        Thread WaitForConnectionsThread;
        Socket socket 
            = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void Awake () {
            Console.WriteLine ("Preparing Server...");
            try { 
                socket.Bind (Protocol.endPoint);
                socket.Listen (20);

                WaitForConnectionsThread = new Thread (WaitForConnections);
                WaitForConnectionsThread.Start ();

                Console.WriteLine ("Server started successfully");
            } catch (SocketException) {
                Console.WriteLine ("EndPoint error! Try to enter another EndPoint");
                Console.Write ("Enter ip:> ");
                string newIPString = Console.ReadLine ();
                Console.Write ("Enter port:> ");
                string newPortString = Console.ReadLine ();

                IPAddress ip;
                int port;
                while (!IPAddress.TryParse (newIPString, out ip)) {
                    Console.Write ("Please, enter correct ip:> ");
                    newIPString = Console.ReadLine ();
                }
                while (!int.TryParse (newPortString, out port)) {
                    Console.Write ("Please, enter correct port:> ");
                    newPortString = Console.ReadLine ();
                }

                socket.Bind (new IPEndPoint (ip, port));
                socket.Listen (20);

                WaitForConnectionsThread = new Thread (WaitForConnections);
                WaitForConnectionsThread.Start ();

                Console.WriteLine ("Server started successfully");
            }
        }

        void WaitForConnections () {
            while (true) {
                Socket newConnectionSocket = socket.Accept ();

                User newConnectedUser = new User (newConnectionSocket, this);
                
                foreach (User user in this) {
                    newConnectedUser.Send (new Package ("connect", user.name, Protocol.PackageType.CONNECT));
                }

                newConnectedUser.Awake ();
                Add (newConnectedUser);

                Console.WriteLine ("New connection");
            }
        }
    }
}
