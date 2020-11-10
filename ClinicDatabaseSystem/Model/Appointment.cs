using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicDatabaseSystem.Model
{
    public class Appointment
    {
        public int PatientId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public int DoctorId { get; set; }

        public string Reason { get; set; }

        public Appointment(int patientId, DateTime scheduledDate, int doctorId, string reason)
        {
            this.PatientId = patientId;
            this.ScheduledDate = scheduledDate;
            this.DoctorId = doctorId;
            this.Reason = reason;
        }

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

            var appointment = (Appointment) obj;
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
    }
}
