using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Message
    {
        public Message(bool admOrNot)
        {
            isAdmMsg = admOrNot;
        }
        public Message(bool admOrNot, string txt)
        {
            isAdmMsg = admOrNot;
            dt = DateTime.Now;//сейчас
            msg = txt;
        }
        public bool isAdmMsg;
        public DateTime dt;
        public string msg;
    }
    class Dialog
    {
        //public static int number;
        public Dialog(string compName)
        {
            msgs = new List<Message>();
            companionName = compName;
        }
        public List<Message> msgs;
        public string companionName;
    }
    class Contact
    {
        public Contact(string nm)
        {
            isActive = false;
            name = nm;
        }
        public bool isActive;
        public string name;
    }
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
