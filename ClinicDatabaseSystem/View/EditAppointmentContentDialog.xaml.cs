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
    public sealed partial class EditAppointmentContentDialog : ContentDialog
    {
        public bool EditAppointmentSuccessful;
        private readonly Appointment previousAppointment;

        public EditAppointmentContentDialog(Appointment appointment)
        {
            this.InitializeComponent();
            this.loadPatientsComboBox();
            this.loadDoctorsComboBox();
            this.previousAppointment = appointment;
            this.loadAppointmentInfo();
        }

        private void loadAppointmentInfo()
        {
            this.reasonRichEditBox.Document.SetText(0, this.previousAppointment.Reason);
            this.datePicker.SelectedDate = this.previousAppointment.ScheduledDate;
            this.timePicker.SelectedTime = this.previousAppointment.ScheduledDate.TimeOfDay;
            this.setSelectedPatient();
            this.setSelectedDoctor();
            this.checkButtonStatus();
        }

        private void setSelectedPatient()
        {
            var patients = this.patientsListView.Items;
            if (patients != null)
            {
                foreach (var patient in patients)
                {
                    if (patient.ToString().Split(':')[0].Equals(this.previousAppointment.PatientId.ToString()))
                    {
                        this.patientsListView.SelectedItem = patient;
                    }
                }
            }
        }

        private void setSelectedDoctor()
        {
            var doctors = this.doctorsListView.Items;
            if (doctors != null)
            {
                foreach (var doctor in doctors)
                {
                    if (doctor.ToString().Split(':')[0].Equals(this.previousAppointment.DoctorId.ToString()))
                    {
                        this.doctorsListView.SelectedItem = doctor;
                    }
                }
            }
        }

        private bool validateInput()
        {
            return this.validatePatient() && this.validateDoctor() && this.validateReason() && this.validateDate() && this.validateTime() && !this.IsDoubleBooked();
        }

        private void checkButtonStatus()
        {
            if (this.validateInput())
            {
                this.confirmButton.IsEnabled = true;
            }
            else
            {
                this.confirmButton.IsEnabled = false;
            }
        }

        private bool IsDoubleBooked()
        {
            var appointments = (List<Appointment>)AppointmentDAL.GetAllAppointments();
            foreach (var appointment in appointments)
            {
                var doctor = this.doctorsListView.SelectedItem;
                var patient = this.patientsListView.SelectedItem;
                var date = this.getSelectedDateAndTime();
                if (doctor != null && patient != null && (date == appointment.ScheduledDate &&
                                       doctor.ToString().Contains(appointment.DoctorId.ToString()) ||
                                       date == appointment.ScheduledDate &&
                                       patient.ToString().Contains(appointment.PatientId.ToString())))
                {
                    this.EditAppointmentSuccessful = true;
                    return true;
                }
            }

            this.EditAppointmentSuccessful = false;
            return false;
        }

        private bool validateDate()
        {
            var date = this.getSelectedDateAndTime();
            int datetimeCompare = DateTime.Compare(date, DateTime.Today);
            return datetimeCompare >= 0 && this.datePicker.SelectedDate != null;
        }

        private bool validateTime()
        {
            var date = this.getSelectedDateAndTime();
            int datetimeCompare = DateTime.Compare(date, DateTime.Now);
            return datetimeCompare >= 0 && this.timePicker.SelectedTime != null;
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
            var doctors = (List<Doctor>)DoctorDAL.GetAllDoctors();
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

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.validateInput())
            {
                var patientID = this.patientsListView.SelectedItem?.ToString().Split(':')[0];
                var doctorID = this.doctorsListView.SelectedItem?.ToString().Split(':')[0];
                this.reasonRichEditBox.Document.GetText(0, out var reasons);
                var date = this.getSelectedDateAndTime();
                reasons = reasons.Trim();
                if (AppointmentDAL.EditAppointment(this.previousAppointment, new Appointment(int.Parse(patientID ?? string.Empty),
                    date, int.Parse(doctorID ?? string.Empty), reasons)))
                {
                    this.Hide();
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void timePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            this.checkButtonStatus();
            if (!this.validateTime())
            {
                this.timeErrorTextBlock.Text = "Invalid Time";
                this.timeErrorTextBlock.Visibility = Visibility.Visible;
            }
            else if (this.IsDoubleBooked())
            {
                this.timeErrorTextBlock.Text = "Time already booked";
                this.timeErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.timeErrorTextBlock.Visibility = Visibility.Collapsed;
            }
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

        private DateTime getSelectedDateAndTime()
        {
            if (this.datePicker.SelectedDate.HasValue && this.timePicker.SelectedTime.HasValue)
            {
                var date = new DateTime(this.datePicker.Date.Year, this.datePicker.Date.Month, this.datePicker.Date.Day,
                    this.timePicker.Time.Hours, this.timePicker.Time.Minutes, this.timePicker.Time.Seconds);
                return date;
            }
            else if (this.datePicker.SelectedDate.HasValue && !this.timePicker.SelectedTime.HasValue)
            {
                return this.datePicker.Date.Date;
            }
            else if (!this.datePicker.SelectedDate.HasValue && this.timePicker.SelectedTime.HasValue)
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, this.timePicker.Time.Hours,
                    this.timePicker.Time.Minutes, this.timePicker.Time.Seconds);
            }

            return DateTime.Now;
        }
    }
}
