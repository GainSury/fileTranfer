using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUITest1
{
    public partial class downloadForm1 : Form
    {
        const int BufferSize = 1024;
        byte[] bytes = new byte[BufferSize];
        public downloadForm1()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket handler = Program.sender;
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选中一个值");
                return;
            }

            //选择文件名
            string pathNew = listView1.SelectedItems[0].Text;

            //传输文件名，需要验证文件名是否存在，如果不存在重复输入知道文件名存在
            byte[] fileNameChoice;
            while (true)
            {
                
                fileNameChoice = Encoding.GetEncoding("GBK").GetBytes(pathNew);
                int recvNum = handler.Send(fileNameChoice, fileNameChoice.Length, SocketFlags.None);
                byte[] fileRevMsg = new byte[BufferSize];

                //接受文件名字符串大小
                int revNum = handler.Receive(fileRevMsg);
                string msg = Encoding.ASCII.GetString(fileRevMsg, 0, revNum);
                if (msg.IndexOf("<NE>") > -1)
                { }
                else
                    break;
            }

            //tmp用来接收文件大小
            byte[] tmp = new byte[8];

            //获取文件大小
            int bufferRecv = handler.Receive(tmp);
            string recvstring = Encoding.ASCII.GetString(tmp, 0, bufferRecv);
            long filesize = long.Parse(recvstring);

            
            //接收文件
            try
            {
                using (FileStream fsNew = new FileStream(pathNew, FileMode.Create, FileAccess.Write))
                {

                    // Read the source file into a byte array.

                    long numBytesToRead = filesize;
                    long compValue = 1;
                    long bufferSize64 = Convert.ToInt64(BufferSize);
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        //need to convert to int32
                        int sendValue = Convert.ToInt32(numBytesToRead / bufferSize64 >= compValue ? bufferSize64 : numBytesToRead % bufferSize64);
                        int n = handler.Receive(bytes, sendValue, SocketFlags.None);
                        fsNew.Write(bytes, 0, n);
                        //减去接收到的数量
                        numBytesToRead -= n;
                    }
                }
            }
            catch (FileNotFoundException ioEx)
            {
                
            }
            this.Close();
        }

        private void downloadForm1_Load(object sender, EventArgs e)
        {
            listView1.MultiSelect = false;
            Socket handler = Program.sender;
            byte[] bytes = new byte[BufferSize];
            
            //获取服务器文件数
            int revNum = handler.Receive(bytes);
            //打印文件名
            string files = Encoding.GetEncoding("GBK").GetString(bytes, 0, revNum);
            string[] filess = files.Split(",".ToCharArray());
            foreach (string file in filess)
                listView1.Items.Add(file);
        }
    }
}
