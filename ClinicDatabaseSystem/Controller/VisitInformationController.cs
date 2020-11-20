using System.ComponentModel;

namespace ClinicDatabaseSystem.Controller
{
    /// <summary>
    /// Handles the currently selected VisitInformation between pages.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class VisitInformationController : INotifyPropertyChanged
    {
        private bool createdVisitInfo;

        /// <summary>
        /// Gets or sets a value indicating whether [created visit information].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [created visit information]; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
