using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Runtime.InteropServices;

namespace carychdb
{
    internal class DB
    {
        MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection("server=localhost; port=3306; username=root; password=; database=carychdb; charset = utf8");

        public void openConnection() { 
        
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySql.Data.MySqlClient.MySqlConnection GetConnection()
        {
            return connection;
        }

    }
}
