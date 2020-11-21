using System;

namespace ClinicDatabaseSystem.Model
{
    /// <summary>
    /// Holds information regarding a test result.
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Gets or sets the test identifier.
        /// </summary>
        /// <value>
        /// The test identifier.
        /// </value>
        public int TestId { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>
        /// The patient identifier.
        /// </value>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the result date time.
        /// </summary>
        /// <value>
        /// The result date time.
        /// </value>
        public DateTime VisitDateTime { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public string Results { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TestResult"/> is abnormal.
        /// </summary>
        /// <value>
        ///   <c>true</c> if abnormal; otherwise, <c>false</c>.
        /// </value>
        public bool Abnormal { get; set; }

        /// <summary>
        /// Gets or sets the test result date time.
        /// </summary>
        /// <value>
        /// The test result date time.
        /// </value>
        public DateTime TestResultDateTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestResult"/> class.
        /// </summary>
        /// <param name="testId">The test identifier.</param>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="visitDateTime">The result date time.</param>
        /// <param name="results">The results.</param>
        /// <param name="abnormal">if set to <c>true</c> [abnormal].</param>
        public TestResult(int testId, int patientId, DateTime visitDateTime, string results, bool abnormal, DateTime testResultDateTime)
        {
            this.TestId = testId;
            this.PatientId = patientId;
            this.VisitDateTime = visitDateTime;
            this.Results = results;
            this.Abnormal = abnormal;
            this.TestResultDateTime = testResultDateTime;
        }
    }
}
