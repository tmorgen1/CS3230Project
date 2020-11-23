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

        /// <summary>
        /// Generates a report of all visit information between the two given dates.
        /// </summary>
        /// <param name="begDate"></param>
        /// <param name="endDate"></param>
        /// <returns>DataTable containing query data.</returns>
        public static DataTable GenerateReport(DateTime begDate, DateTime endDate)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT V.dateTime AS 'Visit Date', V.patientID AS 'Patient ID', CONCAT(P.firstName, ' ', P.lastName) " +
                               "AS 'Patient Name', CONCAT(D.firstName, ' ', D.lastName) AS 'Doctor Name', CONCAT(N.firstName, ' ', N.lastName) " +
                               "AS 'Nurse Name', TR.abnormal AS 'Test Abnormality', T.name AS 'Test Name', V.finalDiagnosis AS 'Final Diagnosis' " +
                               "FROM visit_info V left outer join test_result TR on V.patientID = TR.patientID and V.dateTime = TR.dateTime left " +
                               "outer join test T on T.testID = TR.testID, patient P, doctor D, nurse N, appointment A WHERE ( V.dateTime BETWEEN @begDate AND @endDate ) " +
                               "AND V.patientID = P.patientID AND A.doctorID = D.doctorID AND V.nurseID = N.nurseID AND A.dateTime = V.dateTime AND A.patientID = V.patientID" +
                               " ORDER BY V.dateTime, P.lastName";

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
