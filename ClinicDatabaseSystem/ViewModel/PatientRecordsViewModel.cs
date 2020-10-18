using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;

namespace ClinicDatabaseSystem.ViewModel
{
    public class PatientRecordsViewModel
    {
        public List<Patient> Patients { get; private set; }

        public PatientRecordsViewModel()
        {
            this.Patients = new List<Patient>();
            this.LoadPatients();
        }

        public void LoadPatients()
        {
            this.Patients.Clear();
            var patients = PatientDAL.GetPatients();
            foreach (var patient in patients)
            {
                this.Patients?.Add(patient);
            }
        }
    }
}
