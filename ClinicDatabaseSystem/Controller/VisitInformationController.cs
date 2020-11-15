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
        private bool createdVisitInfo;

        public bool CreatedVisitInfo
        {
            get => createdVisitInfo;
            set
            {
                if (createdVisitInfo == value) return;
                createdVisitInfo = value;
                OnPropertyChanged(nameof(CreatedVisitInfo));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
