using ClinicDatabaseSystem.DAL;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;
using System.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Holds the admin capabilities including their ability to query.
    /// </summary>
    public sealed partial class AdminQueryPage : Page
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminQueryPage"/> class.
        /// </summary>
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
