using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.Model;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles confirming test orders after selecting them.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class ConfirmOrderTestContentDialog : ContentDialog
    {
        private readonly Appointment visitInfoAppointment;
        private readonly VisitInformation visitInformation;
        private IList<string> orderedTests;
        private readonly VisitInformationController visitInformationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmOrderTestContentDialog"/> class.
        /// </summary>
        /// <param name="orderedTests">The ordered tests.</param>
        /// <param name="visitInformation">The visit information.</param>
        /// <param name="appointment">The appointment.</param>
        /// <param name="visitInfoController">The visit information controller.</param>
        public ConfirmOrderTestContentDialog(IList<string> orderedTests, VisitInformation visitInformation, Appointment appointment, VisitInformationController visitInfoController)
        {
            this.InitializeComponent();
            this.loadTests(orderedTests);
            this.visitInformation = visitInformation;
            this.visitInfoAppointment = appointment;
            this.visitInformationController = visitInfoController;
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
            CreateVisitInfoContentDialog createVisitInfoContentDialog = new CreateVisitInfoContentDialog(this.visitInformation, this.visitInfoAppointment, this.orderedTests, this.visitInformationController);
            await createVisitInfoContentDialog.ShowAsync();
        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            OrderTestContentDialog orderTestContentDialog = new OrderTestContentDialog(this.visitInformation, this.visitInfoAppointment, this.visitInformationController);
            await orderTestContentDialog.ShowAsync();
        }
    }
}
