using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.ViewModel
{
    public class AdminQueryResults
    {
        public IList<string> ColumnNames { get; private set; }

        public IList<object[]> QueryData { get; private set; }

        public AdminQueryResults(IList<string> columnNames, IList<object[]> queryData)
        {
            this.ColumnNames = columnNames;
            this.QueryData = queryData;
        }
    }
}
