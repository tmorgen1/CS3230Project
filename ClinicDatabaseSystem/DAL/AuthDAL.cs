﻿using System;
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
                    "select N.nurseID, A.accountID from account_credential A, nurse N where A.accountID = @username and A.password = @password and A.accountID = N.accountID";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@username", MySqlDbType.VarChar);
                    comm.Parameters["@username"].Value = username;
                    comm.Parameters.Add("@password", MySqlDbType.VarChar);
                    comm.Parameters["@password"].Value = password;

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

                    return 0;
                }
            }
        }
    }
}
