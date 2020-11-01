using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Model;

namespace ClinicDatabaseSystem.ViewModel
{
    public class AppointmentNameInfo
    {
        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public Appointment Appointment { get; set; }

        public AppointmentNameInfo(string patientName, string doctorName, Appointment appointment)
        {
            this.PatientName = patientName;
            this.DoctorName = doctorName;
            this.Appointment = appointment;
        }
    }
}
