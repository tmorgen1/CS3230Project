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
    /// Handles taking in test result information.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CreateTestResultContentDialog : ContentDialog
    {
        private readonly TestResult testResult;
        private readonly string testName;
        private readonly AppointmentNameInfo appointmentNameInfo;
        private readonly bool viewResultsOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTestResultContentDialog"/> class.
        /// </summary>
        /// <param name="testResult">The test result.</param>
        /// <param name="appointmentNameInfo">The appointment name information.</param>
        /// <param name="testName">Name of the test.</param>
        /// <param name="viewResultsOnly">if set to <c>true</c> [view results only].</param>
        public CreateTestResultContentDialog(TestResult testResult, AppointmentNameInfo appointmentNameInfo, string testName, bool viewResultsOnly)
        {
            this.InitializeComponent();
            this.testResult = testResult;
            this.testName = testName;
            this.appointmentNameInfo = appointmentNameInfo;
            this.viewResultsOnly = viewResultsOnly;
            this.loadInfo();
        }

        private void loadInfo()
        {
            //TODO: load all info from database that isnt the result.
            this.patientIdTextBox.Text = this.appointmentNameInfo.Appointment.PatientId.ToString();
            this.patientNameTextBox.Text = this.appointmentNameInfo.PatientName;
            this.testIdTextBox.Text = this.testResult.TestId.ToString();
            this.testNameTextBox.Text = this.testName;
            this.dateTextBox.Text = this.testResult.ResultDateTime.Date.ToString();
            this.timeTextBox.Text = this.testResult.ResultDateTime.TimeOfDay.ToString();
        }

        private bool validateResults()
        {
            this.resultsRichEditBox.Document.GetText(0, out var reasons);
            reasons = reasons.Trim();
            return reasons != string.Empty;
        }

        private void checkButtonStatus()
        {
            this.confirmButton.IsEnabled = this.validateResults();
        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            OrderedTestsContentDialog orderedTestsContentDialog = new OrderedTestsContentDialog(this.appointmentNameInfo, this.viewResultsOnly);
            await orderedTestsContentDialog.ShowAsync();
        }

        private async void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.resultsRichEditBox.Document.GetText(0, out var results);
            var isAbnormalChecked = this.abnormalCheckBox.IsChecked ?? false;
            var newTestResult = new TestResult(this.testResult.TestId, this.testResult.PatientId, this.testResult.ResultDateTime, results, isAbnormalChecked);
            if (TestResultDAL.EditTestResult(newTestResult, this.testResult))
            {
                this.Hide();
                OrderedTestsContentDialog orderedTestsContentDialog = new OrderedTestsContentDialog(this.appointmentNameInfo, this.viewResultsOnly);
                await orderedTestsContentDialog.ShowAsync();
            }
        }

        private void ResultsRichEditBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateResults())
            {
                this.resultsErrorTextBlock.Text = "Must have result.";
                this.resultsErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.resultsErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void ResultsRichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            sender.Document.GetText(0, out var value);
            if (value != string.Empty)
            {
                this.resultsErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }
    }
}
