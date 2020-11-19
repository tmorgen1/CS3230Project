using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.ViewModel;
using MySql.Data.MySqlClient;

namespace ClinicDatabaseSystem.DAL
{
    public static class AdministrationDAL
    {
        private const int ORDINAL_INDEX = 1;
        private const int COLUMN_NAME_INDEX = 0;

        public static AdminQueryResults AdminQuery(string query)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        DataTable table = reader.GetSchemaTable();
                        var ordinals = new List<int>();
                        var columnNames = new List<string>();
                        if (table != null)
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                ordinals.Add((int) row[ORDINAL_INDEX]);
                                columnNames.Add((string) row[COLUMN_NAME_INDEX]);
                            }
                        }

                        var queryData = new List<object[]>();
                        for (var i = 0; i < ordinals.Count && reader.Read(); i++)
                        {
                            queryData.Add(new object[columnNames.Count]);
                            for (var j = 0; j < columnNames.Count; j++)
                            {
                                queryData[i][j] = reader.GetValue(j);
                            }
                        }
                        var adminQueryResults = new AdminQueryResults(columnNames, queryData);
                        return adminQueryResults;
                    }
                }
            }
        }
    }
}
