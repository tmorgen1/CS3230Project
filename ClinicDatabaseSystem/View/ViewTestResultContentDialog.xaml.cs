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
    /// Handles showing test results to user.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class ViewTestResultContentDialog : ContentDialog
    {
        private readonly TestResult testResult;
        private readonly string testName;
        private readonly AppointmentNameInfo appointmentNameInfo;
        private readonly bool viewResultsOnly;
        private VisitInformationController visitInformationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewTestResultContentDialog"/> class.
        /// </summary>
        /// <param name="testResult">The test result.</param>
        /// <param name="appointmentNameInfo">The appointment name information.</param>
        /// <param name="testName">Name of the test.</param>
        /// <param name="viewResultsOnly">if set to <c>true</c> [view results only].</param>
        public ViewTestResultContentDialog(TestResult testResult, AppointmentNameInfo appointmentNameInfo, string testName, bool viewResultsOnly, VisitInformationController visitInformationController)
        {
            this.InitializeComponent();
            this.testResult = testResult;
            this.testName = testName;
            this.appointmentNameInfo = appointmentNameInfo;
            this.viewResultsOnly = viewResultsOnly;
            this.visitInformationController = visitInformationController;
            this.loadInfo();
        }

        private void loadInfo()
        {
            this.patientIdTextBox.Text = this.appointmentNameInfo.Appointment.PatientId.ToString();
            this.patientNameTextBox.Text = this.appointmentNameInfo.PatientName;
            this.testIdTextBox.Text = this.testResult.TestId.ToString();
            this.testNameTextBox.Text = this.testName;
            this.visitDateTextBox.Text = this.testResult.VisitDateTime.Date.ToString("MM/dd/yyyy");
            this.testResultDateTextBox.Text = this.testResult.VisitDateTime.Date.ToString("MM/dd/yyyy");
            this.resultsRichEditBox.Document.SetText(TextSetOptions.None, this.testResult.Results);
            this.resultsRichEditBox.IsReadOnly = true;
            this.abnormalCheckBox.IsChecked = this.testResult.Abnormal;
        }

        private async void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            OrderedTestsContentDialog orderedTestsContentDialog = new OrderedTestsContentDialog(this.appointmentNameInfo, this.viewResultsOnly, this.visitInformationController);
            await orderedTestsContentDialog.ShowAsync();
        }
    }
}
