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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class OrderTestContentDialog : ContentDialog
    {
        public OrderTestContentDialog()
        {
            this.InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: prompt user if they are sure they want to order the specified tests
            //TODO: add the order to the database
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var orderedTests = this.orderedTestsListView.Items;
            if (orderedTests != null && !orderedTests.Contains(this.testsListView.SelectedItem))
            {
                this.orderedTestsListView.Items?.Add(this.testsListView.SelectedItem);
            }
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
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO: setup searching for tests in tests list view
        }
    }
}
