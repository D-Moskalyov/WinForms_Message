using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    //public delegate void newUserOk();
    
    public partial class Form2 : Form
    {
        //public event newUserOk userOkEvent;
        public event EventHandler userOkEvent;
        bool newUser = false;

        void raiseNewUserOk()
        {
            if (userOkEvent != null)
            {
                userOkEvent(this, EventArgs.Empty);
            }
        }

        public string text = string.Empty;
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((Form1)(this.Owner)).newName = "";
            ((Form2)((Button)sender).Parent).Close();
            //this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                newUser = true;
                text = textBox1.Text;
                //Form1 for1 = Form1.ActiveForm;
                raiseNewUserOk();
                //((Form1)(this.Owner)).newName = textBox1.Text;
                //this.Close();
                ((Form2)((Button)sender).Parent).Close();
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!newUser)
                ((Form1)(this.Owner)).newName = "";
        }
    }
}
