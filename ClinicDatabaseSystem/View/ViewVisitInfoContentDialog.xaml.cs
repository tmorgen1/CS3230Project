using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;
using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ClinicDatabaseSystem.Controller;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles showing visit info to user.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class ViewVisitInfoContentDialog : ContentDialog
    {

        private readonly AppointmentNameInfo appointmentNameInfo;
        private bool hasInitialFinalDiagnosis;
        private VisitInformationController visitInformationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewVisitInfoContentDialog"/> class.
        /// </summary>
        /// <param name="appointmentNameInfo">The appointment name information.</param>
        public ViewVisitInfoContentDialog(AppointmentNameInfo appointmentNameInfo, VisitInformationController visitInformationController)
        {
            this.InitializeComponent();
            this.appointmentNameInfo = appointmentNameInfo;
            this.visitInformationController = visitInformationController;
            this.loadVisitInfo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewVisitInfoContentDialog"/> class.
        /// </summary>
        /// <param name="appointmentNameInfo">The appointment name information.</param>
        /// <param name="finalDiagnosis">The final diagnosis.</param>
        public ViewVisitInfoContentDialog(AppointmentNameInfo appointmentNameInfo, string finalDiagnosis, VisitInformationController visitInformationController)
        {
            this.InitializeComponent();
            this.appointmentNameInfo = appointmentNameInfo;
            this.visitInformationController = visitInformationController;
            this.loadVisitInfo();
            this.finalDiagnosisRichEditBox.Document.SetText(0, finalDiagnosis);
        }

        private void loadVisitInfo()
        {
            this.patientIdTextBox.Text = this.appointmentNameInfo.Appointment.PatientId.ToString();
            this.patientNameTextBox.Text = this.appointmentNameInfo.PatientName;
            this.dateTextBox.Text = this.appointmentNameInfo.Appointment.ScheduledDate.Date.ToString("MM/dd/yyyy");
            
            var visitInfo = VisitInformationDAL.GetVisitInfoFromAppointment(this.appointmentNameInfo.Appointment);
            this.systolicBpTextBox.Text = visitInfo.SystolicBp;
            this.diastolicBpTextBox.Text = visitInfo.DiastolicBp;
            this.bodyTempTextBox.Text = visitInfo.BodyTemp;
            this.pulseTextBox.Text = visitInfo.Pulse;
            this.weightTextBox.Text = visitInfo.Weight;

            this.symptomsRichEditBox.Document.SetText(TextSetOptions.None, visitInfo.Symptoms);
            this.symptomsRichEditBox.IsReadOnly = true;

            this.initialDiagnosisRichEditBox.Document.SetText(TextSetOptions.None, visitInfo.InitialDiagnosis);
            this.initialDiagnosisRichEditBox.IsReadOnly = true;

            if (visitInfo.FinalDiagnosis != null)
            {
                this.finalDiagnosisRichEditBox.Document.SetText(TextSetOptions.None, visitInfo.FinalDiagnosis);
                this.finalDiagnosisRichEditBox.IsReadOnly = true;
                this.finalDiagnosisRichEditBox.IsTabStop = false;
                this.hasInitialFinalDiagnosis = true;
                this.closeButton.Content = "Done";
            }
            else
            {
                if (!this.hasAllTestResults(visitInfo))
                {
                    this.finalDiagnosisRichEditBox.IsReadOnly = true;
                    this.finalDiagnosisRichEditBox.IsTabStop = false;
                }
            }
        }

        private bool hasAllTestResults(VisitInformation visitInfo)
        {
            var testResults =
                TestResultDAL.GetTestResultsFromPatientAndVisitInfo(visitInfo.PatientId, visitInfo.VisitDateTime);
            foreach (var testResult in testResults)
            {
                if (testResult.Results == string.Empty)
                {
                    return false;
                }
            }

            return true;
        } 

        private async void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.finalDiagnosisRichEditBox.Document.GetText(TextGetOptions.None, out var finalDiagnosis);
            this.Hide();
            if (finalDiagnosis.Trim() != string.Empty && !this.hasInitialFinalDiagnosis)
            {
                var patientId = this.appointmentNameInfo.Appointment.PatientId;
                var date = this.appointmentNameInfo.Appointment.ScheduledDate;
                this.symptomsRichEditBox.Document.GetText(0, out var symptoms);
                this.initialDiagnosisRichEditBox.Document.GetText(0, out var diagnosis);
                var visitInformation = new VisitInformation(patientId, date,
                    this.systolicBpTextBox.Text, this.diastolicBpTextBox.Text, this.bodyTempTextBox.Text,
                    this.pulseTextBox.Text, this.weightTextBox.Text, symptoms, diagnosis, finalDiagnosis.Trim());
                ConfirmFinalDiagnosisContentDialog confirmFinalDiagnosisContentDialog = new ConfirmFinalDiagnosisContentDialog(visitInformation, this.appointmentNameInfo, finalDiagnosis.Trim(), this.visitInformationController);
                await confirmFinalDiagnosisContentDialog.ShowAsync();
            }
        }

        private bool hasFinalDiagnosis()
        {
            this.finalDiagnosisRichEditBox.Document.GetText(TextGetOptions.None, out var finalDiagnosis);
            return finalDiagnosis.Trim() != string.Empty;
        }

        private async void viewTestsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            OrderedTestsContentDialog orderedTestsContentDialog = new OrderedTestsContentDialog(this.appointmentNameInfo, this.hasFinalDiagnosis(), this.visitInformationController);
            await orderedTestsContentDialog.ShowAsync();
        }
    }
}
