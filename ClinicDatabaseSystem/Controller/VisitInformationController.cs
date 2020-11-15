using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Annotations;

namespace ClinicDatabaseSystem.Controller
{
    public class VisitInformationController : INotifyPropertyChanged
    {
        private bool createdVisitInformation;
        public bool CreatedVisitInformation
        {
            get => this.createdVisitInformation;
            set
            {
                this.createdVisitInformation = value;
                this.OnPropertyChanged();
            }
        }

        public VisitInformationController()
        {
            this.createdVisitInformation = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
