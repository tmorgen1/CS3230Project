namespace ClinicDatabaseSystem.Controller
{
    /// <summary>
    /// Handles the currently selected patient between pages.
    /// </summary>
    public static class PatientController
    {
        /// <summary>
        /// Gets or sets the current patient.
        /// </summary>
        /// <value>
        /// The current patient.
        /// </value>
        public static int CurrentPatient { get; set; }
    }
}
