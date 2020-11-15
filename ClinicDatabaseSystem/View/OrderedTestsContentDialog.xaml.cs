using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class OrderedTestsContentDialog : ContentDialog
    {
        private AppointmentNameInfo appointmentNameInfo;
        private IList<TestResult> orderedTestResults;
        private TestResult selectedTestResult;
        private bool viewResultsOnly;

        public OrderedTestsContentDialog(AppointmentNameInfo appointmentNameInfo, bool viewResultsOnly)
        {
            this.InitializeComponent();
            this.appointmentNameInfo = appointmentNameInfo;
            this.viewResultsOnly = viewResultsOnly;
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
            ViewVisitInfoContentDialog viewVisitInfoContentDialog = new ViewVisitInfoContentDialog(this.appointmentNameInfo);
            await viewVisitInfoContentDialog.ShowAsync();
        }

        private async void createResultButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var testName = this.orderedTestsListView.SelectedItem?.ToString().Split(':')[1].Trim();
            if (this.selectedTestHasResult())
            {
                CreateTestResultContentDialog createTestResultContentDialog = new CreateTestResultContentDialog(this.selectedTestResult, this.appointmentNameInfo, testName, this.viewResultsOnly);
                await createTestResultContentDialog.ShowAsync();
            }
            else
            {
                ViewTestResultContentDialog viewTestResultContentDialog = new ViewTestResultContentDialog(this.selectedTestResult, this.appointmentNameInfo, testName, this.viewResultsOnly);
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
                    } else if (orderedTestResult.TestId == Int32.Parse(testId) &&
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
