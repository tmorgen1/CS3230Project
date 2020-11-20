using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles selecting tests to order.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class OrderTestContentDialog : ContentDialog
    {
        private readonly Appointment visitInfoAppointment;
        private readonly VisitInformation visitInformation;
        private readonly VisitInformationController visitInformationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderTestContentDialog"/> class.
        /// </summary>
        /// <param name="visitInformation">The visit information.</param>
        /// <param name="appointment">The appointment.</param>
        /// <param name="visitInfoController">The visit information controller.</param>
        public OrderTestContentDialog(VisitInformation visitInformation, Appointment appointment, VisitInformationController visitInfoController)
        {
            this.InitializeComponent();
            this.visitInformation = visitInformation;
            this.visitInfoAppointment = appointment;
            this.visitInformationController = visitInfoController;
            this.loadTests();
        }

        private void loadTests()
        {
            foreach (var test in TestDAL.GetTestTypes())
            {
                this.testsListView.Items?.Add(test.TestId + ": " + test.Name);
            }
        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            IList<string> orderedTests = new List<string>();
            foreach (var test in orderedTests)
            {
                this.orderedTestsListView.Items?.Add(test);
            }
            this.Hide();
            CreateVisitInfoContentDialog createVisitInfoContentDialog = new CreateVisitInfoContentDialog(this.visitInformation, this.visitInfoAppointment, orderedTests, this.visitInformationController);
            await createVisitInfoContentDialog.ShowAsync();
        }

        private async void orderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            IList<string> orderedTests = new List<string>();
            var itemCollection = this.orderedTestsListView.Items;
            if (itemCollection != null)
            {
                foreach (var test in itemCollection)
                {
                    orderedTests.Add(test.ToString());
                }

                ConfirmOrderTestContentDialog confirmOrderTestContentDialog =
                    new ConfirmOrderTestContentDialog(orderedTests, this.visitInformation, this.visitInfoAppointment, this.visitInformationController);
                await confirmOrderTestContentDialog.ShowAsync();
            }
        }

        private void checkOrderButton()
        {
            if (this.orderedTestsListView.Items?.Count != 0)
            {
                this.orderButton.IsEnabled = true;
            }
            else
            {
                this.orderButton.IsEnabled = false;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var orderedTests = this.orderedTestsListView.Items;
            if (orderedTests != null && !orderedTests.Contains(this.testsListView.SelectedItem))
            {
                this.orderedTestsListView.Items?.Add(this.testsListView.SelectedItem);
            }
            this.checkOrderButton();
        }

        private void testsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.testsListView.SelectedIndex != -1)
            {
                this.addButton.IsEnabled = true;
            }
            else
            {
                this.addButton.IsEnabled = false;
            }
        }

        private void orderedTestsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.orderedTestsListView.SelectedIndex != -1)
            {
                this.removeButton.IsEnabled = true;
            }
            else
            {
                this.removeButton.IsEnabled = false;
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            this.orderedTestsListView.Items?.Remove(this.orderedTestsListView.SelectedItem);
            this.checkOrderButton();
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO: setup searching for tests in tests list view
            if (this.searchTextBox.Text != string.Empty)
            {
                this.testsListView.Items?.Clear();
                foreach (var test in TestDAL.GetTestTypes())
                {
                    var testName = test.Name.ToLower();
                    var searchName = this.searchTextBox.Text.ToLower();
                    if (testName.Contains(searchName))
                    {
                        this.testsListView.Items?.Add(test.TestId + ": " + test.Name);
                    }
                }
            }
            else
            {
                this.testsListView.Items?.Clear();
                this.loadTests();
            }
        }
    }
}
