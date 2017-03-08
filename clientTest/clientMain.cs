using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace client
{
    public partial class clietTest
    {

        static string downloadFlag = "<download>";
        static string uploadFlag = "<upload>";
        static string endFlag = "<EOF>";
        public static void StartClient(string arg)
        {


            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress;
                //命令行第一个参数如果是IP地址，则用命令行输入的ip地址，否则用本机地址作为ip地址 127.0.0.1
                if (arg == null)
                    ipAddress = ipHostInfo.AddressList[0];
                else
                    ipAddress = IPAddress.Parse(arg);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                sender.Connect(remoteEP);

                //登录
                if (signIn(sender) == false)
                {
                    signInFail(sender);
                    goto closeSocketFlag;
                }


                //菜单选项
                while (true)
                {
                    int choice = showMenu();
                    if (choice == 1)
                    {
                        byte[] choiceSendMsg = Encoding.ASCII.GetBytes(downloadFlag);
                        sender.Send(choiceSendMsg);
                        downloadFile(sender);
                    }
                    else if (choice == 2)
                    {
                        byte[] choiceSendMsg = Encoding.ASCII.GetBytes(uploadFlag);
                        sender.Send(choiceSendMsg);
                        uploadFile(sender);

                    }
                    //退出
                    else if (choice == 3)
                    {
                        byte[] choiceSendMsg = Encoding.ASCII.GetBytes(endFlag);
                        sender.Send(choiceSendMsg);
                        break;
                    }
                }

                closeSocketFlag: closeSocket(sender);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            
        }

        public static int Main(String[] args)
        {
            string arg = null;
            StartClient(arg);
            return 0;
        }
    }
}