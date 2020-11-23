using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for administrators held in the database.
    /// </summary>
    public static class AdministrationDAL
    {

        /// <summary>
        /// Gets the data from administrator queries.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>DataTable containing query data.</returns>
        public static async Task<DataTable> AdminQuery(string query)
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

        public static async Task<DataTable> GenerateReport(DateTime begDate, DateTime endDate)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select V.dateTime as 'Visit Date', V.patientID, CONCAT(P.firstName + ' ' + P.lastName) as 'Patient Name'," +
                               " CONCAT(D.firstName + ' ' + D.lastName) as 'Doctor Name', CONCAT(N.firstName + ' ' + N.lastName) as 'Nurse Name', " +
                               "T.name as 'Test Name', TR.abnormal as 'Test Abnormality', V.finalDiagnosis as 'Final Diagnosis'" +
                               " from visit_info V, patient P, doctor D, nurse N, test_result TR, test T where (dateTime between @begDate and @endDate) and" +
                               "V.patientID = P.patientID and V.doctorID = D.doctorID and V.nurseID = N.nurseID and TR.patientID = V.patientID and TR.dateTime = V.dateTime and" +
                               " TR.testID = T.testID";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@begDate", MySqlDbType.DateTime);
                    comm.Parameters["@begDate"].Value = begDate;
                    comm.Parameters.Add("@endDate", MySqlDbType.DateTime);
                    comm.Parameters["@endDate"].Value = endDate;

                    var dataTable = new DataTable();
                    using (var dataAdapter = new MySqlDataAdapter(comm))
                    {
                        dataAdapter.Fill(dataTable);
                    }

                    return dataTable;
                }
            }
        }

        /// <summary>
        /// Gets an admin from the database.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns>Administrator object</returns>
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

        private static Address GetAddressAndZipNewConnection(int adminId, string address1, string zip)
        {
            return AddressDAL.GetAddressWithAdminId(adminId, address1, zip);
        }
    }
}
