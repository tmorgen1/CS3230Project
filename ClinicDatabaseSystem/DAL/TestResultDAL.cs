using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class TestResultDAL
    {
        public static IList<TestResult> GetTestResultsFromPatientAndVisitInfo(int patientId, DateTime visitDateTime)
        {
            List<TestResult> testResults = new List<TestResult>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from test_result where patientID = @pId and dateTime = @dateTime";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = patientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = visitDateTime;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int tIdOrdinal = reader.GetOrdinal("testID");
                        int pIdOrdinal = reader.GetOrdinal("patientID");
                        int dateOrdinal = reader.GetOrdinal("dateTime");
                        int resultsOrdinal = reader.GetOrdinal("results");

                        while (reader.Read())
                        {
                            int testId = !reader.IsDBNull(tIdOrdinal) ? reader.GetInt32(tIdOrdinal) : 0;
                            int pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 0;
                            DateTime dateTime = !reader.IsDBNull(dateOrdinal)
                                ? reader.GetDateTime(dateOrdinal)
                                : default(DateTime);
                            string results = !reader.IsDBNull(resultsOrdinal) ? reader.GetString(resultsOrdinal) : null;

                            testResults.Add(new TestResult(testId, patientId, dateTime, results));
                        }
                    }
                }
            }

            return testResults;
        }
    }
}
