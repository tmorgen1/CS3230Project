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
            var appointmentNameInfo = (AppointmentNameInfo) obj;
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
    }
}
