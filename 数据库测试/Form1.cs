using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace 数据库测试
{
    public partial class Form1 : Form
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet myset = new DataSet();
        DataTable dt = new DataTable();
        public Form1()
        {

            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string strconn = "Data Source=localhost;Database=UserDb;UID=jdbcTest;Pwd=jdbcTest1234";

            SqlConnection connection = new SqlConnection(strconn);

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "select PassWords from dbo.tbName where UserName = @username;";
            string username = "gain";
            command.Parameters.Add(new SqlParameter("@username", username));
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                MessageBox.Show(String.Format("{0}", reader[0]));
            }

            //using (SqlConnection connection = new SqlConnection(
            //   strconn))
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand(queryString, connection);
            //    SqlDataReader reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        Console.WriteLine(String.Format("{0}", reader[0]));
            //    }
            //}




















            //using (SqlConnection connection = new SqlConnection(
            //   strconn))
            //{
            //    SqlCommand command = new SqlCommand();
            //    command.Connection = connection;
            //    command.CommandText = "select * from UserInfo where userId = @id and userSex = @sex;";
            //    int idd = 2005;
            //    string userSex = "nan";
            //    command.Parameters.Add(new SqlParameter("@id", idd));
            //    command.Parameters.Add(new SqlParameter("@sex", userSex));
            //    command.Connection.Open();
            //    int newUserId = (Int32)command.ExecuteScalar();
            //    MessageBox.Show(newUserId.ToString());
            //}


            //using (SqlConnection conn = new SqlConnection(strconn))
            //{
            //    conn.Open();
            //    SqlCommand dbquery = new SqlCommand();            //构造sql命令
            //    dbquery.Connection = conn;
            //    dbquery.CommandText = "select * from UserInfo";
            //    SqlDataReader dbreader = dbquery.ExecuteReader();
            //    string temp = "";
            //    bool hasrow = dbreader.HasRows;
            //    if (hasrow)
            //        MessageBox.Show("存在用户");
            //    while (dbreader.Read())
            //        //temp = temp + dbreader["username"].ToString()+"\n";
            //        //temp = temp + dbreader[0].ToString()+"\n";
            //        if (!dbreader.IsDBNull(0))
            //        {
            //            //temp = temp + dbreader.GetInt32(0) +" " + dbreader.GetInt32(1) + " " + dbreader.GetString(0) + " " + dbreader.GetString(1) + " " + dbreader.GetString(2) + "\n";
            //            temp = temp + dbreader[0] + "\t" + dbreader[1] + "\t" + dbreader[2]+"\t\t\t\t\t" + dbreader[3]+ "\t" + dbreader[4] + "\n";

            //        }
            //        else
            //            temp = temp + "   ";
            //    dbreader.Close();
            //    MessageBox.Show(temp);
            //}



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(da);
            da.Update(dt);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
