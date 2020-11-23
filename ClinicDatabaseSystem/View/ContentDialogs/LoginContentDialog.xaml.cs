using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.DAL;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles logins.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class LoginContentDialog : ContentDialog
    {
        private readonly bool isNurseLogin;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginContentDialog"/> class.
        /// </summary>
        /// <param name="isNurseLogin">if set to <c>true</c> [is nurse login].</param>
        public LoginContentDialog(bool isNurseLogin)
        {
            this.InitializeComponent();
            this.isNurseLogin = isNurseLogin;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.isNurseLogin)
            {
                this.nurseLogin();
            }
            else
            {
                this.adminLogin();
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void nurseLogin()
        {
            var nurseId = AuthDAL.AuthenticateNurse(this.usernameTextBox.Text, this.passwordBox.Password);
            if (nurseId > 0)
            {
                LoginController.CurrentNurse = NurseDAL.GetNurse(nurseId);
                (Window.Current.Content as Frame)?.Navigate(typeof(PatientRecordsPage), null);
                this.Hide();
            }
            else
            {
                this.loginErrorTextBox.Text = "Invalid Credentials.";
                this.loginErrorTextBox.Visibility = Visibility.Visible;
            }
        }

        private void adminLogin()
        {
            var adminId = AuthDAL.AuthenticateAdmin(this.usernameTextBox.Text, this.passwordBox.Password);
            if (adminId > 0)
            {
                LoginController.CurrentAdministrator = AdministrationDAL.GetAdmin(adminId);
                (Window.Current.Content as Frame)?.Navigate(typeof(AdminQueryPage), null);
                this.Hide();
            }
            else
            {
                this.loginErrorTextBox.Text = "Invalid Credentials.";
                this.loginErrorTextBox.Visibility = Visibility.Visible;
            }
        }

        private void UsernameTextBox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            this.checkLoginButtonStatus();
            this.loginErrorTextBox.Visibility = Visibility.Collapsed;
        }

        private void checkLoginButtonStatus()
        {
            if (this.validateInput())
            {
                this.loginButton.IsEnabled = true;
            }
            else
            {
                this.loginButton.IsEnabled = false;
            }
        }

        private bool validateInput()
        {
            return this.usernameTextBox.Text != string.Empty && this.passwordBox.Password != string.Empty;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            this.checkLoginButtonStatus();
            this.loginErrorTextBox.Visibility = Visibility.Collapsed;
        }
    }
}
