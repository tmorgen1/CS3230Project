using System;

namespace ClinicDatabaseSystem.Model
{
    /// <summary>
    /// Holds information regarding patient visits.
    /// </summary>
    public class VisitInformation
    {
        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>
        /// The patient identifier.
        /// </value>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the nurse identifier.
        /// </summary>
        /// <value>
        /// The nurse identifier.
        /// </value>
        public int NurseId { get; set; }

        /// <summary>
        /// Gets or sets the visit date time.
        /// </summary>
        /// <value>
        /// The visit date time.
        /// </value>
        public DateTime VisitDateTime { get; set; }

        /// <summary>
        /// Gets or sets the systolic bp.
        /// </summary>
        /// <value>
        /// The systolic bp.
        /// </value>
        public string SystolicBp { get; set; }

        /// <summary>
        /// Gets or sets the diastolic bp.
        /// </summary>
        /// <value>
        /// The diastolic bp.
        /// </value>
        public string DiastolicBp { get; set; }

        /// <summary>
        /// Gets or sets the body temporary.
        /// </summary>
        /// <value>
        /// The body temporary.
        /// </value>
        public string BodyTemp { get; set; }

        /// <summary>
        /// Gets or sets the pulse.
        /// </summary>
        /// <value>
        /// The pulse.
        /// </value>
        public string Pulse { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public string Weight { get; set; }

        /// <summary>
        /// Gets or sets the symptoms.
        /// </summary>
        /// <value>
        /// The symptoms.
        /// </value>
        public string Symptoms { get; set; }

        /// <summary>
        /// Gets or sets the initial diagnosis.
        /// </summary>
        /// <value>
        /// The initial diagnosis.
        /// </value>
        public string InitialDiagnosis { get; set; }

        /// <summary>
        /// Gets or sets the final diagnosis.
        /// </summary>
        /// <value>
        /// The final diagnosis.
        /// </value>
        public string FinalDiagnosis { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitInformation"/> class.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="nurseId">The nurse identifier.</param>
        /// <param name="visitDateTime">The visit date time.</param>
        /// <param name="systolicBp">The systolic bp.</param>
        /// <param name="diastolicBp">The diastolic bp.</param>
        /// <param name="bodyTemp">The body temporary.</param>
        /// <param name="pulse">The pulse.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="symptoms">The symptoms.</param>
        /// <param name="initialDiagnosis">The initial diagnosis.</param>
        /// <param name="finalDiagnosis">The final diagnosis.</param>
        public VisitInformation(int patientId, int nurseId, DateTime visitDateTime, string systolicBp, string diastolicBp, string bodyTemp, string pulse, string weight, string symptoms, string initialDiagnosis, string finalDiagnosis)
        {
            this.PatientId = patientId;
            this.NurseId = nurseId;
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
