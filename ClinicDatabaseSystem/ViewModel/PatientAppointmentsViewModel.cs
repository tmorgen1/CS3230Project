using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Annotations;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;

namespace ClinicDatabaseSystem.ViewModel
{
    public class PatientAppointmentsViewModel : INotifyPropertyChanged
    {
        private List<AppointmentNameInfo> appointments;

        public List<AppointmentNameInfo> Appointments
        {
            get => this.appointments;
            set
            {
                if (this.appointments != value)
                {
                    this.appointments = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public PatientAppointmentsViewModel()
        {
            this.appointments = new List<AppointmentNameInfo>();
        }

        public void LoadAppointments(int patientId)
        {
            var patientAppointments = (List<Appointment>)AppointmentDAL.GetAllAppointmentsFromPatientId(patientId);
            patientAppointments.Sort((x,y) => x.ScheduledDate.CompareTo(y.ScheduledDate));
            var patients = PatientDAL.GetAllPatients();
            var doctors = DoctorDAL.GetAllDoctors();
            foreach (var patientAppointment in patientAppointments)
            {
                var patientName = "";
                var doctorName = "";
                foreach (var patient in patients)
                {
                    if (patient.PatientId == patientAppointment.PatientId)
                    {
                        patientName = patient.FirstName + " " + patient.LastName;
                    }
                }

                foreach (var doctor in doctors)
                {
                    if (doctor.DoctorId == patientAppointment.DoctorId)
                    {
                        doctorName = doctor.FirstName + " " + doctor.LastName;
                    }
                }

                var appointmentNameInfo = new AppointmentNameInfo(patientName, doctorName, patientAppointment);
                this.appointments.Add(appointmentNameInfo);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
