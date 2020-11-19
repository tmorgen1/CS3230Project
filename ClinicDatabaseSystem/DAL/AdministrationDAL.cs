using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class AdministrationDAL
    {
        public static DataTable AdminQuery(string query)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    var dataTable = new DataTable();
                    using (var dataAdapter = new MySqlDataAdapter(comm))
                    {
                        dataAdapter.Fill(dataTable);
                    }

                    return dataTable;
                }
            }
        }

        public static Administrator GetAdmin(int adminId)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from administrator where adminID = @aId";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@aId", MySqlDbType.Int32);
                    comm.Parameters["@aId"].Value = adminId;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int adminIdOrdinal = reader.GetOrdinal("adminID");
                        int lastNameOrdinal = reader.GetOrdinal("lastName");
                        int firstNameOrdinal = reader.GetOrdinal("firstName");
                        int dobOrdinal = reader.GetOrdinal("dob");
                        int phoneNumberOrdinal = reader.GetOrdinal("phoneNumber");
                        int accountIdOrdinal = reader.GetOrdinal("accountID");
                        int addressOrdinal = reader.GetOrdinal("address");
                        int zipOrdinal = reader.GetOrdinal("zip");

                        while (reader.Read())
                        {
                            int adminIdLocal = !reader.IsDBNull(adminIdOrdinal) ? reader.GetInt32(adminIdOrdinal) : 0;
                            string lastName = !reader.IsDBNull(lastNameOrdinal)
                                ? reader.GetString(lastNameOrdinal)
                                : null;
                            string firstName = !reader.IsDBNull(firstNameOrdinal)
                                ? reader.GetString(firstNameOrdinal)
                                : null;
                            DateTime dob = !reader.IsDBNull(dobOrdinal)
                                ? reader.GetDateTime(dobOrdinal)
                                : default(DateTime);
                            string phoneNumber = !reader.IsDBNull(phoneNumberOrdinal)
                                ? reader.GetString(phoneNumberOrdinal)
                                : null;

                            string accountId = !reader.IsDBNull(accountIdOrdinal)
                                ? reader.GetString(accountIdOrdinal)
                                : null;
                            string address1 = !reader.IsDBNull(addressOrdinal)
                                ? reader.GetString(addressOrdinal)
                                : null;
                            string zip = !reader.IsDBNull(zipOrdinal) ? reader.GetString(zipOrdinal) : null;

                            Address address = GetAddressAndZipNewConnection(adminId, address1, zip);
                            return new Administrator(adminIdLocal, lastName, firstName, dob, phoneNumber, accountId, address1, zip);
                        }
                    }
                }
            }

            return null;
        }

        private static Address GetAddressAndZipNewConnection(int nurseId, string address1, string zip)
        {
            return AddressDAL.GetAddressWithNurseId(nurseId, address1, zip);
        }
    }
}
