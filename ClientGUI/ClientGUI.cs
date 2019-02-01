using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ServerChat;
using ClientChat;
using ProtocolChat;
using Events;

namespace ServerChat {
    public partial class ClientGUI : Form {
        Client client;

        delegate void DelegateLoginCallback (bool isLoginAllow);
        DelegateLoginCallback delegateLoginCallback;

        delegate void DelegateConnectCallback (bool isConnect, string name);
        DelegateConnectCallback delegateConnectCallback;

        delegate void DelegateMessageCallback (Package package);
        DelegateMessageCallback delegateMessageCallback;

        public ClientGUI () {
            InitializeComponent ();

            login.Enabled = true;
            send.Enabled = false;

            client = new Client ();
            client.OnLogin += new DelegateLogin (OnLoginEvent);
            client.OnMessage += new DelegateMessage (OnMessageEvent);
            client.OnConnect += new DelegateConnect (OnConnectionEvent);

            delegateLoginCallback = new DelegateLoginCallback (OnLogin);
            delegateConnectCallback = new DelegateConnectCallback (OnConnect);
            delegateMessageCallback = new DelegateMessageCallback (OnMessage);
            client.Awake ();
        }

        /* ============================================ */
        /* ============================================ */
        /* ============================================ */ 

        void OnLoginEvent (LoginEventArgs args) {
            this.Invoke (delegateLoginCallback, new object[] { args.isLoginAllow });
        }

        void OnMessageEvent (MessageEventArgs args) {
            this.Invoke (delegateMessageCallback, new object[] { new Package (args.name, args.text) });
        }

        void OnConnectionEvent (ConnectEventArgs args) {
            this.Invoke (delegateConnectCallback, new object[] { args.isConnect, args.name });
        }

        /* ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ */
        /* ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ */

        void OnLogin (bool isLoginAllow) {
            if (isLoginAllow) {
                foreach (ListView list in logsTab.Controls) {
                    list.Items.Add ("Корректный логин");
                }
                //MessageBox.Show ("Коректный логин");
                login.Enabled = false;
                send.Enabled = true;
            } else {
                foreach (ListView list in logsTab.Controls) {
                    list.Items.Add ("Такой логин уже занят");
                }
            }
        }

        void OnMessage (Package package) {
            string name, text;
            package.Extract (out name, out text);
            foreach (TabPage tab in tabView.TabPages) {
                if (tab.Text == name) {
                    foreach (ListView list in tab.Controls) {
                        list.Items.Add (name + ":>" + text);
                    }
                }
            }
        }

        void OnConnect (bool isConnect, string name) {
            if (isConnect) {
                userList.Items.Add (name);
                TabPage newUserTab = new TabPage (name);
                ListView newUserTabList = new ListView ();
                newUserTabList.Location = new Point (18, 18);
                newUserTabList.Size = new Size (563, 362);
                newUserTabList.View = View.List;
                newUserTab.Controls.Add (newUserTabList);
                tabView.TabPages.Add (newUserTab);

            } else {
                foreach (ListViewItem item in userList.Items) {
                    if (name == item.Name) {
                        userList.Items.Remove (item);
                    }
                }
                
                foreach (TabPage tab in tabView.TabPages) {
                    if (name == tab.Name) {
                        tabView.TabPages.Remove (tab);
                    }
                }
            }
        }

        /* ============================================ */
        /* ============================================ */
        /* ============================================ */

        private void login_Click (object sender, EventArgs e) {
            if (input.Text != "") {
                client.Send (new Package (input.Text, null, Protocol.PackageType.LOGIN));
                client.name = input.Text;
                input.Text = "Enter message...";
            } else input.Text = "Enter login...";
        }

        private void send_Click (object sender, EventArgs e) {
            if (input.Text != "") {
                client.Send (new Package (tabView.SelectedTab.Text, input.Text));

                foreach (ListView list in tabView.SelectedTab.Controls) {
                    list.Items.Add ("You:>" + input.Text);
                }
            } else input.Text = "Enter message...";
        }

        private void userList_SelectedIndexChanged (object sender, EventArgs e) {
            
        }

        private void input_Click (object sender, EventArgs e) {
            input.Text = "";
        }

        private void tabView_Selected (object sender, TabControlEventArgs e) {

        }
    }
}
