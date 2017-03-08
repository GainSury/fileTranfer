using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUITest1
{
    public partial class signInForm1 : Form
    {
        const int BufferSize = 1024;
        static bool isSend = false;//关闭窗口之前是否发送消息
        public signInForm1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (signIn(textBox1.Text, textBox2.Text))
            {
                //MainForm mf = new MainForm();
                //mf.Show();
                Program.isloginOK = true;
                Program.userName = textBox1.Text;
                this.Close();
            }
            else
                this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private bool signIn(string username, string password)
        {
            Socket sender = Program.sender;
            const string success = "<done>";
            const string fail = "<fail>";
            //userAndPassword 是 username + "," + password 字符串形式，也就是用户名和密码中不能含有逗号
            //登录字符串可以含有中文字符。
            string userAndPassword = username + "," + password;
            byte[] userBytes = Encoding.UTF8.GetBytes(userAndPassword);
            sender.Send(userBytes);
            isSend = true;

            //接收结果
            byte[] resultBytes = new byte[BufferSize];
            int resultRevNum = sender.Receive(resultBytes);
            string resultString = Encoding.ASCII.GetString(resultBytes, 0, resultRevNum);
            if (resultString.IndexOf(success) > -1)
                return true;
            else
                return false;
        }

        private void signInForm1_Load(object sender, EventArgs e)
        {

        }

        private void signInForm1_FormClosing(object sender, FormClosingEventArgs e)
        {

            const string username = "<done>";
            const string password = "<fail>";
            Socket sender1 = Program.sender;
            string userAndPassword = username + "," + password;
            byte[] userBytes = Encoding.UTF8.GetBytes(userAndPassword);
            if(isSend == false)
                sender1.Send(userBytes);
        }
    }
}
