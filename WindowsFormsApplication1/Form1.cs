using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        List<Contact> Contacts = new List<Contact>();
        List<Dialog> Dialogs = new List<Dialog>();
        List<Label> Labels = new List<Label>();
        public string newName;
        int countOfLabels = 0;
        StreamReader sr;
        StreamWriter sw;
        bool brokenFile = false;

        public Form1()
        {
            //Contacts.Add(new Contact("John"));
            //Contacts.Add(new Contact("Kate"));
            //Contacts.Add(new Contact("Mad"));
            //Contacts.Add(new Contact("Mike"));

            if (File.Exists("data.txt"))
            {
                sr = new StreamReader("data.txt");
                string smpl = sr.ReadLine();

                while (smpl != null && !brokenFile)
                {
                    while (smpl != "" && !brokenFile)
                    {
                        Contacts.Add(new Contact(smpl));
                        Dialogs.Add(new Dialog(smpl));
                        smpl = sr.ReadLine();
                        while (smpl != "" && !brokenFile)
                        {
                            //MessageBox.Show(smpl);
                            if (smpl == "True" || smpl == "False")
                            {
                                if (smpl == "True")
                                {
                                    Dialogs.Last().msgs.Add(new Message(true));
                                }
                                else
                                    Dialogs.Last().msgs.Add(new Message(false));
                            }
                            else
                            {
                                brokenFile = true;
                            }
                            if (!brokenFile)
                            {
                                smpl = sr.ReadLine();
                                if (smpl != "" && smpl != null)
                                {
                                    Dialogs.Last().msgs.Last().dt = StringToDateTime(smpl);
                                    smpl = sr.ReadLine();
                                    if (smpl != "" && smpl != null)
                                    {
                                        Dialogs.Last().msgs.Last().msg = smpl;
                                        smpl = sr.ReadLine();
                                    }
                                    else
                                        brokenFile = true;
                                }
                                else
                                    brokenFile = true;
                            }
                        }
                    }
                    smpl = sr.ReadLine();
                }
                sr.Close();
            }

            else
            {
                MessageBox.Show("Файла не существует");
                Contacts.Add(new Contact("John"));
                Contacts.Add(new Contact("Kate"));
                Contacts.Add(new Contact("Mad"));
                Contacts.Add(new Contact("Mike"));
                foreach (Contact cont in Contacts)
                {
                    Dialogs.Add(new Dialog(cont.name));
                }
            }

            if (brokenFile)
            {
                MessageBox.Show("Файла повреждён");
                Contacts.Clear();
                Dialogs.Clear();

                Contacts.Add(new Contact("John"));
                Contacts.Add(new Contact("Kate"));
                Contacts.Add(new Contact("Mad"));
                Contacts.Add(new Contact("Mike"));
                foreach (Contact cont in Contacts)
                {
                    Dialogs.Add(new Dialog(cont.name));
                }
            }

            InitializeComponent();

            foreach (Contact cont in Contacts)
            {
                this.listBox1.Items.Add(cont.name);
                //Dialogs.Add(new Dialog(cont.name));
            }

            /*             foreach (string str in listBox1.Items)
                        {
                            Dialogs.Add(new Dialog(str));
                        }  */
        }

        private DateTime StringToDateTime(string str)
        {
            int year, month, day, hour, minute, second;
            char[] dayCh = str.ToCharArray(0, 2);
            char[] monthCh = str.ToCharArray(3, 2);
            char[] yearCh = str.ToCharArray(6, 4);
            char[] hourCh = str.ToCharArray(11, 2);
            char[] minuteCh = str.ToCharArray(14, 2);
            char[] secondCh = str.ToCharArray(17, 2);
            year = int.Parse(new string(yearCh));
            month = int.Parse(new string(monthCh));
            day = int.Parse(new string(dayCh));
            hour = int.Parse(new string(hourCh));
            minute = int.Parse(new string(minuteCh));
            second = int.Parse(new string(secondCh));
            return new DateTime(year, month, day, hour, minute, second);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                Labels.Clear();
                Control.ControlCollection cC = panel1.Controls;
                while (cC.Count != 0)
                {
                    cC.RemoveAt(0);
                }

                string name = ((ListBox)sender).SelectedItem.ToString();
                label1.Text = name;
                int index;

                for (index = 0; index < Dialogs.Count; index++)
                {
                    if (name == Dialogs[index].companionName)
                        break;
                }

                for (countOfLabels = 0; countOfLabels < Dialogs[index].msgs.Count; countOfLabels++)
                {
                    Labels.Add(new Label());

                    Labels[countOfLabels].Text = string.Format("{0} - {1:H:mm:ss}", Dialogs[index].msgs[countOfLabels].msg, Dialogs[index].msgs[countOfLabels].dt);
                    Labels[countOfLabels].AutoSize = true;
                    Labels[countOfLabels].BorderStyle = BorderStyle.FixedSingle;
                    Labels[countOfLabels].MaximumSize = new System.Drawing.Size(150, 0);
                    if (countOfLabels != 0)
                    {
                        if (Dialogs[index].msgs[countOfLabels].isAdmMsg)
                            Labels[countOfLabels].Location = new System.Drawing.Point(173, Labels[countOfLabels - 1].Bottom + 5);
                        else
                            Labels[countOfLabels].Location = new System.Drawing.Point(3, Labels[countOfLabels - 1].Bottom + 5);
                    }
                    else
                    {
                        if (Dialogs[index].msgs[countOfLabels].isAdmMsg)
                            Labels[countOfLabels].Location = new System.Drawing.Point(173, 3);
                        else
                            Labels[countOfLabels].Location = new System.Drawing.Point(3, 3);
                    }

                    panel1.Controls.Add(Labels[countOfLabels]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                Control.ControlCollection cC = panel1.Controls;
                while (cC.Count != 0)
                {
                    cC.RemoveAt(0);
                }
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                if (MessageBox.Show(this, "Вы уверены?", "Удаление контакта", buttons) == DialogResult.Yes)
                {
                    string name = listBox1.SelectedItem.ToString();
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);//удаление из ListBox
                    for (int i = 0; i < Dialogs.Count; i++)//удаление диалога
                    {
                        if (Dialogs[i].companionName == name)
                        {
                            Dialogs.RemoveAt(i);
                            break;
                        }
                    }
                    for (int i = 0; i < Contacts.Count; i++)//удаление контакта
                    {
                        if (Contacts[i].name == name)
                        {
                            Contacts.RemoveAt(i);
                            break;
                        }
                    }
                    Labels.Clear();
                }
                countOfLabels = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.userOkEvent += new EventHandler(newUserFromForm2);
            f2.ShowDialog(this);
            if (newName != "")
            {
                bool exclus = true;
                foreach (Contact cont in Contacts)
                {
                    if (cont.name == newName)
                    {
                        exclus = false;
                    }
                }
                if (exclus)
                {
                    //MessageBox.Show("dd");
                    listBox1.Items.Add(newName);
                    Contacts.Add(new Contact(newName));
                    Dialogs.Add(new Dialog(newName));
                }
                else
                { //MessageBox.Show("bb"); };
            }
            else
            { //MessageBox.Show("zz"); };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && listBox1.SelectedIndex >= 0)
            {
                string nm = listBox1.SelectedItem.ToString();
                int numOfDialog;

                foreach (Contact cont in Contacts)//контакту поставить флажок активный true
                {
                    if (nm == cont.name)
                    {
                        cont.isActive = true;
                        break;
                    }
                }

                for (numOfDialog = 0; numOfDialog < Dialogs.Count; numOfDialog++)
                {
                    if (nm == Dialogs[numOfDialog].companionName)
                    {
                        Dialogs[numOfDialog].msgs.Add(new Message(true, textBox1.Text));
                        Dialogs[numOfDialog].msgs.Last().dt = DateTime.Now;
                        break;
                    }
                }

                Labels.Add(new Label());

                Labels[countOfLabels].Text = string.Format("{0} - {1:H:mm:ss}", textBox1.Text, Dialogs[numOfDialog].msgs.Last().dt);
                Labels[countOfLabels].AutoSize = true;
                Labels[countOfLabels].BorderStyle = BorderStyle.FixedSingle;
                Labels[countOfLabels].MaximumSize = new System.Drawing.Size(150, 0);
                if (countOfLabels != 0)
                {
                    if (Dialogs[numOfDialog].msgs.Last().isAdmMsg)
                        Labels[countOfLabels].Location = new System.Drawing.Point(173, Labels[countOfLabels - 1].Bottom + 5);
                    else
                        Labels[countOfLabels].Location = new System.Drawing.Point(3, Labels[countOfLabels - 1].Bottom + 5);
                }
                else
                {
                    if (Dialogs[numOfDialog].msgs.Last().isAdmMsg)
                        Labels[countOfLabels].Location = new System.Drawing.Point(173, 3);
                    else
                        Labels[countOfLabels].Location = new System.Drawing.Point(3, 3);
                }


                panel1.Controls.Add(Labels[countOfLabels]);

                countOfLabels++;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (Contact cont in Contacts)
            {
                listBox1.Items.Add(cont.name);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < Contacts.Count; i++)
            {
                if (Contacts[i].isActive)
                {
                    listBox1.Items.Add(Contacts[i].name);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sw = new StreamWriter("data.txt", false);

            foreach (Dialog dlg in Dialogs)
            {
                sw.WriteLine(dlg.companionName);
                foreach (Message msg in dlg.msgs)
                {
                    sw.WriteLine(msg.isAdmMsg.ToString());
                    sw.WriteLine(msg.dt);
                    sw.WriteLine(msg.msg);
                }
                sw.WriteLine();
            }

            /* for(int i = 0; i < Dialogs.Count - 1; i++)
            {
                sw.WriteLine(Dialogs[i].companionName);
                foreach(Message msg in Dialogs[i].msgs)
                {
                    sw.WriteLine(msg.isAdmMsg.ToString());//!?!?!?!?!?!?!?!?!?!?!?
                    sw.WriteLine(msg.dt);
                    sw.WriteLine(msg.msg);
                }
            }
            sw.WriteLine(Dialogs.Last().companionName);
            for(int i = 0; i < Dialogs.Last().msgs.Count - 1; i++)
            {
                sw.WriteLine(Dialogs.Last().msgs[i].isAdmMsg.ToString());//!?!?!?!?!?!?!?!?!?!?!?
                sw.WriteLine(Dialogs.Last().msgs[i].dt);
                sw.WriteLine(Dialogs.Last().msgs[i].msg);
            }
            sw.Write(Dialogs.Last().msgs.Last().isAdmMsg.ToString());
            sw.Write(Dialogs.Last().msgs.Last().dt);
            sw.Write(Dialogs.Last().msgs.Last().msg); */

            sw.Close();
        }
        void newUserFromForm2(object sender, EventArgs args)
        {
            //MessageBox.Show(((Form2)sender).text);
            newName = string.Copy(((Form2)sender).text);
        }
    }
}

//добавление
//ListBox с ValueMember