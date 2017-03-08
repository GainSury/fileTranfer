using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public partial class serverTest
    {
        static void downloadServer(Socket handler)
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[BufferSize];
            #region send files
            string files = getCurrentFiles();
            bytes = Encoding.GetEncoding("GBK").GetBytes(files);
            handler.Send(bytes);
            bytes = new Byte[BufferSize];
            #endregion


            #region transmit fileName
            string path;
            while (true)
            {
                //Console.Write("\ninput filename:   ");
                int fileREv = handler.Receive(bytes);

                path = Encoding.GetEncoding("GBK").GetString(bytes, 0, fileREv);

                if (File.Exists(path))
                {
                    string msg = "<SU>";
                    byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
                    handler.Send(msgBytes);
                    break;
                }
                else
                {
                    string msg = "<NE>";
                    byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
                    handler.Send(msgBytes);
                }

            }
            #endregion

            #region getFileSize
            //获取文件大小
            FileInfo fileInfo = new FileInfo(path);
            string request = fileInfo.Length.ToString();
            byte[] bytesNum = Encoding.ASCII.GetBytes(request);

            //发送文件大小
            handler.Send(bytesNum);
            Array.Clear(bytesNum, 0, bytesNum.Length);
            #endregion

            #region sendFile
            //循环发送文件本体
            try
            {
                using (FileStream fsSource = new FileStream(path,
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
    }
}
