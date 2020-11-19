using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class AdministrationDAL
    {
        public static void AdminQuery(string query)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                        }
                    }
                }
            }
        }
    }
}
