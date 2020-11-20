using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for appointments held in the database.
    /// </summary>
    public static class AppointmentDAL
    {
        /// <summary>
        /// Gets all appointments from database.
        /// </summary>
        /// <returns>collection of all appointments</returns>
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

        /// <summary>
        /// Gets all appointments for specific patient from database.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>Collection of appointments for a specific patient</returns>
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

        /// <summary>
        /// Inserts the appointment into the database.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA appointment inserted into table.</returns>
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

        /// <summary>
        /// Deletes the appointment from database.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA appointment deleted from table.</returns>
        public static bool DeleteAppointment(Appointment appointment)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string deleteStatement = "delete from appointment where patientID = @pId and dateTime = @dateTime";

                using (MySqlCommand comm = new MySqlCommand(deleteStatement, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = appointment.PatientId;
                    comm.Parameters.Add("@dateTime", MySqlDbType.DateTime);
                    comm.Parameters["@dateTime"].Value = appointment.ScheduledDate;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Edits the appointment and updates in the database.
        /// </summary>
        /// <param name="prevAppointment">The previous appointment.</param>
        /// <param name="newAppointment">The new appointment.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA appointment is updated in the table.</returns>
        public static bool EditAppointment(Appointment prevAppointment, Appointment newAppointment)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string editStatement = "update appointment set patientID = @pId, dateTime = @dateTime, doctorID = @dId, reason = @reason where patientID = @opId and dateTime = @odateTime";

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
