using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ClinicDatabaseSystem.Controller;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles showing user the ordered tests for a visit/appointment.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class OrderedTestsContentDialog : ContentDialog
    {
        private readonly AppointmentNameInfo appointmentNameInfo;
        private IList<TestResult> orderedTestResults;
        private TestResult selectedTestResult;
        private readonly bool viewResultsOnly;
        private VisitInformationController visitInformationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedTestsContentDialog"/> class.
        /// </summary>
        /// <param name="appointmentNameInfo">The appointment name information.</param>
        /// <param name="viewResultsOnly">if set to <c>true</c> [view results only].</param>
        public OrderedTestsContentDialog(AppointmentNameInfo appointmentNameInfo, bool viewResultsOnly, VisitInformationController visitInformationController)
        {
            this.InitializeComponent();
            this.appointmentNameInfo = appointmentNameInfo;
            this.viewResultsOnly = viewResultsOnly;
            this.visitInformationController = visitInformationController;
            this.loadTests();
            this.checkFinalDiagnosis();
        }

        private void checkFinalDiagnosis()
        {
            if (this.viewResultsOnly)
            {
                this.createResultButton.Content = "View Result";
            }
        }

        private void loadTests()
        {
            //TODO: load all tests into list view from database
            var orderedTests = TestResultDAL.GetTestResultsFromPatientAndVisitInfo(
                this.appointmentNameInfo.Appointment.PatientId, this.appointmentNameInfo.Appointment.ScheduledDate);
            this.orderedTestResults = orderedTests;
            var testTypes = TestDAL.GetTestTypes();
            foreach (var orderedTest in orderedTests)
            {
                foreach (var currentTestType in testTypes)
                {
                    if (orderedTest.TestId == currentTestType.TestId)
                    {
                        this.orderedTestsListView.Items?.Add(orderedTest.TestId + ": " + currentTestType.Name);
                    }
                }
            }
        }

        private async void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewVisitInfoContentDialog viewVisitInfoContentDialog = new ViewVisitInfoContentDialog(this.appointmentNameInfo, this.visitInformationController);
            await viewVisitInfoContentDialog.ShowAsync();
        }

        private async void createResultButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var testName = this.orderedTestsListView.SelectedItem?.ToString().Split(':')[1].Trim();
            if (this.selectedTestHasResult())
            {
                CreateTestResultContentDialog createTestResultContentDialog = new CreateTestResultContentDialog(this.selectedTestResult, this.appointmentNameInfo, testName, this.viewResultsOnly, this.visitInformationController);
                await createTestResultContentDialog.ShowAsync();
            }
            else
            {
                ViewTestResultContentDialog viewTestResultContentDialog = new ViewTestResultContentDialog(this.selectedTestResult, this.appointmentNameInfo, testName, this.viewResultsOnly, this.visitInformationController);
                await viewTestResultContentDialog.ShowAsync();
            }
        }

        private bool selectedTestHasResult()
        {
            var resultButton = this.createResultButton;
            return resultButton != null && resultButton.Content?.ToString() == "Create Result";
        }

        private void orderedTestsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.orderedTestsListView.SelectedIndex != -1)
            {
                this.createResultButton.IsEnabled = true;
                var testId = this.orderedTestsListView.SelectedItem?.ToString().Split(':')[0].Trim();
                foreach (var orderedTestResult in this.orderedTestResults)
                {
                    if (orderedTestResult.TestId == Int32.Parse(testId) && orderedTestResult.Results == string.Empty && !this.viewResultsOnly)
                    {
                        this.createResultButton.Content = "Create Result";
                        this.selectedTestResult = orderedTestResult;
                    }
                    else if (orderedTestResult.TestId == Int32.Parse(testId) &&
                             orderedTestResult.Results != string.Empty || this.viewResultsOnly && orderedTestResult.TestId == Int32.Parse(testId))
                    {
                        this.createResultButton.Content = "View Result";
                        this.selectedTestResult = orderedTestResult;
                    }
                }
            }
            else
            {
                this.createResultButton.IsEnabled = false;
            }
        }
    }
}
