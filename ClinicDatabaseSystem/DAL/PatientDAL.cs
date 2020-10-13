using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public class PatientDAL
    {
        public static bool InsertPatient(string lastName, string firstName, DateTime dob, string phoneNumber,
            Address address, string zip)
        {

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string insertStatement = "insert into patient(lastName, firstName, dob, phoneNumber, address, zip) values (@lastName, @firstName, @dob, @phoneNumber, @address, @zip)";

                using (MySqlCommand comm = new MySqlCommand(insertStatement, conn))
                {
                    comm.Parameters.Add("@lastName", MySqlDbType.VarChar);
                    comm.Parameters["@lastName"].Value = lastName;
                    comm.Parameters.Add("@firstName", MySqlDbType.VarChar);
                    comm.Parameters["@firstName"].Value = firstName;
                    comm.Parameters.Add("@dob", MySqlDbType.Date);
                    comm.Parameters["@dob"].Value = dob;
                    comm.Parameters.Add("@phoneNumber", MySqlDbType.VarChar);
                    comm.Parameters["@phoneNumber"].Value = phoneNumber;
                    comm.Parameters.Add("@address", MySqlDbType.VarChar);
                    comm.Parameters["@address"].Value = address.Address1;
                    comm.Parameters.Add("@zip", MySqlDbType.VarChar);
                    comm.Parameters["@zip"].Value = zip;

                    bool addressAdded = AddressDAL.InsertAddress(address);
                    return comm.ExecuteNonQuery() > 0 && addressAdded;
                }
            }
        }

        public static IList<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from patient";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("patientID");
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

                            patients.Add(new Patient(id, lastName, firstName, dob, phoneNumber, address));
                        }
                    }
                }
            }

            return patients;
        }

        private static Address GetAddressAndZipNewConnection(int patientId, string address1, string zip)
        {
            return AddressDAL.GetAddressWithPatientId(patientId, address1, zip);
        }
    }
}
