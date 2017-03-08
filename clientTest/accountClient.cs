using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    
    public partial class clietTest
    {
        const int BufferSize = 4096;

        private static bool signIn(Socket sender)
        {
            const string success = "<done>";
            const string fail = "<fail>";
            //userAndPassword 是 username + "," + password 字符串形式，也就是用户名和密码中不能含有逗号
            //登录字符串可以含有中文字符。
            string userAndPassword = showAccountMenu();
            byte[] userBytes = Encoding.UTF8.GetBytes(userAndPassword);
            sender.Send(userBytes);

            //接收结果
            byte[] resultBytes = new byte[BufferSize];
            int resultRevNum = sender.Receive(resultBytes);
            string resultString = Encoding.ASCII.GetString(resultBytes, 0, resultRevNum);
            if (resultString.IndexOf(success) > -1)
                return true;
            else
                return false;
        }

        private static void signInFail(Socket sender)
        {
            Console.WriteLine("no such user!fail to sign in!");
        }
        private static string showAccountMenu()
        {
            Console.WriteLine("Sign in------------------");
            string username = getInput("UserName: ");
            string password = getInput("Password: ");

            


            return username+","+ password;
        }

        private static string getInput(string uiMsg)
        {
            UsernameFlag:
            Console.Write(uiMsg);
            string result = Console.ReadLine();
            if (result == "")
            {
                Console.WriteLine("null input!");
                goto UsernameFlag;
            }
            if (result.IndexOf(",") > -1)
            {
                Console.WriteLine("cannot include ,");
                goto UsernameFlag;
            }
            return result;
        }


    }


}


