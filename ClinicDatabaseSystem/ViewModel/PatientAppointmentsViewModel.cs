using ClinicDatabaseSystem.Annotations;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicDatabaseSystem.ViewModel
{
    /// <summary>
    /// View model handles model/data interactions for PatientAppointmentsPage.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class PatientAppointmentsViewModel : INotifyPropertyChanged
    {
        private List<AppointmentNameInfo> appointments;

        /// <summary>
        /// Gets or sets the appointments.
        /// </summary>
        /// <value>
        /// The appointments.
        /// </value>
        public List<AppointmentNameInfo> Appointments
        {
            get => this.appointments;
            set
            {
                this.appointments = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientAppointmentsViewModel"/> class.
        /// </summary>
        public PatientAppointmentsViewModel()
        {
            this.appointments = new List<AppointmentNameInfo>();
        }

        /// <summary>
        /// Loads the appointments.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        public void LoadAppointments(int patientId)
        {
            var patientAppointments = (List<Appointment>)AppointmentDAL.GetAllAppointmentsFromPatientId(patientId);
            patientAppointments.Sort((x, y) => x.ScheduledDate.CompareTo(y.ScheduledDate));
            var patients = PatientDAL.GetAllPatients();
            var doctors = DoctorDAL.GetAllDoctors();
            var appointmentNameInfoList = new List<AppointmentNameInfo>();
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
                appointmentNameInfoList.Add(appointmentNameInfo);
            }

            this.Appointments = appointmentNameInfoList;
        }

        /// <summary>
        /// Gets the appointment name information.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns>An appointment with name information</returns>
        public AppointmentNameInfo GetAppointmentNameInfo(Appointment appointment)
        {
            foreach (var appointmentNameInfo in this.appointments)
            {
                if (appointment.Equals(appointmentNameInfo.Appointment))
                {
                    return appointmentNameInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <returns>property changed event handler</returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
