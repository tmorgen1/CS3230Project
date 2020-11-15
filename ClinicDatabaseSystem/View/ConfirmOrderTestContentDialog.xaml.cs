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
    public sealed partial class ConfirmOrderTestContentDialog : ContentDialog
    {
        private Appointment visitInfoAppointment;
        private VisitInformation visitInformation;
        private IList<string> orderedTests;

        public ConfirmOrderTestContentDialog(IList<string> orderedTests, VisitInformation visitInformation, Appointment appointment)
        {
            this.InitializeComponent();
            this.loadTests(orderedTests);
            this.visitInformation = visitInformation;
            this.visitInfoAppointment = appointment;
        }

        private void loadTests(IList<string> orderedTests)
        {
            this.orderedTests = orderedTests;
            foreach (var test in orderedTests)
            {
                this.orderedTestsListView.Items?.Add(test);
            }
        }

        private async void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CreateVisitInfoContentDialog createVisitInfoContentDialog = new CreateVisitInfoContentDialog(this.visitInformation, this.visitInfoAppointment, this.orderedTests);
            await createVisitInfoContentDialog.ShowAsync();
        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            OrderTestContentDialog orderTestContentDialog = new OrderTestContentDialog(this.visitInformation, this.visitInfoAppointment);
            await orderTestContentDialog.ShowAsync();
        }
    }
}
