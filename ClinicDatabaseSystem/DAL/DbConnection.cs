using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class DbConnection
    {
        private static readonly string connString = "Server=160.10.25.16;Port=3306;Database=cs3230f20h;Uid=cs3230f20h;Pwd=Fi8q2ZyuVWUwwUMz;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connString);
        }
    }
}
