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

        private void checkButtonStatus()
        {
            if (this.validateInput())
            {
                this.createButton.IsEnabled = true;
            } else
            {
                this.createButton.IsEnabled = false;
            }
        }

        private bool IsDoubleBooked()
        {
            return false;
        }

        private bool validateDate()
        {
            int datetimeCompare = DateTime.Compare(this.datePicker.Date.Date, DateTime.Today);
            return datetimeCompare >= 0 && this.datePicker.SelectedDate != null;
        }

        private bool validateReason()
        {
            this.reasonRichEditBox.Document.GetText(0, out var reasons);
            reasons = reasons.Trim();
            return reasons != string.Empty;
        }

        private bool validateDoctor()
        {
            return this.doctorsListView.SelectedIndex != -1;
        }

        private bool validatePatient()
        {
            return this.patientsListView.SelectedIndex != -1;
        }

        private void loadDoctorsComboBox()
        {
            var doctors = (List<Doctor>) DoctorDAL.GetAllDoctors();
            foreach (var doctor in doctors)
            {
                var fullName = doctor.DoctorId + ": " + doctor.FirstName + " " + doctor.LastName;
                this.doctorsListView.Items?.Add(fullName);
            }
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
            this.checkButtonStatus();
            if (!this.validateDate())
            {
                this.dateErrorTextBlock.Text = "Invalid Date";
                this.dateErrorTextBlock.Visibility = Visibility.Visible;
            }
            else if (this.IsDoubleBooked())
            {
                this.dateErrorTextBlock.Text = "Date already booked";
                this.dateErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.dateErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void reasonRichEditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateReason())
            {
                this.reasonErrorTextBlock.Text = "Must have reason.";
                this.reasonErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.reasonErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void DoctorsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateDoctor())
            {
                this.doctorErrorTextBlock.Text = "Must select doctor";
                this.doctorErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.doctorErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void PatientsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validatePatient())
            {
                this.patientErrorTextBlock.Text = "Must select doctor";
                this.patientErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.patientErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void ReasonRichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            this.checkButtonStatus();
        }
    }
}
