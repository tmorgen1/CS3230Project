using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class Test
    {
        public int TestId { get; set; }

        public string Name { get; set; }

        public Test(int testId, string name)
        {
            this.TestId = testId;
            this.Name = name;
        }
    }
}
