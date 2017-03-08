using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public partial class clietTest
    {
        private static void downloadFile(Socket sender)
        {
            const int BufferSize = 1024;
            // Data buffer for incoming data.
            byte[] bytes = new byte[BufferSize];
            try
            {
                //获取服务器文件数
                int revNum = sender.Receive(bytes);
                //打印文件名
                string files = Encoding.GetEncoding("GBK").GetString(bytes, 0, revNum);
                printFiles(files);

                //选择文件
                Console.Write("\ninput save filename:   ");
                string pathNew;

                //传输文件名，需要验证文件名是否存在，如果不存在重复输入知道文件名存在
                byte[] fileNameChoice;
                while (true)
                {
                    nullLine:

                    pathNew = Console.ReadLine();
                    if (pathNew == "")
                    {
                        Console.Write("blank line，enter again：");
                        goto nullLine;
                    }
                    fileNameChoice = Encoding.GetEncoding("GBK").GetBytes(pathNew);
                    int recvNum = sender.Send(fileNameChoice, fileNameChoice.Length, SocketFlags.None);
                    if (revNum == 0)
                        Console.WriteLine("network error");
                    byte[] fileRevMsg = new byte[BufferSize];
                    revNum = sender.Receive(fileRevMsg);
                    string msg = Encoding.ASCII.GetString(fileRevMsg, 0, revNum);
                    if (msg.IndexOf("<NE>") > -1)
                    {
                        printFiles(files);
                        Console.Write("no such file: " + pathNew + "\nplease enter again: ");
                    }
                    else
                        break;
                }


                //tmp用来接收文件大小——long-int64
                byte[] tmp = new byte[8];


                //获取文件大小
                int bufferRecv = sender.Receive(tmp);
                string recvstring = Encoding.ASCII.GetString(tmp, 0, bufferRecv);
                long filesize = long.Parse(recvstring);

                //接收文件路径
                Console.Write("\ninput save filename:   ");
                pathNew = Console.ReadLine();
                //接收文件版本二



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
                            int n = sender.Receive(bytes, sendValue, SocketFlags.None);
                            fsNew.Write(bytes, 0, n);
                            //减去接收到的数量
                            numBytesToRead -= n;
                        }
                    }
                }
                catch (FileNotFoundException ioEx)
                {
                    Console.WriteLine(ioEx.Message);
                }

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
    }
}
