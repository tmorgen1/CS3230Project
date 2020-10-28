using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class DoctorSpecialty
    {
        public int DoctorId { get; set; }

        public string Specialty { get; set; }

        public DoctorSpecialty(int doctorId, string specialty)
        {
            this.DoctorId = doctorId;
            this.Specialty = specialty;
        }
    }
}
