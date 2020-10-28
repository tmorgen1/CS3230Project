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
        public static IList<Appointment> GetAllAppointmentsFromPatientId(int patientId)
        {
            List<Appointment> appointments = new List<Appointment>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

            }

            return appointments;
        }
    }
}
