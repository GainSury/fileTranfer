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
        static void uploadFile(Socket handler)
        {

            string files = getCurrentFiles();
            printFiles(files);
            string fileName = getFileName();

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
            handler.Send(fileNameBytes,fileNameBytes.Length,SocketFlags.None);
            System.Threading.Thread.Sleep(1000);
            
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
        }


        private static string getFileName()
        {
            string fileName = "";

            Console.Write("input fileName to save:");

            while (true)
            {
                fileName = Console.ReadLine();
                if (fileName == "")
                    Console.Write("null input,input again:");
                else if (File.Exists(fileName) == false)
                    Console.Write("file is not exist,input again:");
                else
                    break;
            }
            return fileName;
        }

    }
}
