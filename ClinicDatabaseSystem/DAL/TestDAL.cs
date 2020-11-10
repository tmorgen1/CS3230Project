using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class TestDAL
    {
        public static IList<Test> GetTestTypes()
        {
            List<Test> testTypes = new List<Test>();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "select * from test";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        int tIdOrdinal = reader.GetOrdinal("testID");
                        int tNameOrdinal = reader.GetOrdinal("name");

                        while (reader.Read())
                        {
                            int testId = !reader.IsDBNull(tIdOrdinal) ? reader.GetInt32(tIdOrdinal) : 0;
                            string tName = !reader.IsDBNull(tNameOrdinal) ? reader.GetString(tNameOrdinal) : null;

                            testTypes.Add(new Test(testId, tName));
                        }
                    }
                }
            }

            return testTypes;
        }
    }
}
