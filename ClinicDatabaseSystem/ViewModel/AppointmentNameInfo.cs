using ClinicDatabaseSystem.Model;

namespace ClinicDatabaseSystem.ViewModel
{
    /// <summary>
    /// Wrapper class to contain name information, along with appointments.
    /// Needed because id/number information is the only thing given in appointment.
    /// </summary>
    public class AppointmentNameInfo
    {
        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>
        /// The name of the patient.
        /// </value>
        public string PatientName { get; set; }

        /// <summary>
        /// Gets or sets the name of the doctor.
        /// </summary>
        /// <value>
        /// The name of the doctor.
        /// </value>
        public string DoctorName { get; set; }

        /// <summary>
        /// Gets or sets the appointment.
        /// </summary>
        /// <value>
        /// The appointment.
        /// </value>
        public Appointment Appointment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentNameInfo"/> class.
        /// </summary>
        /// <param name="patientName">Name of the patient.</param>
        /// <param name="doctorName">Name of the doctor.</param>
        /// <param name="appointment">The appointment.</param>
        public AppointmentNameInfo(string patientName, string doctorName, Appointment appointment)
        {
            this.PatientName = patientName;
            this.DoctorName = doctorName;
            this.Appointment = appointment;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(AppointmentNameInfo))
            {
                return false;
            }
            var appointmentNameInfo = (AppointmentNameInfo)obj;
            if (this.PatientName != appointmentNameInfo.PatientName)
            {
                return false;
            }

            if (this.DoctorName != appointmentNameInfo.DoctorName)
            {
                return false;
            }

            if (!this.Appointment.Equals(appointmentNameInfo.Appointment))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
