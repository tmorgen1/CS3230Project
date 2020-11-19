using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Hashing;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class AuthDAL
    {
        private const string HexKey = "t3stk3y";

        public static int AuthenticateNurse(string username, string password)
        {
            BlowFish bf = new BlowFish(HexKey);
            string newPass = bf.Encrypt_ECB(password);

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select N.nurseID, A.accountID from account_credential A, nurse N where A.accountID = @username and A.password = @password and A.accountID = N.accountID";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@username", MySqlDbType.VarChar);
                    comm.Parameters["@username"].Value = username;
                    comm.Parameters.Add("@password", MySqlDbType.VarChar);
                    comm.Parameters["@password"].Value = newPass;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int usernameOrdinal = reader.GetOrdinal("accountID");
                        int nurseIdOrdinal = reader.GetOrdinal("nurseID");

                        while (reader.Read())
                        {
                            string readUsername = !reader.IsDBNull(usernameOrdinal)
                                ? reader.GetString(usernameOrdinal)
                                : null;
                            int nurseId = !reader.IsDBNull(nurseIdOrdinal)
                                ? reader.GetInt32(nurseIdOrdinal)
                                : 0;

                            if (readUsername == null)
                            {
                                continue;
                            }
                            if (readUsername.Equals(username))
                            {
                                return nurseId;
                            }
                        }
                    }
                }
            }

            return 0;
        }

        public static int AuthenticateAdmin(string username, string password)
        {
            BlowFish bf = new BlowFish(HexKey);
            string newPass = bf.Encrypt_ECB(password);

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select A.adminID, AC.accountID from administrator A, account_credential AC where AC.accountID = @username and AC.password = @password and A.accountID = AC.accountID";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@username", MySqlDbType.VarChar);
                    comm.Parameters["@username"].Value = username;
                    comm.Parameters.Add("@password", MySqlDbType.VarChar);
                    comm.Parameters["@password"].Value = newPass;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int adminIdOrdinal = reader.GetOrdinal("adminID");
                        int usernameOrdinal = reader.GetOrdinal("accountID");

                        while (reader.Read())
                        {
                            string readUsername = !reader.IsDBNull(usernameOrdinal)
                                ? reader.GetString(usernameOrdinal) 
                                : null;
                            int adminId = !reader.IsDBNull(adminIdOrdinal) 
                                ? reader.GetInt32(adminIdOrdinal) 
                                : 0;

                            if (readUsername == null)
                            {
                                continue;
                            }
                            if (readUsername.Equals(username))
                            {
                                return adminId;
                            }
                        }
                    }
                }
            }

            return 0;
        }
    }
}
