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
                    "select dateTime, doctorID, reason from appointment where patientID = @id";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32);
                    comm.Parameters["@id"].Value = patientId;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int dIdOrdinal = reader.GetOrdinal("doctorID");
                        int appDateTimeOrdinal = reader.GetOrdinal("dateTime");
                        int reasonOrdinal = reader.GetOrdinal("reason");

                        while (reader.Read())
                        {
                            DateTime appDateTime = !reader.IsDBNull(appDateTimeOrdinal)
                                ? reader.GetDateTime(appDateTimeOrdinal)
                                : default(DateTime);
                            int dId = !reader.IsDBNull(dIdOrdinal) ? reader.GetInt32(dIdOrdinal) : 0;
                            string reason = !reader.IsDBNull(reasonOrdinal) ? reader.GetString(reasonOrdinal) : null;

                            appointments.Add(new Appointment(patientId, appDateTime, dId, reason));
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

        public static bool EditAppointment(Appointment prevAppointment, Appointment newAppointment)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string editStatement = "update appointment set patientID = @pId, dateTime = @dateTime, doctorID = @dId, reason = @reason where patientID = @opId, dateTime = @odateTime";

                using (MySqlCommand comm = new MySqlCommand(editStatement, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = newAppointment.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = newAppointment.ScheduledDate;
                    comm.Parameters.Add("@dId", MySqlDbType.Int32);
                    comm.Parameters["@dId"].Value = newAppointment.DoctorId;
                    comm.Parameters.Add("@reason", MySqlDbType.String);
                    comm.Parameters["@reason"].Value = newAppointment.Reason;
                    comm.Parameters.Add("@opId", MySqlDbType.Int32);
                    comm.Parameters["@opId"].Value = prevAppointment.PatientId;
                    comm.Parameters.Add("@odateTime", MySqlDbType.DateTime);
                    comm.Parameters["@odateTime"].Value = prevAppointment.ScheduledDate;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
