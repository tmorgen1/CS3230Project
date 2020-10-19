using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class Patient
    {
        public int PatientId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public DateTime Dob { get; set; }

        public string PhoneNumber { get; set; }

        public Address Address { get; set; }

        public Patient(int patientId, string lastName, string firstName, DateTime dob, string phoneNumber, Address address)
        {
            this.PatientId = patientId;
            this.LastName = lastName;
            this.FirstName = firstName;
            this.Dob = dob;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
        }
    }
}
