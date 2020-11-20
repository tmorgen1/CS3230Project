using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for doctors held in the database.
    /// </summary>
    public static class DoctorDAL
    {
        /// <summary>
        /// Gets all doctors from database.
        /// </summary>
        /// <returns>List of all doctors in the database</returns>
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

        /// <summary>
        /// Gets the doctor by doctor id from the database.
        /// </summary>
        /// <param name="dId">The doctor identifier.</param>
        /// <returns>Doctor with specific doctor id</returns>
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

        /// <summary>
        /// Gets the address and zip using a new connection object.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="zip">The zip.</param>
        /// <returns>Address of the doctor</returns>
        private static Address GetAddressAndZipNewConnection(int doctorId, string address1, string zip)
        {
            return AddressDAL.GetAddressWithDoctorId(doctorId, address1, zip);
        }

        /// <summary>
        /// Gets the specialties using a new connection object.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <returns>List of doctor specialties</returns>
        private static IList<DoctorSpecialty> GetSpecialtiesNewConnection(int doctorId)
        {
            return DoctorSpecialtyDAL.GetSpecialtyWithDoctorId(doctorId);
        }
    }
}
