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
    }
}
