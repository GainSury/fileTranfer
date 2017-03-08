using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUITest1
{
    public partial class MainForm1 : Form
    {
        public MainForm1()
        {
            InitializeComponent();
        }

        private void MainForm1_Load(object sender, EventArgs e)
        {
            label1.Text = "欢迎回来," + Program.userName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] choiceSendMsg = Encoding.ASCII.GetBytes(Program.uploadFlag);
            Program.sender.Send(choiceSendMsg);
          
            Form uploadForm = new uploadForm1();
            uploadForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] choiceSendMsg = Encoding.ASCII.GetBytes(Program.downloadFlag);
            Program.sender.Send(choiceSendMsg);

            Form downloadForm = new downloadForm1();
            downloadForm.ShowDialog();
        }
    }
}
