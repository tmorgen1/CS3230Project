using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClinicDatabaseSystem.Annotations;

namespace ClinicDatabaseSystem.ViewModel
{
    public class AdminQueryViewModel :INotifyPropertyChanged
    {
        private AdminQueryResults results;

        public AdminQueryResults Results
        {
            get => this.results;
            set
            {
                if (this.results != value)
                {
                    this.results = value;
                    this.OnPropertyChanged();
                }
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
