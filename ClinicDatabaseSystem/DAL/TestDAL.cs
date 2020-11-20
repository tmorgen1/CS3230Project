using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for tests in the database.
    /// </summary>
    public static class TestDAL
    {
        /// <summary>
        /// Gets the test types from the database.
        /// </summary>
        /// <returns>Collection of test types</returns>
        public static IList<Test> GetTestTypes()
        {
            List<Test> testTypes = new List<Test>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from test";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int tIdOrdinal = reader.GetOrdinal("testID");
                        int tNameOrdinal = reader.GetOrdinal("name");

                        while (reader.Read())
                        {
                            int testId = !reader.IsDBNull(tIdOrdinal) ? reader.GetInt32(tIdOrdinal) : 0;
                            string tName = !reader.IsDBNull(tNameOrdinal) ? reader.GetString(tNameOrdinal) : null;

                            testTypes.Add(new Test(testId, tName));
                        }
                    }
                }
            }

            return testTypes;
        }
    }
}
