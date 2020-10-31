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
using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientAppointmentsPage : Page
    {

        private readonly PatientAppointmentsViewModel viewModel;

        public PatientAppointmentsPage()
        {
            this.InitializeComponent();
            this.viewModel = new PatientAppointmentsViewModel();
            this.updateCurrentUserTextBlocks();
            this.viewModel.LoadAppointments(PatientController.CurrentPatient);
        }

        private void updateCurrentUserTextBlocks()
        {
            this.fullNameTextBlock.Text =
                LoginController.CurrentUser.FirstName + " " + LoginController.CurrentUser.LastName;
            this.usernameTextBlock.Text = LoginController.CurrentUser.AccountId;
            this.idTextBlock.Text = LoginController.CurrentUser.NurseId.ToString();
        }

        private async void createAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAppointmentContentDialog createAppointmentContentDialog = new CreateAppointmentContentDialog();
            await createAppointmentContentDialog.ShowAsync();
            if (createAppointmentContentDialog.CreateAppointmentSuccessful)
            {
                this.viewModel.LoadAppointments(1);
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void viewPatientsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PatientRecordsPage));
        }
    }
}
