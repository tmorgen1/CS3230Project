using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Page that shows all the appointments for a specific patient.
    /// </summary>
    public sealed partial class PatientAppointmentsPage : Page
    {

        private readonly PatientAppointmentsViewModel viewModel;
        private bool editAppointmentHovered;
        private bool createVisitInfoHovered;
        private bool viewVisitInfoHovered;
        private bool deleteAppointmentHovered;
        private AppointmentNameInfo selectedAppointment;
        private readonly VisitInformationController visitInfoController;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientAppointmentsPage"/> class.
        /// </summary>
        public PatientAppointmentsPage()
        {
            this.InitializeComponent();
            this.viewModel = new PatientAppointmentsViewModel();
            this.updateCurrentUserTextBlocks();
            this.updateCurrentPageTitle();
            this.viewModel.LoadAppointments(PatientController.CurrentPatient);
            this.visitInfoController = new VisitInformationController();
            this.visitInfoController.PropertyChanged += VisitInfoControllerOnPropertyChanged;
        }

        private void VisitInfoControllerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.visitInfoController.CreatedVisitInfo = false;
            this.appointmentsDataGrid.ItemsSource = null;
            this.viewModel.LoadAppointments(PatientController.CurrentPatient);
            this.appointmentsDataGrid.SelectedItem = this.selectedAppointment;
            this.checkAppointment(this.selectedAppointment);
        }

        private void updateCurrentUserTextBlocks()
        {
            this.fullNameTextBlock.Text =
                LoginController.CurrentNurse.FirstName + " " + LoginController.CurrentNurse.LastName;
            this.usernameTextBlock.Text = LoginController.CurrentNurse.AccountId;
            this.idTextBlock.Text = LoginController.CurrentNurse.NurseId.ToString();
        }

        private void updateCurrentPageTitle()
        {
            var currentPatient = PatientDAL.GetPatientById(PatientController.CurrentPatient);
            var title = currentPatient.FirstName + " " + currentPatient.LastName + "'s Appointments";
            this.pageTitle1.Text = title;
            this.pageTitle2.Text = title;
        }

        private async void createAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAppointmentPatientSelectedContentDialog createAppointmentContentDialog = new CreateAppointmentPatientSelectedContentDialog();
            await createAppointmentContentDialog.ShowAsync();
            if (createAppointmentContentDialog.CreateAppointmentSuccessful)
            {
                this.viewModel.LoadAppointments(PatientController.CurrentPatient);
                this.appointmentsDataGrid.SelectedItem = this.viewModel.GetAppointmentNameInfo(createAppointmentContentDialog.CreatedAppointment);
            }


            this.checkAppointment(this.selectedAppointment);
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

            if (!this.viewVisitInfoHovered)
            {
                this.viewVisitInfoButton.IsEnabled = false;
            }

            if (!this.deleteAppointmentHovered)
            {
                this.deleteAppointmentButton.IsEnabled = false;
            }
        }

        private void AppointmentsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var appointment = this.getClickedAppointment();
            this.checkAppointment(appointment);
        }

        private void checkAppointment(AppointmentNameInfo appointment)
        {
            if (appointment != null)
            {
                var date = appointment.Appointment.ScheduledDate;
                var datetimeCompare = DateTime.Compare(date, DateTime.Today);
                this.selectedAppointment = appointment;
                var visitInfoCount = VisitInformationDAL.GetVisitInfoFromAppointment(appointment.Appointment);

                if (datetimeCompare > 0)
                {
                    this.editAppointmentButton.IsEnabled = true;
                    this.deleteAppointmentButton.IsEnabled = true;
                }
                else
                {
                    this.editAppointmentButton.IsEnabled = false;
                    this.deleteAppointmentButton.IsEnabled = false;
                }

                if (visitInfoCount == null)
                {
                    this.createVisitInfoButton.IsEnabled = true;
                    this.viewVisitInfoButton.IsEnabled = false;

                }
                else
                {
                    this.createVisitInfoButton.IsEnabled = false;
                    this.viewVisitInfoButton.IsEnabled = true;
                    this.editAppointmentButton.IsEnabled = false;
                    this.deleteAppointmentButton.IsEnabled = false;
                }
            }
        }

        private AppointmentNameInfo getClickedAppointment()
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
            this.displayEditAppointmentContentDialog(this.selectedAppointment.Appointment);
        }

        private async void displayEditAppointmentContentDialog(Appointment appointment)
        {
            this.setButtonsDisabled();
            EditAppointmentContentDialog editAppointmentContentDialog = new EditAppointmentContentDialog(appointment);
            await editAppointmentContentDialog.ShowAsync();
            if (editAppointmentContentDialog.EditAppointmentSuccessful)
            {
                this.viewModel.LoadAppointments(PatientController.CurrentPatient);
                foreach (var appointmentNameInfo in this.viewModel.Appointments)
                {
                    if (appointmentNameInfo.Appointment.Equals(editAppointmentContentDialog.NewAppointment))
                    {
                        this.selectedAppointment = appointmentNameInfo;
                    }
                }
            }
            this.appointmentsDataGrid.SelectedItem = this.selectedAppointment;
            this.checkAppointment(this.selectedAppointment);
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
            this.displayCreateVisitInfoContentDialog(this.selectedAppointment.Appointment);
        }

        private async void displayCreateVisitInfoContentDialog(Appointment appointment)
        {
            this.setButtonsDisabled();
            CreateVisitInfoContentDialog createVisitInfoContentDialog = new CreateVisitInfoContentDialog(appointment, this.visitInfoController);
            await createVisitInfoContentDialog.ShowAsync();
            this.appointmentsDataGrid.ItemsSource = null;
            this.viewModel.LoadAppointments(PatientController.CurrentPatient);
            this.appointmentsDataGrid.SelectedItem = this.selectedAppointment;
            this.checkAppointment(this.selectedAppointment);
        }

        private void setButtonsDisabled()
        {
            this.editAppointmentButton.IsEnabled = false;
            this.createVisitInfoButton.IsEnabled = false;
            this.viewVisitInfoButton.IsEnabled = false;
            this.deleteAppointmentButton.IsEnabled = false;
        }

        private void viewVisitInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.displayViewVisitInfoContentDialog(this.selectedAppointment);
        }

        private async void displayViewVisitInfoContentDialog(AppointmentNameInfo appointment)
        {
            this.setButtonsDisabled();
            ViewVisitInfoContentDialog viewVisitInfoContentDialog = new ViewVisitInfoContentDialog(appointment, this.visitInfoController);
            await viewVisitInfoContentDialog.ShowAsync();
            this.appointmentsDataGrid.ItemsSource = null;
            this.viewModel.LoadAppointments(PatientController.CurrentPatient);
            this.appointmentsDataGrid.SelectedItem = this.selectedAppointment;
            this.checkAppointment(this.selectedAppointment);
        }

        private void ViewVisitInfoButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.viewVisitInfoHovered = true;
        }

        private void ViewVisitInfoButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.viewVisitInfoHovered = false;
        }

        private void deleteAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add a ARE YOU SURE YOU WANT TO DELETE?
            if (this.selectedAppointment != null)
            {
                this.setButtonsDisabled();
                AppointmentDAL.DeleteAppointment(this.selectedAppointment.Appointment);
                this.viewModel.LoadAppointments(PatientController.CurrentPatient);
                this.appointmentsDataGrid.SelectedItem = null;
            }
        }

        private void DeleteAppointmentButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.deleteAppointmentHovered = true;
        }

        private void DeleteAppointmentButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.deleteAppointmentHovered = false;
        }

        private void AppointmentsDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var rowIndex = e.Row.GetIndex();
            var appointmentNameInfo = this.viewModel.Appointments[rowIndex];
            var date = appointmentNameInfo.Appointment.ScheduledDate;
            var datetimeCompare = DateTime.Compare(date, DateTime.Today);
            var visitInfo = VisitInformationDAL.GetVisitInfoFromAppointment(appointmentNameInfo.Appointment);
            if (datetimeCompare < 0)
            {
                e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 84, 84, 84));
            }
            else
            {
                if (visitInfo == null)
                {
                    e.Row.Background = new SolidColorBrush(Color.FromArgb(255, 119, 102, 153));
                }
                else
                {
                    var finalDiagnosis = visitInfo.FinalDiagnosis;
                    e.Row.Background = finalDiagnosis != null ? new SolidColorBrush(Color.FromArgb(255, 92, 152, 103)) : new SolidColorBrush(Color.FromArgb(255, 4, 96, 112));
                }
            }
        }
    }
}
