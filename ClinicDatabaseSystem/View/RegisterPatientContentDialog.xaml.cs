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
using ClinicDatabaseSystem.Enums;
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
            this.addStatesToComboBox();

            this.firstNameTextBox.MaxLength = 15;
            this.lastNameTextBox.MaxLength = 15;
            this.firstNameTextBox.TextChanged += onFirstNameChanged;
            this.lastNameTextBox.TextChanged += onLastNameChanged;
        }

        private void onFirstNameChanged(object e, TextChangedEventArgs args)
        {
            if (this.firstNameTextBox.Text.Length >= this.firstNameTextBox.MaxLength)
            {
                this.firstNameErrorTextBlock.Text = "First name should be 15 characters at max.";
                this.firstNameErrorTextBlock.Opacity = 100.0;
            }
            else
            {
                this.firstNameErrorTextBlock.Opacity = 0.0;
            }
        }

        private void onLastNameChanged(object e, TextChangedEventArgs args)
        {
            if (this.lastNameTextBox.Text.Length >= this.lastNameTextBox.MaxLength)
            {
                this.lastNameErrorTextBlock.Text = "Last name should be 15 characters at max.";
                this.lastNameErrorTextBlock.Opacity = 100.0;
            }
            else
            {
                this.lastNameErrorTextBlock.Opacity = 0.0;
            }
        }

        private void registerPatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientDAL.InsertPatient(this.lastNameTextBox.Text, this.firstNameTextBox.Text,
                this.birthdateDatePicker.Date.Date, this.phoneNumberTextBox.Text,
                new Address(this.addressTextBox.Text, this.address2TextBox.Text, this.zipTextBox.Text,
                    this.cityTextBox.Text, this.stateComboBox.SelectionBoxItem.ToString()), this.zipTextBox.Text))
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

        private void addStatesToComboBox()
        {
            foreach (var state in Enum.GetValues(typeof(States)))
            {
                this.stateComboBox.Items?.Add(state);
            }
        }

        private bool isStateEmpty()
        {
            if (this.stateComboBox.SelectedIndex == -1)
            {
                this.stateErrorTextBlock.Text = "Must select a state.";
                this.stateErrorTextBlock.Visibility = Visibility.Visible;
                return true;
            }

            return false;
        }

        private void onCityTextKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (this.cityTextBox.Text.Length == this.cityTextBox.MaxLength)
            {
                this.cityErrorTextBlock.Text = "Max character limit reached.";
                this.cityErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.cityErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void zipTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.zipTextBox.Text.Length < 5)
            {
                this.zipErrorTextBlock.Text = "Zip must be 5 characters.";
                this.zipErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.zipErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void zipTextBox_KeyDown(object sender, RoutedEventArgs e)
        {
            if (this.zipTextBox.Text.Length == this.zipTextBox.MaxLength)
            {
                this.zipErrorTextBlock.Text = "Max character limit reached.";
                this.zipErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.zipErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void address2TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (this.address2TextBox.Text.Length == this.address2TextBox.MaxLength)
            {
                this.address2ErrorTextBlock.Text = "Max character limit reached.";
                this.address2ErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.address2ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void addressTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (this.addressTextBox.Text.Length == this.addressTextBox.MaxLength)
            {
                this.addressErrorTextBlock.Text = "Max character limit reached.";
                this.addressErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.addressErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void AddressTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.addressErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void Address2TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.address2ErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void CityTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.cityErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ZipTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (args.NewText.Any(c => !char.IsDigit(c)))
            {
                args.Cancel = true;
                this.zipErrorTextBlock.Text = "Only Digits allowed.";
                this.zipErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.zipErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }
    }
}
