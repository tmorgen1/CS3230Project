using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for nurses in the database.
    /// </summary>
    public static class NurseDAL
    {
        /// <summary>
        /// Gets the nurse by nurse ID from the database.
        /// </summary>
        /// <param name="nurseId">The nurse identifier.</param>
        /// <returns>The nurse object</returns>
        public static Nurse GetNurse(int nurseId)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from nurse where nurseID = @nurseId";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@nurseId", MySqlDbType.Int32);
                    comm.Parameters["@nurseId"].Value = nurseId;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int nurseIdOrdinal = reader.GetOrdinal("nurseID");
                        int lastNameOrdinal = reader.GetOrdinal("lastName");
                        int firstNameOrdinal = reader.GetOrdinal("firstName");
                        int dobOrdinal = reader.GetOrdinal("dob");
                        int phoneNumberOrdinal = reader.GetOrdinal("phoneNumber");
                        int accountIdOrdinal = reader.GetOrdinal("accountID");
                        int addressOrdinal = reader.GetOrdinal("address");
                        int zipOrdinal = reader.GetOrdinal("zip");

                        while (reader.Read())
                        {
                            int nurseIdLocal = !reader.IsDBNull(nurseIdOrdinal) ? reader.GetInt32(nurseIdOrdinal) : 0;
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

                            Address address = GetAddressAndZipNewConnection(nurseId, address1, zip);
                            return new Nurse(nurseIdLocal, lastName, firstName, dob, phoneNumber, accountId, address);
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
