using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class Address
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public Address(string address1, string address2, string zip, string city, string state)
        {
            this.Address1 = address1;
            this.Address2 = address2;
            this.Zip = zip;
            this.City = city;
            this.State = state;
        }
    }
}
