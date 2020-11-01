using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class VisitInformationDAL
    {
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
                            string symptoms = !reader.IsDBNull(symptomsOrdinal)
                                ? reader.GetString(symptomsOrdinal)
                                : null;
                            string initialDiagnosis = !reader.IsDBNull(initialDiagnosisOrdinal)
                                ? reader.GetString(initialDiagnosisOrdinal)
                                : null;
                            string finalDiagnosis = !reader.IsDBNull(finalDiagnosisOrdinal)
                                ? reader.GetString(finalDiagnosisOrdinal)
                                : null;

                            visitInfos.Add(new VisitInformation(pId, dateTime, systolicBp, diastolicBp, bodyTemp, pulse, symptoms, initialDiagnosis, finalDiagnosis));
                        }
                    }
                }
            }

            return visitInfos;
        }

        public static bool InsertVisitInfo(VisitInformation visitInfo)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string insertStatement = "insert into visit_info values (@pId, @dateTime, @systolic, @diastolic, @bodyTemp, @pulse, @symptoms, @initialDiagnosis, @finalDiagnosis)";

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
    }
}
