using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles confirming final diagnosis, leading to a read only visit info.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class ConfirmFinalDiagnosisContentDialog : ContentDialog
    {
        private readonly VisitInformation visitInformation;
        private readonly AppointmentNameInfo appointmentNameInfo;
        private readonly string finalDiagnosis;
        private VisitInformationController visitInformationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmFinalDiagnosisContentDialog"/> class.
        /// </summary>
        /// <param name="visitInformation">The visit information.</param>
        /// <param name="appointmentNameInfo">The appointment name information.</param>
        /// <param name="finalDiagnosis">The final diagnosis.</param>
        public ConfirmFinalDiagnosisContentDialog(VisitInformation visitInformation, AppointmentNameInfo appointmentNameInfo, string finalDiagnosis)
        {
            this.InitializeComponent();
            this.visitInformation = visitInformation;
            this.appointmentNameInfo = appointmentNameInfo;
            this.finalDiagnosis = finalDiagnosis;
            this.visitInformationController = visitInformationController;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.visitInformationController.CreatedVisitInfo = true;
            this.Hide();
            VisitInformationDAL.AddFinalDiagnosis(this.visitInformation, this.finalDiagnosis);
        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewVisitInfoContentDialog viewVisitInfoContentDialog =
                new ViewVisitInfoContentDialog(this.appointmentNameInfo, this.finalDiagnosis, this.visitInformationController);
            await viewVisitInfoContentDialog.ShowAsync();
        }
    }
}
