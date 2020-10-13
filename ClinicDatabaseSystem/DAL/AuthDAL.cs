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
        public static int AuthenticateNurse(string username, string password)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select N.nurseID from account_credential A, nurse N where A.accountID = @username and A.password = @password and A.accountID = N.accountID";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@username", MySqlDbType.VarChar);
                    comm.Parameters["@username"].Value = username;
                    comm.Parameters.Add("@password", MySqlDbType.VarChar);
                    comm.Parameters["@password"].Value = password;

                    int count = Convert.ToInt32(comm.ExecuteScalar());

                    return count;
                }
            }
        }
    }
}
