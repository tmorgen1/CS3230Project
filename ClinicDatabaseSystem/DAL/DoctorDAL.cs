using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class DoctorDAL
    {
        public static IList<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from doctor";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("doctorID");
                        int lastNameOrdinal = reader.GetOrdinal("lastName");
                        int firstNameOrdinal = reader.GetOrdinal("firstName");
                        int dobOrdinal = reader.GetOrdinal("dob");
                        int phoneNumberOrdinal = reader.GetOrdinal("phoneNumber");
                        int addressOrdinal = reader.GetOrdinal("address");
                        int zipOrdinal = reader.GetOrdinal("zip");

                        while (reader.Read())
                        {
                            int id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 0;
                            string lastName = !reader.IsDBNull(lastNameOrdinal)
                                ? reader.GetString(lastNameOrdinal)
                                : null;
                            string firstName = !reader.IsDBNull(firstNameOrdinal)
                                ? reader.GetString(firstNameOrdinal)
                                : null;
                            DateTime dob = !reader.IsDBNull(dobOrdinal)
                                ? reader.GetDateTime(dobOrdinal)
                                : default(DateTime);
                            string phoneNumber = !reader.IsDBNull(phoneNumberOrdinal)
                                ? reader.GetString(phoneNumberOrdinal)
                                : null;
                            string address1 = !reader.IsDBNull(addressOrdinal)
                                ? reader.GetString(addressOrdinal)
                                : null;
                            string zip = !reader.IsDBNull(zipOrdinal) ? reader.GetString(zipOrdinal) : null;
                            Address address = GetAddressAndZipNewConnection(id, address1, zip);
                            IList<DoctorSpecialty> specialties = GetSpecialtiesNewConnection(id);

                            doctors.Add(new Doctor(id, lastName, firstName, dob, phoneNumber, address, specialties));
                        }
                    }
                }
            }

            return doctors;
        }

        public static Doctor GetDoctorById(int dId)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from doctor where doctorID = @dId";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@dId", MySqlDbType.Int32);
                    comm.Parameters["@dId"].Value = dId;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("doctorID");
                        int lastNameOrdinal = reader.GetOrdinal("lastName");
                        int firstNameOrdinal = reader.GetOrdinal("firstName");
                        int dobOrdinal = reader.GetOrdinal("dob");
                        int phoneNumberOrdinal = reader.GetOrdinal("phoneNumber");
                        int addressOrdinal = reader.GetOrdinal("address");
                        int zipOrdinal = reader.GetOrdinal("zip");

                        while (reader.Read())
                        {
                            int id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : 0;
                            string lastName = !reader.IsDBNull(lastNameOrdinal)
                                ? reader.GetString(lastNameOrdinal)
                                : null;
                            string firstName = !reader.IsDBNull(firstNameOrdinal)
                                ? reader.GetString(firstNameOrdinal)
                                : null;
                            DateTime dob = !reader.IsDBNull(dobOrdinal)
                                ? reader.GetDateTime(dobOrdinal)
                                : default(DateTime);
                            string phoneNumber = !reader.IsDBNull(phoneNumberOrdinal)
                                ? reader.GetString(phoneNumberOrdinal)
                                : null;
                            string address1 = !reader.IsDBNull(addressOrdinal)
                                ? reader.GetString(addressOrdinal)
                                : null;
                            string zip = !reader.IsDBNull(zipOrdinal) ? reader.GetString(zipOrdinal) : null;
                            Address address = GetAddressAndZipNewConnection(id, address1, zip);
                            IList<DoctorSpecialty> specialties = GetSpecialtiesNewConnection(id);

                            return new Doctor(id, lastName, firstName, dob, phoneNumber, address, specialties);
                        }
                    }
                }
            }

            return null;
        }

        private static Address GetAddressAndZipNewConnection(int doctorId, string address1, string zip)
        {
            return AddressDAL.GetAddressWithDoctorId(doctorId, address1, zip);
        }

        private static IList<DoctorSpecialty> GetSpecialtiesNewConnection(int doctorId)
        {
            return DoctorSpecialtyDAL.GetSpecialtyWithDoctorId(doctorId);
        }
    }
}
