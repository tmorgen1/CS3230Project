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

        public override string ToString()
        {
            //probably replace with a real formatter, yoder wouldnt be happy
            return "PatientID: " + this.PatientId + ", Last Name: " + this.LastName + ", First Name: " + this.FirstName +
                   ", Birthdate: " + this.Dob + ", Phone Number: " + this.PhoneNumber + ", Address: " + this.Address.Address1 + 
                   ", Zip: " + this.Address.Zip + ", City: " + this.Address.City + ", State: " + this.Address.State + ", Address2: " + 
                   this.Address.Address2;
        }
    }
}
