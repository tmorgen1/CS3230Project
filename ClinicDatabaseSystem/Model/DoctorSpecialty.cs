namespace ClinicDatabaseSystem.Model
{
    /// <summary>
    /// Holds a doctor's specialty title and doctor.
    /// </summary>
    public class DoctorSpecialty
    {
        /// <summary>
        /// Gets or sets the doctor identifier.
        /// </summary>
        /// <value>
        /// The doctor identifier.
        /// </value>
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the specialty.
        /// </summary>
        /// <value>
        /// The specialty.
        /// </value>
        public string Specialty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorSpecialty"/> class.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="specialty">The specialty.</param>
        public DoctorSpecialty(int doctorId, string specialty)
        {
            this.DoctorId = doctorId;
            this.Specialty = specialty;
        }
    }
}
