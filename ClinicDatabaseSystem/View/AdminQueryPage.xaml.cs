using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using ClinicDatabaseSystem.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminQueryPage : Page
    {

        public AdminQueryPage()
        {
            this.InitializeComponent();
            this.updateCurrentUserTextBlocks();
        }

        private void updateCurrentUserTextBlocks()
        {
            this.fullNameTextBlock.Text =
                LoginController.CurrentAdministrator.FirstName + " " + LoginController.CurrentAdministrator.LastName;
            this.usernameTextBlock.Text = LoginController.CurrentAdministrator.AccountId;
            this.idTextBlock.Text = LoginController.CurrentAdministrator.AdminId.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = AdministrationDAL.AdminQuery(this.queryTextBox.Text);
            this.loadDataTable(result);
        }

        private void loadDataTable(DataTable result)
        {
            this.dataGrid.Columns.Clear();
            for (var i = 0; i < result.Columns.Count; i++)
            {
                this.dataGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = result.Columns[i].ColumnName,
                    Binding = new Binding { Path = new PropertyPath("[" + i.ToString() + "]") }
                });
            }
            var collection = new ObservableCollection<object>();
            foreach (DataRow row in result.Rows)
            {
                collection.Add(row.ItemArray);
            }

            this.dataGrid.ItemsSource = collection;
        }

        private void viewReportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void queryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.queryTextBox.Text == string.Empty)
            {
                this.executeButton.IsEnabled = false;
            }
            else
            {
                this.executeButton.IsEnabled = true;
            }
        }

        private void StartDatePicker_OnDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            this.checkStartAndEndDates();
        }

        private void checkStartAndEndDates()
        {
            if (this.startDatePicker.SelectedDate.HasValue && this.endDatePicker.SelectedDate.HasValue)
            {
                this.viewReportButton.IsEnabled = true;
            }
            else
            {
                this.viewReportButton.IsEnabled = false;
            }
            if (this.endDatePicker.SelectedDate.HasValue && this.endDatePicker.Date < this.startDatePicker.Date)
            {
                this.datesErrorTextBlock.Text = "Start date must come before end date.";
                this.datesErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.datesErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void EndDatePicker_OnDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            this.checkStartAndEndDates();
        }
    }
}
