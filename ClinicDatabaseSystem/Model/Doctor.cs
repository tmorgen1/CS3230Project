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

        public string AccountId { get; set; }

        public string Address { get; set; }

        public string Zip { get; set; }

        public Doctor(int doctorId, string lastName, string firstName, DateTime dob, string phoneNumber, string accountId, string address, string zip)
        {
            this.DoctorId = doctorId;
            this.LastName = lastName;
            this.FirstName = firstName;
            this.DoB = dob;
            this.PhoneNumber = phoneNumber;
            this.AccountId = accountId;
            this.Address = address;
            this.Zip = zip;
        }
    }
}
