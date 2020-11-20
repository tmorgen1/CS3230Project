using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void nurseLoginButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.displayLoginContentDialog(true);
        }

        private void accessAdminButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.displayLoginContentDialog(false);
        }

        private async void displayLoginContentDialog(bool isNurseLogin)
        {
            LoginContentDialog loginContentDialog = new LoginContentDialog(isNurseLogin);
            ContentDialogResult result = await loginContentDialog.ShowAsync();
        }
    }
}
