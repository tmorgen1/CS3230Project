using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class VisitInformation
    {
        public int PatientId { get; set; }

        public DateTime VisitDateTime { get; set; }

        public string SystolicBp { get; set; }

        public string DiastolicBp { get; set; }

        public string BodyTemp { get; set; }

        public string Pulse { get; set; }

        public string Weight { get; set; }

        public string Symptoms { get; set; }

        public string InitialDiagnosis { get; set; }

        public string FinalDiagnosis { get; set; }

        public VisitInformation(int patientId, DateTime visitDateTime, string systolicBp, string diastolicBp, string bodyTemp, string pulse, string weight, string symptoms, string initialDiagnosis, string finalDiagnosis)
        {
            this.PatientId = patientId;
            this.VisitDateTime = visitDateTime;
            this.SystolicBp = systolicBp;
            this.DiastolicBp = diastolicBp;
            this.BodyTemp = bodyTemp;
            this.Pulse = pulse;
            this.Weight = weight;
            this.Symptoms = symptoms;
            this.InitialDiagnosis = initialDiagnosis;
            this.FinalDiagnosis = finalDiagnosis;
        }
    }
}
