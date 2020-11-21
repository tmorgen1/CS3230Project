using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for test results in the database.
    /// </summary>
    public static class TestResultDAL
    {
        /// <summary>
        /// Gets the test results from patient and visit information.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="visitDateTime">The visit date time.</param>
        /// <returns>Collection of test results</returns>
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
                        int abnormalOrdinal = reader.GetOrdinal("abnormal");
                        int testResultDateOrdinal = reader.GetOrdinal("testResultDate");

                        while (reader.Read())
                        {
                            int testId = !reader.IsDBNull(tIdOrdinal) ? reader.GetInt32(tIdOrdinal) : 0;
                            int pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 0;
                            DateTime dateTime = !reader.IsDBNull(dateOrdinal)
                                ? reader.GetDateTime(dateOrdinal)
                                : default(DateTime);
                            string results = !reader.IsDBNull(resultsOrdinal) ? reader.GetString(resultsOrdinal) : null;
                            bool abnormal = !reader.IsDBNull(abnormalOrdinal) && reader.GetBoolean(abnormalOrdinal);
                            DateTime testResultDateTime = !reader.IsDBNull(testResultDateOrdinal)
                                ? reader.GetDateTime(testResultDateOrdinal)
                                : default(DateTime);

                            testResults.Add(new TestResult(testId, patientId, dateTime, results, abnormal, testResultDateTime));
                        }
                    }
                }
            }

            return testResults;
        }

        /// <summary>
        /// Inserts the test result into the database.
        /// </summary>
        /// <param name="testResult">The test result.</param>
        /// <returns>True if test result inserted properly</returns>
        public static bool InsertTestResult(TestResult testResult)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string insertStatement = "insert into test_result values (@tId, @pId, @dateTime, @results, @abnormal, @testResultDate);";

                using (MySqlCommand comm = new MySqlCommand(insertStatement, conn))
                {
                    comm.Parameters.Add("@tId", MySqlDbType.Int32);
                    comm.Parameters["@tId"].Value = testResult.TestId;
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = testResult.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = testResult.VisitDateTime;
                    comm.Parameters.Add("@results", MySqlDbType.VarChar);
                    comm.Parameters["@results"].Value = testResult.Results;
                    comm.Parameters.Add("@abnormal", MySqlDbType.Int16);
                    comm.Parameters["@abnormal"].Value = testResult.Abnormal;
                    comm.Parameters.Add("@testResultDate", MySqlDbType.DateTime);
                    comm.Parameters["@testResultDate"].Value = testResult.TestResultDateTime;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Edits the test result and updates the database.
        /// </summary>
        /// <param name="newTestResult">The new test result.</param>
        /// <param name="oldTestResult">The old test result.</param>
        /// <returns>True if test result is updated successfully</returns>
        public static bool EditTestResult(TestResult newTestResult, TestResult oldTestResult)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string editStatement = "update test_result set testID = @tId, patientID = @pId, dateTime = @dateTime," +
                                       " results = @results, abnormal = @abnormal, testResultDate = @testResultDate where " +
                                       "testID = @oldTId and patientID = @oldPId and dateTime = @oldDateTime";

                using (MySqlCommand comm = new MySqlCommand(editStatement, conn))
                {
                    comm.Parameters.Add("@tId", MySqlDbType.Int32);
                    comm.Parameters["@tId"].Value = newTestResult.TestId;
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = newTestResult.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = newTestResult.VisitDateTime;
                    comm.Parameters.Add("@results", MySqlDbType.VarChar);
                    comm.Parameters["@results"].Value = newTestResult.Results;
                    comm.Parameters.Add("@abnormal", MySqlDbType.Int16);
                    comm.Parameters["@abnormal"].Value = newTestResult.Abnormal;
                    comm.Parameters.Add("@testResultDate", MySqlDbType.DateTime);
                    comm.Parameters["@testResultDate"].Value = newTestResult.TestResultDateTime;
                    comm.Parameters.Add("@oldTId", MySqlDbType.Int32);
                    comm.Parameters["@oldTId"].Value = oldTestResult.TestId;
                    comm.Parameters.Add("@oldPId", MySqlDbType.Int32);
                    comm.Parameters["@oldPId"].Value = oldTestResult.PatientId;
                    comm.Parameters.Add("@oldDateTime", MySqlDbType.DateTime);
                    comm.Parameters["@oldDateTime"].Value = oldTestResult.VisitDateTime;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
