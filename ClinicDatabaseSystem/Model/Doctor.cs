using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public DateTime DoB { get; set; }

        public string PhoneNumber { get; set; }

        public Address Address { get; set; }

        public IList<DoctorSpecialty> Specialties { get; set; }

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
