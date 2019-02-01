namespace ServerChat {
    partial class ClientGUI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.userList = new System.Windows.Forms.ListView();
            this.login = new System.Windows.Forms.Button();
            this.send = new System.Windows.Forms.Button();
            this.logsTab = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabView = new System.Windows.Forms.TabControl();
            this.input = new System.Windows.Forms.TextBox();
            this.logsTab.SuspendLayout();
            this.tabView.SuspendLayout();
            this.SuspendLayout();
            // 
            // userList
            // 
            this.userList.Location = new System.Drawing.Point(626, 35);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(165, 347);
            this.userList.TabIndex = 1;
            this.userList.UseCompatibleStateImageBehavior = false;
            this.userList.View = System.Windows.Forms.View.List;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(625, 388);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(165, 44);
            this.login.TabIndex = 2;
            this.login.Text = "Login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(626, 438);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(165, 41);
            this.send.TabIndex = 3;
            this.send.Text = "Send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // logsTab
            // 
            this.logsTab.Controls.Add(this.listView1);
            this.logsTab.Location = new System.Drawing.Point(4, 22);
            this.logsTab.Name = "logsTab";
            this.logsTab.Padding = new System.Windows.Forms.Padding(3);
            this.logsTab.Size = new System.Drawing.Size(598, 397);
            this.logsTab.TabIndex = 0;
            this.logsTab.Text = "logs";
            this.logsTab.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(18, 18);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(563, 362);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // tabView
            // 
            this.tabView.Controls.Add(this.logsTab);
            this.tabView.Location = new System.Drawing.Point(13, 13);
            this.tabView.Name = "tabView";
            this.tabView.SelectedIndex = 0;
            this.tabView.Size = new System.Drawing.Size(606, 423);
            this.tabView.TabIndex = 0;
            this.tabView.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabView_Selected);
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(13, 442);
            this.input.Multiline = true;
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(602, 37);
            this.input.TabIndex = 4;
            this.input.Text = "Enter login...";
            this.input.Click += new System.EventHandler(this.input_Click);
            // 
            // ClientGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 491);
            this.Controls.Add(this.input);
            this.Controls.Add(this.send);
            this.Controls.Add(this.login);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.tabView);
            this.Name = "ClientGUI";
            this.Text = "Form1";
            this.logsTab.ResumeLayout(false);
            this.tabView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView userList;
        private System.Windows.Forms.Button login;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.TabPage logsTab;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabControl tabView;
        private System.Windows.Forms.TextBox input;
    }
}