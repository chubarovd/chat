using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGUI {
    static class Program {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main () {
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            Form mainForm = new ServerChat.ClientGUI ();
            mainForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            Application.Run (mainForm);
        }
    }
}
