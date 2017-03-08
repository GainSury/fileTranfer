using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public partial class serverTest
    {
        private static string getChoice(Socket handler)
        {
            byte[] revMsgChoice = new byte[BufferSize];
            int bytesRev = handler.Receive(revMsgChoice);
            string choice = Encoding.ASCII.GetString(revMsgChoice, 0, bytesRev);
            return choice;
        }
    }
}
