using System;
using System.Collections.Generic;
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
        private AdminQueryResults results;

        public AdminQueryPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = AdministrationDAL.AdminQuery(this.queryTextBox.Text);
            this.loadDataTable(result);
        }

        private void loadDataTable(AdminQueryResults result)
        {
            var dataTable = new DataTable();
            foreach (var columnName in result.ColumnNames)
            {
                dataTable.Columns.Add(columnName);
            }
            DataGrid dataGrid = new DataGrid();
            dataGrid.Height = 700;
            dataGrid.Width = 700;
            dataGrid.Margin = new Thickness(150, 0, 0, 0);
            dataGrid.AutoGenerateColumns = false;
            dataGrid.DataContext = dataTable;
            this.grid.Children.Add(dataGrid);
        }
    }
}
