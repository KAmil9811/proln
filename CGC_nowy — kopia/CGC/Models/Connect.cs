using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CGC.Models
{
    public class Connect
    {
        public Connect()
        {
            cnn = new SqlConnection(builder.ConnectionString);
        }

        static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "projekt-inz.database.windows.net",
            Database = "projekt-inz",
            UserID = "Michal",
            Password = "lemES98naw141",
            //SslMode = MySqlSslMode.Required,
        };

        public SqlConnection cnn { get; set; }
    }
}
