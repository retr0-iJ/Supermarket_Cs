using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace cs_IMMANUEL_JOSEPH_2301852215.Repositories
{
    public static class DBConnection
    {
        public static SqlConnection connection;
        public static SqlCommand command;

        public static void Init()
        {
            command = new SqlCommand(); // Always give a new Command Object. Otherwise the command would hold the parameters till die and causes severe
                                           // tragedic problems :)).
            if (connection != null) return;
            connection = new SqlConnection(@"Data Source=.;Initial Catalog=marketDB;Integrated Security=True");
        }
    }
}
