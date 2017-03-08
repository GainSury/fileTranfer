using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace server
{
    public partial class serverTest
    {
        const int BufferSize = 1024;
        static string downloadFlag = "<download>";
        static string uploadFlag = "<upload>";
        static string endFlag = "<EOF>";
        // Incoming data from the client.
        public static string data = null;

        public static void StartListening()
        {

            #region before_create_socket
            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            #endregion

            listener.Bind(localEndPoint);
            listener.Listen(10);

            try
            {
                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    Console.WriteLine("Socket connected to {0}",
                handler.RemoteEndPoint.ToString());

                    try
                    {
                        const string success = "<done>";
                        const string fail = "<fail>";
                        //用户密码验证
                        if (AccountValid(handler) == true)
                        {
                            byte[] bytes = Encoding.ASCII.GetBytes(success);
                            handler.Send(bytes);
                        }
                        else
                        {
                            //失败发送fail，并关闭这个客户端的连接。
                            byte[] bytes = Encoding.ASCII.GetBytes(fail);
                            handler.Send(bytes);
                            goto closeSocketFlag;
                            
                        }

                        //选择菜单
                        while (true)
                        {
                            string choice = getChoice(handler);
                            if (choice.IndexOf(downloadFlag) > -1)
                                downloadServer(handler);
                            else if (choice.IndexOf(uploadFlag) > -1)
                                uploadServer(handler);
                            else if (choice.IndexOf(endFlag) > -1)
                                break;
                        }
                    }
                    catch (SocketException e)
                    {
                        //等待写入日志
                        Console.WriteLine("a host close the connection");

                    }
                    closeSocketFlag:  closeSocket(handler);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
        public static int Main(String[] args)
        {
            StartListening();
            return 0;
        }
    }
}