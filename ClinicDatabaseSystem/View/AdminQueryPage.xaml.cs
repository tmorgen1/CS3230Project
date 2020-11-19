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
    }
}
