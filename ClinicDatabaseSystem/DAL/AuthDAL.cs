using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public class AuthDAL
    {
        public static bool AuthenticateNurse(string username, string password)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select count(*) from account_credential where accountID = @username and password = @password";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@username", MySqlDbType.VarChar);
                    comm.Parameters["@username"].Value = username;
                    comm.Parameters.Add("@password", MySqlDbType.VarChar);
                    comm.Parameters["@password"].Value = password;

                    int count = Convert.ToInt32(comm.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}
