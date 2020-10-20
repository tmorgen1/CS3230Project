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
    public class PatientRecordsViewModel : INotifyPropertyChanged
    {
        private List<Patient> patients;

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

        public PatientRecordsViewModel()
        {
            this.patients = new List<Patient>();
            this.LoadPatients();
        }

        public void LoadPatients()
        {
            this.Patients = (List<Patient>)PatientDAL.GetAllPatients();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
