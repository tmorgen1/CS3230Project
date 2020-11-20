using System;

namespace ClinicDatabaseSystem.Model
{
    /// <summary>
    /// Holds information regarding appointments.
    /// </summary>
    public class Appointment
    {
        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>
        /// The patient identifier.
        /// </value>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the scheduled date.
        /// </summary>
        /// <value>
        /// The scheduled date.
        /// </value>
        public DateTime ScheduledDate { get; set; }

        /// <summary>
        /// Gets or sets the doctor identifier.
        /// </summary>
        /// <value>
        /// The doctor identifier.
        /// </value>
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment"/> class.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="scheduledDate">The scheduled date.</param>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="reason">The reason.</param>
        public Appointment(int patientId, DateTime scheduledDate, int doctorId, string reason)
        {
            this.PatientId = patientId;
            this.ScheduledDate = scheduledDate;
            this.DoctorId = doctorId;
            this.Reason = reason;
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
            if (obj.GetType() != typeof(Appointment))
            {
                return false;
            }

            var appointment = (Appointment)obj;
            if (this.PatientId != appointment.PatientId)
            {
                return false;
            }
            if (this.DoctorId != appointment.DoctorId)
            {
                return false;
            }
            if (!this.Reason.Equals(appointment.Reason))
            {
                return false;
            }
            if (this.ScheduledDate != appointment.ScheduledDate)
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
