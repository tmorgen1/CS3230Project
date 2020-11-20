using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Data Access Layer for addresses held in the database.
    /// </summary>
    public static class AddressDAL
    {
        /// <summary>
        /// Inserts the address in the database.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA address inserted into table.</returns>
        public static bool InsertAddress(Address address)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string insertStatement = "insert into address values (@address1, @zip, @city, @state, @address2)";

                using (MySqlCommand comm = new MySqlCommand(insertStatement, conn))
                {
                    comm.Parameters.Add("@address1", MySqlDbType.VarChar);
                    comm.Parameters["@address1"].Value = address.Address1;
                    comm.Parameters.Add("@zip", MySqlDbType.VarChar);
                    comm.Parameters["@zip"].Value = address.Zip;
                    comm.Parameters.Add("@city", MySqlDbType.VarChar);
                    comm.Parameters["@city"].Value = address.City;
                    comm.Parameters.Add("@state", MySqlDbType.VarChar);
                    comm.Parameters["@state"].Value = address.State;
                    comm.Parameters.Add("@address2", MySqlDbType.VarChar);
                    comm.Parameters["@address2"].Value = address.Address2;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Inserts the address using an already established connection/command.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="command">The command.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA address inserted into table.</returns>
        public static bool InsertAddressUsingCommand(Address address, MySqlCommand command)
        {
            string insertStatement = "insert into address values (@2address1, @2zip, @2city, @2state, @2address2)";

            using (MySqlCommand comm = command)
            {
                comm.CommandText = insertStatement;

                comm.Parameters.Add("@2address1", MySqlDbType.VarChar);
                comm.Parameters["@2address1"].Value = address.Address1;
                comm.Parameters.Add("@2zip", MySqlDbType.VarChar);
                comm.Parameters["@2zip"].Value = address.Zip;
                comm.Parameters.Add("@2city", MySqlDbType.VarChar);
                comm.Parameters["@2city"].Value = address.City;
                comm.Parameters.Add("@2state", MySqlDbType.VarChar);
                comm.Parameters["@2state"].Value = address.State;
                comm.Parameters.Add("@2address2", MySqlDbType.VarChar);
                comm.Parameters["@2address2"].Value = address.Address2;

                return comm.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Edits the address in the database.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA address was edited in the table.</returns>
        public static bool EditAddress(Address address, int patientId)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string updateStatement = "update address A, patient P set A.address1 = @address1, A.zip = @zip, A.city = @city, A.state = @state, A.address2 = @address2 where P.address = A.address1 and P.zip = A.zip and P.patientId = @patientId";

                using (MySqlCommand comm = new MySqlCommand(updateStatement, conn))
                {
                    comm.Parameters.Add("@address1", MySqlDbType.VarChar);
                    comm.Parameters["@address1"].Value = address.Address1;
                    comm.Parameters.Add("@zip", MySqlDbType.VarChar);
                    comm.Parameters["@zip"].Value = address.Zip;
                    comm.Parameters.Add("@city", MySqlDbType.VarChar);
                    comm.Parameters["@city"].Value = address.City;
                    comm.Parameters.Add("@state", MySqlDbType.VarChar);
                    comm.Parameters["@state"].Value = address.State;
                    comm.Parameters.Add("@address2", MySqlDbType.VarChar);
                    comm.Parameters["@address2"].Value = address.Address2;
                    comm.Parameters.Add("@patientId", MySqlDbType.Int32);
                    comm.Parameters["@patientId"].Value = patientId;

                    return comm.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Edits the address using an already established connection/command.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="command">The command.</param>
        /// <returns>True if the number of rows affected is greater than 0. AKA address was edited in the table.</returns>
        public static bool EditAddressUsingCommand(Address address, int patientId, MySqlCommand command)
        {
            string updateStatement = "update address A, patient P set A.address1 = @2address1, A.zip = @2zip, A.city = @2city, A.state = @2state, A.address2 = @2address2 where P.address = A.address1 and P.zip = A.zip and P.patientId = @2patientId";

            using (MySqlCommand comm = command)
            {
                comm.CommandText = updateStatement;

                comm.Parameters.Add("@2address1", MySqlDbType.VarChar);
                comm.Parameters["@2address1"].Value = address.Address1;
                comm.Parameters.Add("@2zip", MySqlDbType.VarChar);
                comm.Parameters["@2zip"].Value = address.Zip;
                comm.Parameters.Add("@2city", MySqlDbType.VarChar);
                comm.Parameters["@2city"].Value = address.City;
                comm.Parameters.Add("@2state", MySqlDbType.VarChar);
                comm.Parameters["@2state"].Value = address.State;
                comm.Parameters.Add("@2address2", MySqlDbType.VarChar);
                comm.Parameters["@2address2"].Value = address.Address2;
                comm.Parameters.Add("@2patientId", MySqlDbType.Int32);
                comm.Parameters["@2patientId"].Value = patientId;

                return comm.ExecuteNonQuery() > 0;
            }

        }

        /// <summary>
        /// Gets the address with patient identifier, address, and zip.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="zip">The zip.</param>
        /// <returns>Address given patientId, address1, and zip</returns>
        public static Address GetAddressWithPatientId(int patientId, string address1, string zip)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select A.address2, A.city, A.state from patient P, address A where P.patientID = @id and A.address1 = @address and A.zip = @zip";
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32);
                    comm.Parameters["@id"].Value = patientId;
                    comm.Parameters.Add("@address", MySqlDbType.VarChar);
                    comm.Parameters["@address"].Value = address1;
                    comm.Parameters.Add("@zip", MySqlDbType.VarChar);
                    comm.Parameters["@zip"].Value = zip;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int address2Ordinal = reader.GetOrdinal("address2");
                        int cityOrdinal = reader.GetOrdinal("city");
                        int stateOrdinal = reader.GetOrdinal("state");

                        while (reader.Read())
                        {
                            string address2 = !reader.IsDBNull(address2Ordinal) ? reader.GetString(address2Ordinal) : null;
                            string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                            string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;

                            return new Address(address1, address2, zip, city, state);
                        }
                    }
                }

                // look at this
                return null;
            }

        }

        /// <summary>
        /// Gets the address with doctor identifier.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="zip">The zip.</param>
        /// <returns>Address given doctorId, address1, and zip</returns>
        public static Address GetAddressWithDoctorId(int doctorId, string address1, string zip)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select A.address2, A.city, A.state from doctor D, address A where D.doctorID = @id and A.address1 = @address and A.zip = @zip";
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32);
                    comm.Parameters["@id"].Value = doctorId;
                    comm.Parameters.Add("@address", MySqlDbType.VarChar);
                    comm.Parameters["@address"].Value = address1;
                    comm.Parameters.Add("@zip", MySqlDbType.VarChar);
                    comm.Parameters["@zip"].Value = zip;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int address2Ordinal = reader.GetOrdinal("address2");
                        int cityOrdinal = reader.GetOrdinal("city");
                        int stateOrdinal = reader.GetOrdinal("state");

                        while (reader.Read())
                        {
                            string address2 = !reader.IsDBNull(address2Ordinal) ? reader.GetString(address2Ordinal) : null;
                            string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                            string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;

                            return new Address(address1, address2, zip, city, state);
                        }
                    }
                }

                // look at this
                return null;
            }

        }

        /// <summary>
        /// Gets the address with nurse identifier, address, and zip.
        /// </summary>
        /// <param name="nurseId">The nurse identifier.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="zip">The zip.</param>
        /// <returns>Address given nurseId, address1, and zip</returns>
        public static Address GetAddressWithNurseId(int nurseId, string address1, string zip)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select A.address2, A.city, A.state from nurse N, address A where N.nurseId = @id and A.address1 = @address and A.zip = @zip";
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32);
                    comm.Parameters["@id"].Value = nurseId;
                    comm.Parameters.Add("@address", MySqlDbType.VarChar);
                    comm.Parameters["@address"].Value = address1;
                    comm.Parameters.Add("@zip", MySqlDbType.VarChar);
                    comm.Parameters["@zip"].Value = zip;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int address2Ordinal = reader.GetOrdinal("address2");
                        int cityOrdinal = reader.GetOrdinal("city");
                        int stateOrdinal = reader.GetOrdinal("state");

                        while (reader.Read())
                        {
                            string address2 = !reader.IsDBNull(address2Ordinal) ? reader.GetString(address2Ordinal) : null;
                            string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                            string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;

                            return new Address(address1, address2, zip, city, state);
                        }
                    }
                }

                // look at this
                return null;
            }
        }

        /// <summary>
        /// Gets the address with admin identifier.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="zip">The zip.</param>
        /// <returns>Address given adminId, address1, and zip</returns>
        public static Address GetAddressWithAdminId(int adminId, string address1, string zip)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query =
                    "select A.address2, A.city, A.state from administrator AD, address A where AD.adminId = @id and A.address1 = @address and A.zip = @zip";
                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.Add("@id", MySqlDbType.Int32);
                    comm.Parameters["@id"].Value = adminId;
                    comm.Parameters.Add("@address", MySqlDbType.VarChar);
                    comm.Parameters["@address"].Value = address1;
                    comm.Parameters.Add("@zip", MySqlDbType.VarChar);
                    comm.Parameters["@zip"].Value = zip;

                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int address2Ordinal = reader.GetOrdinal("address2");
                        int cityOrdinal = reader.GetOrdinal("city");
                        int stateOrdinal = reader.GetOrdinal("state");

                        while (reader.Read())
                        {
                            string address2 = !reader.IsDBNull(address2Ordinal) ? reader.GetString(address2Ordinal) : null;
                            string city = !reader.IsDBNull(cityOrdinal) ? reader.GetString(cityOrdinal) : null;
                            string state = !reader.IsDBNull(stateOrdinal) ? reader.GetString(stateOrdinal) : null;

                            return new Address(address1, address2, zip, city, state);
                        }
                    }
                }

                // look at this
                return null;
            }
        }
    }
}
