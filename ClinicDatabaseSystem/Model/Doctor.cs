using System;
using System.Collections.Generic;

namespace ClinicDatabaseSystem.Model
{
    /// <summary>
    /// Holds information regarding Doctors.
    /// </summary>
    public class Doctor
    {
        /// <summary>
        /// Gets or sets the doctor identifier.
        /// </summary>
        /// <value>
        /// The doctor identifier.
        /// </value>
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the do b.
        /// </summary>
        /// <value>
        /// The do b.
        /// </value>
        public DateTime DoB { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the specialties.
        /// </summary>
        /// <value>
        /// The specialties.
        /// </value>
        public IList<DoctorSpecialty> Specialties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="address">The address.</param>
        /// <param name="specialties">The specialties.</param>
        public Doctor(int doctorId, string lastName, string firstName, DateTime dob, string phoneNumber, Address address, IList<DoctorSpecialty> specialties)
        {
            this.DoctorId = doctorId;
            this.LastName = lastName;
            this.FirstName = firstName;
            this.DoB = dob;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
            this.Specialties = specialties;
        }
    }
}
