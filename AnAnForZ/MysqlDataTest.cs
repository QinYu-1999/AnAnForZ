using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace AnAnForZ
{
    class MysqlDataTest
    {
        public MySqlConnection getConnection()
        {
            String connetStr = "server=localhost;port=3306;user=root;password=root;database=o3o;Charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connetStr);
            return conn;
        }

        public void select()
        {
            MySqlConnection conn = getConnection(); //连接数据库
            conn.Open(); //打开数据库连接
            string sql = "select * from user'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                string str = mdr["authority"].ToString();//接收一个返回值
                                                         //对查询到数据进行相应的操作
            }
            conn.Close();
        }

/*        public static void Main(String[] args)
        {
            MysqlDataTest mysqlDataTest = new MysqlDataTest();
            MySqlConnection a = mysqlDataTest.getConnection();
            Console.WriteLine("{0}", a);
        }*/
    }
    


}
