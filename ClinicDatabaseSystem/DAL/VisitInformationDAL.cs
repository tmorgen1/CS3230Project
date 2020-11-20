using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for visit information in the database.
    /// </summary>
    public static class VisitInformationDAL
    {
        /// <summary>
        /// Gets the visit information from given appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns>The visit information for the given appointment</returns>
        public static IList<VisitInformation> GetVisitInfoFromAppointment(Appointment appointment)
        {
            List<VisitInformation> visitInfos = new List<VisitInformation>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from visit_info where patientID = @pId and dateTime = @dateTime";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = appointment.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = appointment.ScheduledDate;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int pIdOrdinal = reader.GetOrdinal("patientID");
                        int dateTimeOrdinal = reader.GetOrdinal("dateTime");
                        int systolicBpOrdinal = reader.GetOrdinal("systolicBP");
                        int diastolicBpOrdinal = reader.GetOrdinal("diastolicBP");
                        int bodyTempOrdinal = reader.GetOrdinal("bodyTemp");
                        int pulseOrdinal = reader.GetOrdinal("pulse");
                        int weightOrdinal = reader.GetOrdinal("weight");
                        int symptomsOrdinal = reader.GetOrdinal("symptoms");
                        int initialDiagnosisOrdinal = reader.GetOrdinal("initialDiagnosis");
                        int finalDiagnosisOrdinal = reader.GetOrdinal("finalDiagnosis");

                        while (reader.Read())
                        {
                            int pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 0;
                            DateTime dateTime = !reader.IsDBNull(dateTimeOrdinal)
                                ? reader.GetDateTime(dateTimeOrdinal)
                                : default(DateTime);
                            string systolicBp = !reader.IsDBNull(systolicBpOrdinal)
                                ? reader.GetString(systolicBpOrdinal)
                                : null;
                            string diastolicBp = !reader.IsDBNull(diastolicBpOrdinal)
                                ? reader.GetString(diastolicBpOrdinal)
                                : null;
                            string bodyTemp = !reader.IsDBNull(bodyTempOrdinal)
                                ? reader.GetString(bodyTempOrdinal)
                                : null;
                            string pulse = !reader.IsDBNull(pulseOrdinal) ? reader.GetString(pulseOrdinal) : null;
                            string weight = !reader.IsDBNull(weightOrdinal) ? reader.GetString(weightOrdinal) : null;
                            string symptoms = !reader.IsDBNull(symptomsOrdinal)
                                ? reader.GetString(symptomsOrdinal)
                                : null;
                            string initialDiagnosis = !reader.IsDBNull(initialDiagnosisOrdinal)
                                ? reader.GetString(initialDiagnosisOrdinal)
                                : null;
                            string finalDiagnosis = !reader.IsDBNull(finalDiagnosisOrdinal)
                                ? reader.GetString(finalDiagnosisOrdinal)
                                : null;

                            visitInfos.Add(new VisitInformation(pId, dateTime, systolicBp, diastolicBp, bodyTemp, pulse, weight, symptoms, initialDiagnosis, finalDiagnosis));
                        }
                    }
                }
            }

            return visitInfos;
        }

        /// <summary>
        /// Inserts the visit information into the database.
        /// </summary>
        /// <param name="visitInfo">The visit information.</param>
        /// <returns>True if the visit information is inserted into the database</returns>
        public static bool InsertVisitInfo(VisitInformation visitInfo)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string insertStatement = "insert into visit_info values (@pId, @dateTime, @systolic, @diastolic, @bodyTemp, @pulse, @weight, @symptoms, @initialDiagnosis, @finalDiagnosis)";

                using (MySqlCommand comm = new MySqlCommand(insertStatement, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = visitInfo.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = visitInfo.VisitDateTime;
                    comm.Parameters.Add("@systolic", MySqlDbType.String);
                    comm.Parameters["@systolic"].Value = visitInfo.SystolicBp;
                    comm.Parameters.Add("@diastolic", MySqlDbType.String);
                    comm.Parameters["@diastolic"].Value = visitInfo.DiastolicBp;
                    comm.Parameters.Add("@bodyTemp", MySqlDbType.String);
                    comm.Parameters["@bodyTemp"].Value = visitInfo.BodyTemp;
                    comm.Parameters.Add("@pulse", MySqlDbType.String);
                    comm.Parameters["@pulse"].Value = visitInfo.Pulse;
                    comm.Parameters.Add("@weight", MySqlDbType.String);
                    comm.Parameters["@weight"].Value = visitInfo.Weight;
                    comm.Parameters.Add("@symptoms", MySqlDbType.String);
                    comm.Parameters["@symptoms"].Value = visitInfo.Symptoms;
                    comm.Parameters.Add("@initialDiagnosis", MySqlDbType.String);
                    comm.Parameters["@initialDiagnosis"].Value = visitInfo.InitialDiagnosis;
                    comm.Parameters.Add("@finalDiagnosis", MySqlDbType.String);
                    comm.Parameters["@finalDiagnosis"].Value = visitInfo.FinalDiagnosis;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Deletes the visit information.
        /// </summary>
        /// <param name="visitInfo">The visit information.</param>
        /// <returns>True if visit info is deleted</returns>
        public static bool DeleteVisitInfo(VisitInformation visitInfo)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string deleteStatement = "delete from visit_info where patientID = @pId and dateTime = @dateTime";

                using (MySqlCommand comm = new MySqlCommand(deleteStatement, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = visitInfo.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = visitInfo.VisitDateTime;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Edits the visit information and updates the database.
        /// </summary>
        /// <param name="oldVisitInfo">The old visit information.</param>
        /// <param name="newVisitInfo">The new visit information.</param>
        /// <returns>True if visit info is updated</returns>
        public static bool EditVisitInfo(VisitInformation oldVisitInfo, VisitInformation newVisitInfo)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string editStatement = "update visit_info set patientID = @pId, dateTime = @dateTime, systolicBP = @systolic, diastolicBP = @diastolic, bodyTemp = @bodyTemp, " +
                                       "pulse = @pulse, weight = @weight, symptoms = @symptoms, initialDiagnosis = @initialDiagnosis, finalDiagnosis = @finalDiagnosis" +
                                       " where patientID = @oldPId and dateTime = @oldDateTime";

                using (MySqlCommand comm = new MySqlCommand(editStatement, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = newVisitInfo.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = newVisitInfo.VisitDateTime;
                    comm.Parameters.Add("@systolic", MySqlDbType.String);
                    comm.Parameters["@systolic"].Value = newVisitInfo.SystolicBp;
                    comm.Parameters.Add("@diastolic", MySqlDbType.String);
                    comm.Parameters["@diastolic"].Value = newVisitInfo.DiastolicBp;
                    comm.Parameters.Add("@bodyTemp", MySqlDbType.String);
                    comm.Parameters["@bodyTemp"].Value = newVisitInfo.BodyTemp;
                    comm.Parameters.Add("@pulse", MySqlDbType.String);
                    comm.Parameters["@pulse"].Value = newVisitInfo.Pulse;
                    comm.Parameters.Add("@weight", MySqlDbType.String);
                    comm.Parameters["@weight"].Value = newVisitInfo.Weight;
                    comm.Parameters.Add("@symptoms", MySqlDbType.String);
                    comm.Parameters["@symptoms"].Value = newVisitInfo.Symptoms;
                    comm.Parameters.Add("@initialDiagnosis", MySqlDbType.String);
                    comm.Parameters["@initialDiagnosis"].Value = newVisitInfo.InitialDiagnosis;
                    comm.Parameters.Add("@finalDiagnosis", MySqlDbType.String);
                    comm.Parameters["@finalDiagnosis"].Value = newVisitInfo.FinalDiagnosis;
                    comm.Parameters.Add("@oldPId", MySqlDbType.Int32);
                    comm.Parameters["@oldPId"].Value = oldVisitInfo.PatientId;
                    comm.Parameters.Add("@oldDateTime", MySqlDbType.DateTime);
                    comm.Parameters["@oldDateTime"].Value = oldVisitInfo.VisitDateTime;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Adds the final diagnosis into an existing visit info.
        /// </summary>
        /// <param name="visitInfo">The visit information.</param>
        /// <param name="finalDiagnosis">The final diagnosis.</param>
        /// <returns>True if visit info updates</returns>
        public static bool AddFinalDiagnosis(VisitInformation visitInfo, string finalDiagnosis)
        {
            VisitInformation oldVisitInfo = visitInfo;
            VisitInformation newVisitInfo = new VisitInformation(oldVisitInfo.PatientId, oldVisitInfo.VisitDateTime, oldVisitInfo.SystolicBp, oldVisitInfo.DiastolicBp,
                oldVisitInfo.BodyTemp, oldVisitInfo.Pulse, oldVisitInfo.Weight, oldVisitInfo.Symptoms, oldVisitInfo.InitialDiagnosis, finalDiagnosis);

            return EditVisitInfo(oldVisitInfo, newVisitInfo);
        }
    }
}
