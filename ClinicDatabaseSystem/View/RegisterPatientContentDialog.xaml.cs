using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
        }

        private bool validateInput()
        {
            return this.validateFirstName() && this.validateLastName() && this.validateBirthdate() &&
                   this.validatePhoneNumber() && this.validateAddress() && this.validateZip() && this.validateCity() &&
                   this.validateState();
        }

        private void checkButtonStatus()
        {
            if (this.validateInput())
            {
                this.registerPatientButton.IsEnabled = true;
            }
            else
            {
                this.registerPatientButton.IsEnabled = false;
            }
        }

        private bool validateFirstName()
        {
            return this.firstNameTextBox.Text != string.Empty;
        }

        private bool validateLastName()
        {
            return this.lastNameTextBox.Text != string.Empty;
        }

        private bool validateBirthdate()
        {
            int datetimeCompare = DateTime.Compare(this.birthdateDatePicker.Date.Date, DateTime.Today);
            return datetimeCompare <= 0 && this.birthdateDatePicker.SelectedDate != null;
        }

        private bool validatePhoneNumber()
        {
            Regex regex = new Regex(@"[0-9]{3}-[0-9]{3}-[0-9]{4}");
            if (!regex.IsMatch(this.phoneNumberTextBox.Text) && this.phoneNumberTextBox.Text != String.Empty)
            {
                this.phoneNumberErrorTextBlock.Text = "Incorrect format. E.g. 123-456-7890";
                this.phoneNumberErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }
            this.phoneNumberErrorTextBlock.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool validateAddress()
        {
            return this.addressTextBox.Text != string.Empty;
        }

        private bool validateZip()
        {
            return this.zipTextBox.Text != string.Empty && this.zipTextBox.Text.Length == 5;
        }

        private bool validateCity()
        {
            return this.cityTextBox.Text != string.Empty;
        }

        private bool validateState()
        {
            return this.stateComboBox.SelectedIndex != -1;
        }

        private void onFirstNameReachedMaxLength(object e, KeyRoutedEventArgs args)
        {
            if (this.firstNameTextBox.Text.Length >= this.firstNameTextBox.MaxLength)
            {
                this.firstNameErrorTextBlock.Text = "First name should be 15 characters at max.";
                this.firstNameErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.firstNameErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void onLastNameReachedMaxLength(object e, KeyRoutedEventArgs args)
        {
            if (this.lastNameTextBox.Text.Length >= this.lastNameTextBox.MaxLength)
            {
                this.lastNameErrorTextBlock.Text = "Last name should be 15 characters at max.";
                this.lastNameErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.lastNameErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void onSelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            int datetimeCompare = DateTime.Compare(this.birthdateDatePicker.Date.Date, DateTime.Today);
            if (datetimeCompare > 0)
            {
                this.birthdateErrorTextBlock.Text = "Birthdate should be today at the latest.";
                this.birthdateErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.birthdateErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void onPhoneNumberLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            this.validatePhoneNumber();
        }

        private void registerPatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.validateInput())
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
            this.checkButtonStatus();
            if (this.zipTextBox.Text.Length < 5 && this.zipTextBox.Text != string.Empty)
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
            this.checkButtonStatus();
            this.addressErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void Address2TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            this.address2ErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void CityTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
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

        private void FirstNameTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            this.firstNameErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void LastNameTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            this.lastNameErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void BirthdateDatePicker_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
        }

        private void StateComboBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (this.stateComboBox.SelectedIndex != -1)
            {
                this.stateErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.stateErrorTextBlock.Text = "Must select a state.";
                this.stateErrorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
