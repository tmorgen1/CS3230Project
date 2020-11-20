using ClinicDatabaseSystem.Annotations;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicDatabaseSystem.ViewModel
{
    /// <summary>
    /// Viewmodel handles model/data interactions for PatientRecordsPage.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class PatientRecordsViewModel : INotifyPropertyChanged
    {
        private List<Patient> patients;

        /// <summary>
        /// Gets or sets the patients.
        /// </summary>
        /// <value>
        /// The patients.
        /// </value>
        public List<Patient> Patients
        {
            get => this.patients;
            set
            {
                if (this.Patients != value)
                {
                    this.patients = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientRecordsViewModel"/> class.
        /// </summary>
        public PatientRecordsViewModel()
        {
            this.patients = new List<Patient>();
            this.LoadPatients();
        }

        /// <summary>
        /// Loads the patients.
        /// </summary>
        public void LoadPatients()
        {
            this.Patients = (List<Patient>)PatientDAL.GetAllPatients();
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
