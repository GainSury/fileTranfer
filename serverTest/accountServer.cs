using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public partial class serverTest
    {
        private static bool AccountValid(Socket handler)
        {
            byte[] accountBytes = new byte[BufferSize];
            int revNum = handler.Receive(accountBytes);
            string userAndPassword = Encoding.UTF8.GetString(accountBytes, 0, revNum);
            string[] userPArray = userAndPassword.Split(",".ToCharArray());
 
            string user = userPArray[0];
            string password = userPArray[1];

            if (isValidByDb(user,password))
                return true;
            else
                return false;
        }

        private static bool isValidByDb(string user, string password)
        {
            //数据库验证
            string strconn = "Data Source=localhost;Database=UserDb;UID=jdbcTest;Pwd=jdbcTest1234";

            SqlConnection connection = new SqlConnection(strconn);

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "select PassWords from dbo.tbName where UserName = @username;";
            string username = user;
            command.Parameters.Add(new SqlParameter("@username", username));
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            
            if (reader[0].Equals(password))
                return true;
            else
                return false;
            
 
        }
    }
}
