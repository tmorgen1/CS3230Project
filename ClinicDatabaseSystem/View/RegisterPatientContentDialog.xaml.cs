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

            this.firstNameTextBox.MaxLength = 15;
            this.lastNameTextBox.MaxLength = 15;
            this.phoneNumberTextBox.MaxLength = 12;

            this.firstNameTextBox.KeyDown += onFirstNameReachedMaxLength;
            this.lastNameTextBox.KeyDown += onLastNameReachedMaxLength;
            this.birthdateDatePicker.SelectedDateChanged += onSelectedDateChanged;
            this.phoneNumberTextBox.LostFocus += onPhoneNumberKeyDown;
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

        private void onPhoneNumberKeyDown(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"[0-9]{3}-[0-9]{3}-[0-9]{4}");
            if (!regex.IsMatch(this.phoneNumberTextBox.Text) && this.phoneNumberTextBox.Text != String.Empty)
            {
                this.phoneNumberErrorTextBlock.Text = "Incorrect format. E.g. 123-456-7890";
                this.phoneNumberErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.phoneNumberErrorTextBlock.Visibility = Visibility.Collapsed;
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
    }
}
