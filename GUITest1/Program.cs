using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUITest1
{
    
    static class Program
    {
        public static bool isloginOK = false;
        public static string userName;
        public static string downloadFlag = "<download>";
        public static string uploadFlag = "<upload>";
        public static string endFlag = "<EOF>";
        public static int BufferSize = 1024;

        public static Socket sender;
        /// <summary>
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]


        static void Main()
        {
            getSocket();
            if (sender.Connected == false)
            {
                MessageBox.Show("网络连接失败");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form login = new signInForm1();
            Form mainform = new MainForm1();

            login.ShowDialog();
            if (isloginOK == true)
                Application.Run(mainform);
            else
            {
                MessageBox.Show("用户名密码不正确,登录失败。");
            }
            byte[] choiceSendMsg = Encoding.ASCII.GetBytes(endFlag);
            sender.Send(choiceSendMsg);
            closeSocket(sender);
        }

        private static void closeSocket(Socket sender)
        {
            byte[] choiceSendMsg = Encoding.ASCII.GetBytes(endFlag);
            sender.Send(choiceSendMsg);
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        public static void getSocket()
        {

            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress;
                ipAddress = ipHostInfo.AddressList[0];

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(remoteEP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
