using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientRecordsPage : Page
    {
        private readonly PatientRecordsViewModel viewModel;

        public PatientRecordsPage()
        {
            this.InitializeComponent();
            this.viewModel = new PatientRecordsViewModel();
            this.updateCurrentUserTextBlocks();
        }

        private void newPatientButton_Click(object sender, RoutedEventArgs e)
        {
            this.displayRegisterPatientContentDialog();
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
        }

        private void updateCurrentUserTextBlocks()
        {
            this.fullNameTextBlock.Text =
                LoginController.CurrentUser.FirstName + " " + LoginController.CurrentUser.LastName;
            this.usernameTextBlock.Text = LoginController.CurrentUser.AccountId;
            this.idTextBlock.Text = LoginController.CurrentUser.NurseId.ToString();
        }

        private void editPatientButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.recordsDataGrid.SelectedIndex;
            var patient = this.viewModel.Patients[index];
            this.displayEditPatientContentDialog(patient);
        }

        private async void displayEditPatientContentDialog(Patient patient)
        {
            EditPatientContentDialog editPatientContentDialog = new EditPatientContentDialog(patient);
            await editPatientContentDialog.ShowAsync();
            if (editPatientContentDialog.UpdateSuccessful)
            {
                this.viewModel.LoadPatients();
            }
        }

        private void recordsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.editPatientButton.IsEnabled = true;
        }

        private void RecordsDataGrid_OnLostFocus(object sender, RoutedEventArgs e)
        {
            //TODO: fix edit button enabled
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

    }
}
