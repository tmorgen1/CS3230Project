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
using ClinicDatabaseSystem.ViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class OrderedTestsContentDialog : ContentDialog
    {
        private AppointmentNameInfo appointmentNameInfo;
        public OrderedTestsContentDialog(AppointmentNameInfo appointmentNameInfo)
        {
            this.InitializeComponent();
            this.appointmentNameInfo = appointmentNameInfo;
            this.loadTests();
        }

        private void loadTests()
        {
            //TODO: load all tests into list view from database
        }

        private async void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewVisitInfoContentDialog viewVisitInfoContentDialog = new ViewVisitInfoContentDialog(this.appointmentNameInfo);
            await viewVisitInfoContentDialog.ShowAsync();
        }

        private async void createResultButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            //TODO: if selected item does not have a test result
            CreateTestResultContentDialog createTestResultContentDialog = new CreateTestResultContentDialog();
            await createTestResultContentDialog.ShowAsync();
            //TODO: if selected item has test result
            //ViewTestResultContentDialog viewTestResultContentDialog = new ViewTestResultContentDialog();
            //await viewTestResultContentDialog.ShowAsync();
        }

        private void orderedTestsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.orderedTestsListView.SelectedIndex != -1)
            {
                this.createResultButton.IsEnabled = true;
            }
            else
            {
                this.createResultButton.IsEnabled = false;
            }
        }
    }
}
