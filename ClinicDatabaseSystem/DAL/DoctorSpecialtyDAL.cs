using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class DoctorSpecialtyDAL
    {
        public static IList<DoctorSpecialty> GetSpecialtyWithDoctorId(int doctorId)
        {
            List<DoctorSpecialty> specialties = new List<DoctorSpecialty>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select HS.doctorID, HS.specialtyType from has_specialty HS, doctor D where HS.doctorID = @id";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32);
                    comm.Parameters["@id"].Value = doctorId;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("doctorID");
                        int specialtyOrdinal = reader.GetOrdinal("specialtyType");

                        while (reader.Read())
                        {
                            int id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 0;
                            string specialtyType = !reader.IsDBNull(specialtyOrdinal)
                                ? reader.GetString(specialtyOrdinal)
                                : null;

                            specialties.Add(new DoctorSpecialty(id, specialtyType));
                        }
                    }
                }
            }

            return specialties;
        }
    }
}
