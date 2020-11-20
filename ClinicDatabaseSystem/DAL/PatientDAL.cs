using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for patients in the database.
    /// </summary>
    public static class PatientDAL
    {
        /// <summary>
        /// Inserts the patient into the database.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="address">The address.</param>
        /// <param name="zip">The zip.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA patient is inserted into table.</returns>
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

                    // look into sql transactions
                    try
                    {
                        comm.Transaction = conn.BeginTransaction();
                        AddressDAL.InsertAddressUsingCommand(address, comm);
                        comm.CommandText = insertStatement;
                        comm.ExecuteNonQuery();
                        comm.Transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            comm.Transaction.Rollback();
                            throw ex;
                        }
                        catch (Exception ex2)
                        {
                            throw ex2;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Edits the patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA patient is edited in the table.</returns>
        public static bool EditPatient(Patient patient)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string updateStatement =
                    "update patient set lastName = @lastName, firstName = @firstName, dob = @dob, phoneNumber = @phoneNumber where patientId = @patientId";

                using (MySqlCommand comm = new MySqlCommand(updateStatement, conn))
                {
                    comm.Parameters.Add("@lastName", MySqlDbType.VarChar);
                    comm.Parameters["@lastName"].Value = patient.LastName;
                    comm.Parameters.Add("@firstName", MySqlDbType.VarChar);
                    comm.Parameters["@firstName"].Value = patient.FirstName;
                    comm.Parameters.Add("@dob", MySqlDbType.Date);
                    comm.Parameters["@dob"].Value = patient.Dob;
                    comm.Parameters.Add("@phoneNumber", MySqlDbType.VarChar);
                    comm.Parameters["@phoneNumber"].Value = patient.PhoneNumber;
                    comm.Parameters.Add("@patientId", MySqlDbType.Int32);
                    comm.Parameters["@patientId"].Value = patient.PatientId;

                    try
                    {
                        comm.Transaction = conn.BeginTransaction();
                        AddressDAL.EditAddressUsingCommand(patient.Address, patient.PatientId, comm);
                        comm.CommandText = updateStatement;
                        comm.ExecuteNonQuery();
                        comm.Transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            comm.Transaction.Rollback();
                            throw ex;
                        }
                        catch (Exception ex2)
                        {
                            throw ex2;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all patients from the database.
        /// </summary>
        /// <returns>Collection of all the patients</returns>
        public static IList<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                //Stored procedure by Michael
                string spName = "patient_getAllPatients";

                using (MySqlCommand comm = new MySqlCommand(spName, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
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

        /// <summary>
        /// Gets the patient by identifier from the database.
        /// </summary>
        /// <param name="pId">The patient identifier.</param>
        /// <returns>The patient object</returns>
        public static Patient GetPatientById(int pId)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from patient where patientID = @pId";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@pId", MySqlDbType.Int32);
                    comm.Parameters["@pId"].Value = pId;

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

                            return new Patient(id, lastName, firstName, dob, phoneNumber, address);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Searches for patients in the database.
        /// </summary>
        /// <param name="dob">The date of birth.</param>
        /// <returns>Collection of patients born on dob</returns>
        public static IList<Patient> SearchForPatients(DateTime dob)
        {
            List<Patient> patients = new List<Patient>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from patient where dob = @dob";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@dob", MySqlDbType.Date);
                    comm.Parameters["@dob"].Value = dob;

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
                            DateTime localDob = !reader.IsDBNull(dobOrdinal)
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

                            patients.Add(new Patient(id, lastName, firstName, localDob, phoneNumber, address));
                        }
                    }
                }
            }

            return patients;
        }

        /// <summary>
        /// Searches for patients in database.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>Collection of patients with the full name given</returns>
        public static IList<Patient> SearchForPatients(string firstName, string lastName)
        {
            List<Patient> patients = new List<Patient>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                // Thomas's Stored Procedure
                string storedProcedureName = "patient_getPatientByFullName";

                using (MySqlCommand comm = new MySqlCommand(storedProcedureName, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add("firstNameInput", MySqlDbType.VarChar);
                    comm.Parameters["firstNameInput"].Value = firstName;
                    comm.Parameters["firstNameInput"].Direction = ParameterDirection.Input;
                    comm.Parameters.Add("lastNameInput", MySqlDbType.VarChar);
                    comm.Parameters["lastNameInput"].Value = lastName;
                    comm.Parameters["lastNameInput"].Direction = ParameterDirection.Input;

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
                            string localLastName = !reader.IsDBNull(lastNameOrdinal)
                                ? reader.GetString(lastNameOrdinal)
                                : null;
                            string localFirstName = !reader.IsDBNull(firstNameOrdinal)
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

                            patients.Add(new Patient(id, localLastName, localFirstName, dob, phoneNumber, address));
                        }
                    }
                }
            }

            return patients;
        }

        /// <summary>
        /// Searches for patients in the database.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dob">The date of birth.</param>
        /// <returns>Collection of patients with full name given and dob on given date</returns>
        public static IList<Patient> SearchForPatients(string firstName, string lastName, DateTime dob)
        {
            List<Patient> patients = new List<Patient>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from patient where firstName = @firstName and lastName = @lastName and dob = @dob";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@firstName", MySqlDbType.VarChar);
                    comm.Parameters["@firstName"].Value = firstName;
                    comm.Parameters.Add("@lastName", MySqlDbType.VarChar);
                    comm.Parameters["@lastName"].Value = lastName;
                    comm.Parameters.Add("@dob", MySqlDbType.Date);
                    comm.Parameters["@dob"].Value = dob;

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
                            string localLastName = !reader.IsDBNull(lastNameOrdinal)
                                ? reader.GetString(lastNameOrdinal)
                                : null;
                            string localFirstName = !reader.IsDBNull(firstNameOrdinal)
                                ? reader.GetString(firstNameOrdinal)
                                : null;
                            DateTime localDob = !reader.IsDBNull(dobOrdinal)
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

                            patients.Add(new Patient(id, lastName, firstName, localDob, phoneNumber, address));
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
