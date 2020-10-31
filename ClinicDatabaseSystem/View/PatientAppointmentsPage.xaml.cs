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
using ClinicDatabaseSystem.Model;
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
        private bool editAppointmentHovered;
        private bool createVisitInfoHovered;
        private Appointment selectedAppointment;

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

        private void EditAppointmentButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.editAppointmentHovered = true;
        }

        private void EditAppointmentButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.editAppointmentHovered = false;
        }

        private void appointmentsDataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            this.appointmentsDataGrid.SelectedIndex = -1;
            if (!this.editAppointmentHovered)
            {
                this.editAppointmentButton.IsEnabled = false;
            }

            if (!this.createVisitInfoHovered)
            {
                this.createVisitInfoButton.IsEnabled = false;
            }
        }

        private void AppointmentsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var appointment = this.getClickedAppointment();
            if (appointment != null )
            {
                var date = appointment.ScheduledDate;
                var datetimeCompare = DateTime.Compare(date, DateTime.Today);
                this.selectedAppointment = appointment;
                if (datetimeCompare > 0)
                {
                    this.editAppointmentButton.IsEnabled = true;
                }
                else
                {
                    this.editAppointmentButton.IsEnabled = false;
                }

                this.createVisitInfoButton.IsEnabled = true;
            }
        }

        private Appointment getClickedAppointment()
        {
            var index = this.appointmentsDataGrid.SelectedIndex;
            if (index != -1)
            {
                var appointment = this.viewModel.Appointments[index];
                return appointment;
            }

            return null;
        }

        private void editAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.displayEditAppointmentContentDialog(this.selectedAppointment);
        }

        private async void displayEditAppointmentContentDialog(Appointment appointment)
        {
            this.editAppointmentButton.IsEnabled = false;
            this.createVisitInfoButton.IsEnabled = false;
            EditAppointmentContentDialog editAppointmentContentDialog = new EditAppointmentContentDialog(appointment);
            await editAppointmentContentDialog.ShowAsync();
            if (editAppointmentContentDialog.EditAppointmentSuccessful)
            {
                this.viewModel.LoadAppointments(PatientController.CurrentPatient);
            }
        }

        private void createVisitInfoButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.createVisitInfoHovered = true;
        }

        private void CreateVisitInfoButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.createVisitInfoHovered = false;
        }

        private void createVisitInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.displayCreateVisitInfoContentDialog(this.selectedAppointment);
        }

        private async void displayCreateVisitInfoContentDialog(Appointment appointment)
        {
            this.editAppointmentButton.IsEnabled = false;
            this.createVisitInfoButton.IsEnabled = false;
            CreateVisitInfoContentDialog createVisitInfoContentDialog = new CreateVisitInfoContentDialog(appointment);
            await createVisitInfoContentDialog.ShowAsync();
        }
    }
}
