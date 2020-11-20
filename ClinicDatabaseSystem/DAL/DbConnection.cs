using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    /// <summary>
    /// Handles creating connection to this programs specific database.
    /// </summary>
    public static class DbConnection
    {
        private static readonly string connString = "Server=160.10.25.16;Port=3306;Database=cs3230f20h;Uid=cs3230f20h;Pwd=Fi8q2ZyuVWUwwUMz;";

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>Returns MySqlConnection object with proper connection settings.  Configured to our database.</returns>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connString);
        }
    }
}
