using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class TestResult
    {
        public int TestId { get; set; }

        public int PatientId { get; set; }

        public DateTime ResultDateTime { get; set; }

        public string Results { get; set; }

        public bool Abnormal { get; set; }

        public TestResult(int testId, int patientId, DateTime resultDateTime, string results, bool abnormal)
        {
            this.TestId = testId;
            this.PatientId = patientId;
            this.ResultDateTime = resultDateTime;
            this.Results = results;
            this.Abnormal = abnormal;
        }
    }
}
