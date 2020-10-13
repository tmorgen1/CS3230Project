using System;
using Windows.UI.Xaml.Controls;
using ClinicDatabaseSystem.DAL;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void nurseLoginButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.displayLoginContentDialog();
        }

        private void accessAdminButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private async void displayLoginContentDialog()
        {
            LoginContentDialog loginContentDialog = new LoginContentDialog();
            ContentDialogResult result = await loginContentDialog.ShowAsync();
        }
    }
}
