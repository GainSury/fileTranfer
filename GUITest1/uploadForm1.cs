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
    public partial class uploadForm1 : Form
    {
       const int BufferSize = 1024;
        public uploadForm1()
        {
            InitializeComponent();
        }

        private void uploadForm1_Load(object sender, EventArgs e)
        {
            listView1.MultiSelect = false;
            string[] files = getCurrentFiles();
            foreach (string file in files)
                listView1.Items.Add(file);
            
        }

        private static string[] getCurrentFiles()
        {
            DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            string[] tmp = new string[di.GetFiles().Length];
            int i = 0;
            foreach (var fi in di.GetFiles())
            {
                tmp[i++] = fi.Name;
            }
            return tmp;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选中一个值");
                return;
            }
            String fileName = listView1.SelectedItems[0].Text;

            Socket handler = Program.sender;

            //获取文件大小
            FileInfo fileInfo = new FileInfo(fileName);
            string request = fileInfo.Length.ToString();
            byte[] bytesNum = Encoding.ASCII.GetBytes(request);
            //发送文件大小
            handler.Send(bytesNum, bytesNum.Length, SocketFlags.None);
            Array.Clear(bytesNum, 0, bytesNum.Length);

            System.Threading.Thread.Sleep(1000);


            //发送文件名
            byte[] fileNameBytes = Encoding.GetEncoding("GBK").GetBytes(fileName);
            handler.Send(fileNameBytes, fileNameBytes.Length, SocketFlags.None);

            System.Threading.Thread.Sleep(3000);

            #region sendFile
            //循环发送文件本体
            byte[] bytes = new byte[BufferSize];
            try
            {
                using (FileStream fsSource = new FileStream(fileName,
                    FileMode.Open, FileAccess.Read))
                {

                    // Read the source file into a byte array.

                    long numBytesToRead = fsSource.Length;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(bytes, 0, bytes.Length);
                        //fsNew.Write(bytes, 0, n);
                        handler.Send(bytes, n, SocketFlags.None);
                        Array.Clear(bytes, 0, bytes.Length);
                        numBytesToRead -= n;
                    }
                }
            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            #endregion

            this.Close();
        }
    }
}
