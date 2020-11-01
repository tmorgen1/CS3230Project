﻿using System;
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
using ClinicDatabaseSystem.DAL;
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
        private bool viewVisitInfoHovered;
        private AppointmentNameInfo selectedAppointment;

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
            CreateAppointmentPatientSelectedContentDialog createAppointmentContentDialog = new CreateAppointmentPatientSelectedContentDialog();
            await createAppointmentContentDialog.ShowAsync();
            if (createAppointmentContentDialog.CreateAppointmentSuccessful)
            {
                this.viewModel.LoadAppointments(PatientController.CurrentPatient);
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

            if (!this.viewVisitInfoHovered)
            {
                this.viewVisitInfoButton.IsEnabled = false;
            }
        }

        private void AppointmentsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var appointment = this.getClickedAppointment();
            if (appointment != null )
            {
                var date = appointment.Appointment.ScheduledDate;
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

                if (VisitInformationDAL.GetVisitInfoFromAppointment(appointment.Appointment).Count == 0)
                {
                    this.createVisitInfoButton.IsEnabled = true;
                    this.viewVisitInfoButton.IsEnabled = false;
                }
                else
                {
                    this.createVisitInfoButton.IsEnabled = false;
                    this.viewVisitInfoButton.IsEnabled = true;
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
            this.editAppointmentButton.IsEnabled = false;
            this.createVisitInfoButton.IsEnabled = false;
            this.viewVisitInfoButton.IsEnabled = false;
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
            this.displayCreateVisitInfoContentDialog(this.selectedAppointment.Appointment);
        }

        private async void displayCreateVisitInfoContentDialog(Appointment appointment)
        {
            this.editAppointmentButton.IsEnabled = false;
            this.createVisitInfoButton.IsEnabled = false;
            this.viewVisitInfoButton.IsEnabled = false;
            CreateVisitInfoContentDialog createVisitInfoContentDialog = new CreateVisitInfoContentDialog(appointment);
            await createVisitInfoContentDialog.ShowAsync();
        }

        private void viewVisitInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.displayViewVisitInfoContentDialog(this.selectedAppointment.Appointment);
        }

        private async void displayViewVisitInfoContentDialog(Appointment appointment)
        {
            this.editAppointmentButton.IsEnabled = false;
            this.createVisitInfoButton.IsEnabled = false;
            this.viewVisitInfoButton.IsEnabled = false;
            ViewVisitInfoContentDialog viewVisitInfoContentDialog = new ViewVisitInfoContentDialog(appointment);
            await viewVisitInfoContentDialog.ShowAsync();
        }

        private void ViewVisitInfoButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.viewVisitInfoHovered = true;
        }

        private void ViewVisitInfoButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.viewVisitInfoHovered = false;
        }
    }
}
