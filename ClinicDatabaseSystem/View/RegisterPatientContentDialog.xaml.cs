using System;
using System.Collections.Generic;
using System.Globalization;
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
using ClinicDatabaseSystem.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class RegisterPatientContentDialog : ContentDialog
    {

        public bool RegisterSuccessful { get; private set; }
        
        public RegisterPatientContentDialog()
        {
            this.InitializeComponent();
        }

        private void registerPatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientDAL.InsertPatient(this.lastNameTextBox.Text, this.firstNameTextBox.Text,
                DateTime.Parse(this.birthdateTextBox.Text), this.phoneNumberTextBox.Text,
                new Address(this.addressTextBox.Text, this.address2TextBox.Text, this.zipTextBox.Text,
                    this.cityTextBox.Text, this.stateTextBox.Text), this.zipTextBox.Text))
            {
                (Window.Current.Content as Frame)?.Navigate(typeof(PatientRecordsPage), null);
                this.Hide();
                this.RegisterSuccessful = true;
            }

            this.RegisterSuccessful = false;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
