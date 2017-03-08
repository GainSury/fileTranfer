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
        private static void printFiles(string files)
        {

            Console.WriteLine(files);
            //打印文件名
            string[] filess = files.Split(",".ToCharArray());
            foreach (var sd in filess)
                Console.WriteLine(sd);

        }

        private static void closeSocket(Socket sender)
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        private static string getCurrentFiles()
        {
            DirectoryInfo di = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            string[] tmp = new string[di.GetFiles().Length];
            int i = 0;
            foreach (var fi in di.GetFiles())
            {
                tmp[i++] = fi.Name;
            }
            return String.Join(",", tmp);
        }

    }
}
