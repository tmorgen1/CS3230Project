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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class CreateAppointmentContentDialog : ContentDialog
    {


        public CreateAppointmentContentDialog(Patient patient)
        {
            this.InitializeComponent();
            this.loadPatientsComboBox();
            this.loadDoctorsComboBox();
        }

        private bool validateInput()
        {
            return this.validatePatient() && this.validateDoctor() && this.validateReason() && this.validateDate() && !this.IsDoubleBooked();
        }

        private bool IsDoubleBooked()
        {
            return false;
        }

        private bool validateDate()
        {
            int datetimeCompare = DateTime.Compare(this.datePicker.Date.Date, DateTime.Today);
            if (datetimeCompare >= 0 && this.datePicker.SelectedDate != null)
            {
                this.dateErrorTextBlock.Visibility = Visibility.Collapsed;
                return true;
            }
            else
            {
                this.dateErrorTextBlock.Text = "Invalid Date";
                this.dateErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }
        }

        private bool validateReason()
        {
            this.reasonRichEditBox.Document.GetText(0, out var reasons);
            if (reasons.Equals(string.Empty))
            {
                this.reasonErrorTextBlock.Text = "Must have reason.";
                this.reasonErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                this.reasonErrorTextBlock.Visibility = Visibility.Collapsed;
                return true;
            }
        }

        private bool validateDoctor()
        {
            if (this.doctorsListView.SelectedIndex == -1)
            {
                this.doctorErrorTextBlock.Text = "Must select doctor";
                this.doctorErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                this.doctorErrorTextBlock.Visibility = Visibility.Collapsed;
                return true;
            }
        }

        private bool validatePatient()
        {
            if (this.patientsListView.SelectedIndex == -1)
            {
                this.patientErrorTextBlock.Text = "Must select doctor";
                this.patientErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                this.patientErrorTextBlock.Visibility = Visibility.Collapsed;
                return true;
            }
        }

        private void loadDoctorsComboBox()
        {
            
        }

        private void loadPatientsComboBox()
        {
            var patients = (List<Patient>)PatientDAL.GetAllPatients();
            foreach (var patient in patients)
            {
                var fullName = patient.PatientId + ": " + patient.FirstName + " " + patient.LastName;
                this.patientsListView.Items?.Add(fullName);
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame)?.Navigate(typeof(PatientRecordsPage), null);
            this.Hide();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void datePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            this.validateDate();
        }

        private void reasonRichEditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.validateReason();
        }

        private void patientsListView_LostFocus(object sender, RoutedEventArgs e)
        {
            this.validatePatient();
        }

        private void doctorsListView_LostFocus(object sender, RoutedEventArgs e)
        {
            this.validateDoctor();
        }
    }
}
