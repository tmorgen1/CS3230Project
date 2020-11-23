using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Enums;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.Utils;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles editing a patients information.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class EditPatientContentDialog : ContentDialog
    {
        private readonly Patient patient;

        /// <summary>
        /// Gets a value indicating whether [update successful].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [update successful]; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateSuccessful { get; private set; }

        /// <summary>
        /// Gets the new patient.
        /// </summary>
        /// <value>
        /// The new patient.
        /// </value>
        public Patient NewPatient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPatientContentDialog"/> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        public EditPatientContentDialog(Patient patient)
        {
            this.InitializeComponent();
            this.patient = patient;
            this.addStatesToComboBox();
            this.loadPatientInfo(patient);
        }

        private void addStatesToComboBox()
        {
            foreach (var state in Enum.GetValues(typeof(States)))
            {
                this.stateComboBox.Items?.Add(state);
            }
        }

        private void loadPatientInfo(Patient patient)
        {
            this.firstNameTextBox.Text = patient.FirstName;
            this.lastNameTextBox.Text = patient.LastName;
            this.birthdateDatePicker.SelectedDate = patient.Dob;
            this.addressTextBox.Text = patient.Address.Address1;
            this.phoneNumberTextBox.Text = patient.PhoneNumber;
            this.zipTextBox.Text = patient.Address.Zip;
            this.address2TextBox.Text = patient.Address.Address2;
            this.cityTextBox.Text = patient.Address.City;
            this.stateComboBox.SelectedIndex = (int)StateConverter.ConvertToState(patient.Address.State);
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
                this.confirmButton.IsEnabled = true;
            }
            else
            {
                this.confirmButton.IsEnabled = false;
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

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.validateInput())
            {
                this.updatePatientInfo();
                if (PatientDAL.EditPatient(this.patient))
                {
                    this.NewPatient = this.patient;
                    this.UpdateSuccessful = true;
                    this.Hide();
                }
                else
                {
                    this.UpdateSuccessful = false;
                }
            }
        }

        private void updatePatientInfo()
        {
            this.patient.Address.Address1 = this.addressTextBox.Text;
            this.patient.Address.Address2 = this.address2TextBox.Text;
            this.patient.Address.Zip = this.zipTextBox.Text;
            this.patient.Address.City = this.cityTextBox.Text;
            this.patient.Address.State = this.stateComboBox.SelectionBoxItem.ToString();
            this.patient.Dob = this.birthdateDatePicker.Date.Date;
            this.patient.FirstName = this.firstNameTextBox.Text;
            this.patient.LastName = this.lastNameTextBox.Text;
            this.patient.PhoneNumber = this.phoneNumberTextBox.Text;
        }
    }
}
