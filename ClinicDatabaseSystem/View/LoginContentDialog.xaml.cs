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
using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.DAL;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class LoginContentDialog : ContentDialog
    {
        public LoginContentDialog()
        {
            this.InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var nurseId = AuthDAL.AuthenticateNurse(this.usernameTextBox.Text, this.passwordBox.Password);
            if (nurseId > 0)
            {
                LoginController.CurrentUser = NurseDAL.GetNurse(nurseId);
                (Window.Current.Content as Frame)?.Navigate(typeof(PatientRecordsPage), null);
                this.Hide();
            }
            else
            {
                this.loginErrorTextBox.Text = "Invalid Credentials.";
                this.loginErrorTextBox.Visibility = Visibility.Visible;
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
