using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class ViewTestResultContentDialog : ContentDialog
    {
        private readonly TestResult testResult;
        private readonly string testName;
        private readonly AppointmentNameInfo appointmentNameInfo;
        private bool viewResultsOnly;

        public ViewTestResultContentDialog(TestResult testResult, AppointmentNameInfo appointmentNameInfo, string testName, bool viewResultsOnly)
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
            this.patientIdTextBox.Text = this.appointmentNameInfo.Appointment.PatientId.ToString();
            this.patientNameTextBox.Text = this.appointmentNameInfo.PatientName;
            this.testIdTextBox.Text = this.testResult.TestId.ToString();
            this.testNameTextBox.Text = this.testName;
            this.dateTextBox.Text = this.testResult.ResultDateTime.Date.ToString();
            this.timeTextBox.Text = this.testResult.ResultDateTime.TimeOfDay.ToString();
            this.resultsRichEditBox.Document.SetText(TextSetOptions.None, this.testResult.Results);
            this.resultsRichEditBox.IsReadOnly = true;
        }

        private async void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            OrderedTestsContentDialog orderedTestsContentDialog = new OrderedTestsContentDialog(this.appointmentNameInfo, this.viewResultsOnly);
            await orderedTestsContentDialog.ShowAsync();
        }
    }
}
