using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Shows all the patients in the database and allows to search for them.
    /// </summary>
    public sealed partial class PatientRecordsPage : Page
    {
        private readonly PatientRecordsViewModel viewModel;
        private Patient selectedPatient;
        private bool editPatientHovered;
        private bool viewAppointmentHovered;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientRecordsPage"/> class.
        /// </summary>
        public PatientRecordsPage()
        {
            this.InitializeComponent();
            this.viewModel = new PatientRecordsViewModel();
            this.updateCurrentUserTextBlocks();
        }

        private async void newPatientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.displayRegisterPatientContentDialog();
            }
            catch (Exception exception)
            {
                var messageDialog = new MessageDialog(exception.Message, "Sql Transaction Error")
                {
                    CancelCommandIndex = 0,
                    DefaultCommandIndex = 0
                };
                await messageDialog.ShowAsync();
            }
        }

        private async void displayRegisterPatientContentDialog()
        {
            RegisterPatientContentDialog registerPatientContentDialog = new RegisterPatientContentDialog();
            await registerPatientContentDialog.ShowAsync();
            if (registerPatientContentDialog.RegisterSuccessful)
            {
                this.viewModel.LoadPatients();
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            LoginController.CurrentAdministrator = null;
            LoginController.CurrentNurse = null;
        }

        private void updateCurrentUserTextBlocks()
        {
            this.fullNameTextBlock.Text =
                LoginController.CurrentNurse.FirstName + " " + LoginController.CurrentNurse.LastName;
            this.usernameTextBlock.Text = LoginController.CurrentNurse.AccountId;
            this.idTextBlock.Text = LoginController.CurrentNurse.NurseId.ToString();
        }

        private async void editPatientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.displayEditPatientContentDialog(this.selectedPatient);
            }
            catch (Exception exception)
            {
                var messageDialog = new MessageDialog(exception.Message, "Sql Transaction Error")
                {
                    CancelCommandIndex = 0,
                    DefaultCommandIndex = 0
                };
                await messageDialog.ShowAsync();
            }
        }

        private async void displayEditPatientContentDialog(Patient patient)
        {
            EditPatientContentDialog editPatientContentDialog = new EditPatientContentDialog(patient);
            await editPatientContentDialog.ShowAsync();
            if (editPatientContentDialog.UpdateSuccessful)
            {
                this.viewModel.LoadPatients();
                this.selectedPatient = editPatientContentDialog.NewPatient;
            }

            this.recordsDataGrid.SelectedItem = patient;
            this.enablePatientSelectedButtons();
        }

        private void enablePatientSelectedButtons()
        {
            this.editPatientButton.IsEnabled = true;
            this.viewAppointmentsButton.IsEnabled = true;
        }

        private void recordsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var patient = this.getClickedPatient();
            if (patient != null)
            {
                this.selectedPatient = patient;
                this.enablePatientSelectedButtons();
            }
        }

        private Patient getClickedPatient()
        {
            var index = this.recordsDataGrid.SelectedIndex;
            if (index != -1)
            {
                var patient = this.viewModel.Patients[index];
                return patient;
            }

            return null;
        }

        private void RecordsDataGrid_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.recordsDataGrid.SelectedIndex = -1;
            if (!this.editPatientHovered)
            {
                this.editPatientButton.IsEnabled = false;
            }

            if (!this.viewAppointmentHovered)
            {
                this.viewAppointmentsButton.IsEnabled = false;
            }

            if (!this.editPatientHovered && !this.viewAppointmentHovered)
            {
                this.selectedPatient = null;
            }
        }

        private void searchFullName_LostFocus(object sender, RoutedEventArgs e)
        {
            var fullName = this.searchFullName.Text.Trim();
            if (fullName.Contains(" "))
            {
                this.searchFullNameErrorTextBlock.Visibility = Visibility.Collapsed;
                this.searchPatients();
            }
            else if (this.searchFullName.Text != string.Empty && !fullName.Contains(" "))
            {
                this.searchFullNameErrorTextBlock.Text = "Please enter both first and last name.";
                this.searchFullNameErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.searchFullNameErrorTextBlock.Visibility = Visibility.Collapsed;
                this.viewModel.LoadPatients();
            }
        }

        private void searchDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            this.searchPatients();
        }

        private void searchPatients()
        {
            if (!this.searchDatePicker.SelectedDate.HasValue && this.searchFullName.Text != string.Empty)
            {
                var names = this.getFirstAndLastName();
                var firstName = names[0];
                var lastName = names[1];
                this.viewModel.Patients = (List<Patient>)(PatientDAL.SearchForPatients(firstName, lastName));
            }
            else if (this.searchFullName.Text == string.Empty && this.searchDatePicker.SelectedDate.HasValue)
            {
                this.viewModel.Patients = (List<Patient>)(PatientDAL.SearchForPatients(this.searchDatePicker.Date.Date));
            }
            else if (this.searchFullName.Text != string.Empty && this.searchDatePicker.SelectedDate.HasValue)
            {
                var names = this.getFirstAndLastName();
                var firstName = names[0];
                var lastName = names[1];
                this.viewModel.Patients = (List<Patient>)(PatientDAL.SearchForPatients(firstName, lastName, this.searchDatePicker.Date.Date));
            }
            else
            {
                this.viewModel.LoadPatients();
            }
        }

        private string[] getFirstAndLastName()
        {
            string fullName = this.searchFullName.Text;
            fullName = fullName.Trim();
            var firstSpaceIndex = fullName.IndexOf(" ", StringComparison.Ordinal);
            var firstName = fullName.Substring(0, firstSpaceIndex);
            var lastName = fullName.Substring(firstSpaceIndex + 1);
            var names = new[] { firstName, lastName };
            return names;
        }

        private void SymbolIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.searchDatePicker.SelectedDate = null;
            this.searchPatients();
        }

        private void SearchDatePicker_OnSelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            this.searchPatients();
        }

        private void EditPatientButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.editPatientHovered = true;
        }

        private void EditPatientButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.editPatientHovered = false;
        }

        private void viewAppointmentsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.viewAppointmentHovered = true;
        }

        private void ViewAppointmentsButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.viewAppointmentHovered = false;
        }

        private void viewAppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            PatientController.CurrentPatient = this.selectedPatient.PatientId;
            Frame.Navigate(typeof(PatientAppointmentsPage));
        }

    }
}
