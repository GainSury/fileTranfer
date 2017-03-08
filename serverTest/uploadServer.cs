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
        
        private static void uploadServer(Socket sender)
        {
            
            try
            {
                int bufferRecv;
                
                //tmp用来接收文件大小——long-int64
                byte[] tmp = new byte[BufferSize];
                //获取文件大小
                bufferRecv = sender.Receive(tmp);
                string recvstring = Encoding.ASCII.GetString(tmp, 0, bufferRecv);
                long filesize = long.Parse(recvstring);


                //传送文件名
                byte[] fileNameBytes = new byte[BufferSize];
                bufferRecv = sender.Receive(fileNameBytes);
                string fileNameRev = Encoding.GetEncoding("GBK").GetString(fileNameBytes, 0, bufferRecv);

                //设置文件保存路径
                //string fileName = saveFileName(fileNameRev);
                string fileName = fileNameRev;
                //接收文件版本二
                try
                {

                    using (FileStream fsNew = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    {

                        // Read the source file into a byte array.
                        byte[] bytes = new byte[BufferSize];
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
                int i = 0;
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

        private static string saveFileName(string fileNameRev)
        {
            string fileName = fileNameRev;
            if (File.Exists(fileName) == true)
                fileName = getNewName(fileName);
                
            return fileName;
        }

        private static string getNewName(string fileName)
        {
            string newName = fileName;
            if (newName.IndexOf(".") == -1)
                newName += "(副本)";
            else
            {
                string[] filenames = newName.Split(".".ToCharArray());
                string fileMainName = filenames[0];
                string fileExtension = filenames[fileMainName.Length - 1];
                filenames[0] += "(副本)";
                filenames[fileMainName.Length - 1] = "." + filenames[fileMainName.Length - 1]; 
                newName = "";
                foreach (string name in filenames)
                    newName += name;
            }
            while (File.Exists(newName) == true)
            {
                string[] filenames = newName.Split(".".ToCharArray());
                string fileMainName = filenames[0];
                string fileExtension = filenames[fileMainName.Length - 1];
                filenames[0] += "(副本)";
                filenames[fileMainName.Length - 1] = "." + filenames[fileMainName.Length - 1]; 
                newName = "";
                foreach (string name in filenames)
                    newName += name;
            }
            return newName;
        }
    }
}
