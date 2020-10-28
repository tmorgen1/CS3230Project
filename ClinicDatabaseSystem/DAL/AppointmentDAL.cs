using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class AppointmentDAL
    {
        public static IList<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from appointment";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int pIdOrdinal = reader.GetOrdinal("patientID");
                        int appDateTimeOrdinal = reader.GetOrdinal("dateTime");
                        int dIdOrdinal = reader.GetOrdinal("doctorID");
                        int reasonOrdinal = reader.GetOrdinal("reason");

                        while (reader.Read())
                        {
                            int pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 0;
                            DateTime appDateTime = !reader.IsDBNull(appDateTimeOrdinal)
                                ? reader.GetDateTime(appDateTimeOrdinal)
                                : default(DateTime);
                            int dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 0;
                            string reason = !reader.IsDBNull(reasonOrdinal) ? reader.GetString(reasonOrdinal) : null;

                            appointments.Add(new Appointment(pId, appDateTime, dId, reason));
                        }
                    }
                }
            }

            return appointments;
        }

        public static IList<Appointment> GetAllAppointmentsFromPatientId(int patientId)
        {
            List<Appointment> appointments = new List<Appointment>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select A.dateTime, A.doctorID, A.reason from appointment A, patient P where A.patientID = @id";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32);
                    comm.Parameters["@id"].Value = patientId;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int pIdOrdinal = reader.GetOrdinal("A.patientID");
                        int appDateTimeOrdinal = reader.GetOrdinal("A.dateTime");
                        int dIdOrdinal = reader.GetOrdinal("A.doctorID");
                        int reasonOrdinal = reader.GetOrdinal("reason");

                        while (reader.Read())
                        {
                            int pId = !reader.IsDBNull(pIdOrdinal) ? reader.GetInt32(pIdOrdinal) : 0;
                            DateTime appDateTime = !reader.IsDBNull(appDateTimeOrdinal)
                                ? reader.GetDateTime(appDateTimeOrdinal)
                                : default(DateTime);
                            int dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 0;
                            string reason = !reader.IsDBNull(reasonOrdinal) ? reader.GetString(reasonOrdinal) : null;

                            appointments.Add(new Appointment(pId, appDateTime, dId, reason));
                        }
                    }
                }
            }

            return appointments;
        }

        public static bool InsertAppointment(Appointment appointment)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string insertStatement = "insert into appointment values (@pId, @appDateTime, @dId, @reason)";

                using (MySqlCommand comm = new MySqlCommand(insertStatement, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = appointment.PatientId;
                    comm.Parameters.Add("@appDateTime", MySqlDbType.DateTime);
                    comm.Parameters["@appDateTime"].Value = appointment.ScheduledDate;
                    comm.Parameters.Add("@dId", MySqlDbType.Int32);
                    comm.Parameters["@dId"].Value = appointment.DoctorId;
                    comm.Parameters.Add("@reason", MySqlDbType.String);
                    comm.Parameters["@reason"].Value = appointment.Reason;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
