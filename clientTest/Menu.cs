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
        private static int showMenu()
        {
            int choice = 0;
            Console.WriteLine("1.download files--------------");
            Console.WriteLine("2.upload files----------------");
            Console.WriteLine("3.exit------------------------");
            choice = getChoice();
            return choice;

        }

        static int getChoice()
        {
            //增加菜单时，需要在errormsg里加上 1&2 ，然后在choice里加上1 和 2,然后在showMenu函数里加上2选项
            string errormsg = "choice too big,please input 1 or 2 or 3";
            int choice = 0;

            int flag = 0;
            Console.Write("Please input your choice(integer):  ");
            while (flag == 0)
            {
                try
                {
                    choice = Int32.Parse(Console.ReadLine());
                    if (choice == 1 || choice == 2 || choice == 3)
                        flag = 1;
                    else
                        Console.WriteLine(errormsg);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(errormsg);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("format error,please input integer");
                }
            }
            return choice;
        }

    }
}
