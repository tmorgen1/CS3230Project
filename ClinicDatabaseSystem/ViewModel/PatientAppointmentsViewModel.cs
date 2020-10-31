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
        private List<Appointment> appointments;

        public List<Appointment> Appointments
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
            this.appointments = new List<Appointment>();
        }

        public void LoadAppointments(int patientId)
        {
            var patientAppointments = (List<Appointment>)AppointmentDAL.GetAllAppointmentsFromPatientId(patientId);
            patientAppointments.Sort((x,y) => x.ScheduledDate.CompareTo(y.ScheduledDate));
            this.Appointments = patientAppointments;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
