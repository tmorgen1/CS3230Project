using ClinicDatabaseSystem.Controller;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// Handles taking visit information.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CreateVisitInfoContentDialog : ContentDialog
    {
        private Appointment visitInfoAppointment;
        private IList<string> orderedTests;
        private readonly VisitInformationController visitInformationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVisitInfoContentDialog"/> class.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="visitInfoController">The visit information controller.</param>
        public CreateVisitInfoContentDialog(Appointment appointment, VisitInformationController visitInfoController)
        {
            this.InitializeComponent();
            this.visitInfoAppointment = appointment;
            this.visitInformationController = visitInfoController;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVisitInfoContentDialog"/> class.
        /// </summary>
        /// <param name="visitInformation">The visit information.</param>
        /// <param name="appointment">The appointment.</param>
        /// <param name="orderedTests">The ordered tests.</param>
        /// <param name="visitInfoController">The visit information controller.</param>
        public CreateVisitInfoContentDialog(VisitInformation visitInformation, Appointment appointment, IList<string> orderedTests, VisitInformationController visitInfoController)
        {
            this.InitializeComponent();
            this.visitInformationController = visitInfoController;
            this.loadPartialVisitInfo(visitInformation, appointment, orderedTests);
        }

        private void loadPartialVisitInfo(VisitInformation visitInformation, Appointment appointment, IList<string> orderedTests)
        {
            this.visitInfoAppointment = appointment;
            this.orderedTests = orderedTests;
            this.systolicBpTextBox.Text = visitInformation.SystolicBp;
            this.diastolicBpTextBox.Text = visitInformation.DiastolicBp;
            this.bodyTempTextBox.Text = visitInformation.BodyTemp;
            this.pulseTextBox.Text = visitInformation.Pulse;
            this.weightTextBox.Text = visitInformation.Weight;
            this.symptomsRichEditBox.Document.SetText(TextSetOptions.None, visitInformation.Symptoms);
            this.initialDiagnosisRichEditBox.Document.SetText(TextSetOptions.None, visitInformation.InitialDiagnosis);
        }

        private bool validateInput()
        {
            return this.validateInitialDiagnosis() && this.validateSymptoms() && this.validatePulse() &&
                   this.validateBodyTemp() && this.validateDiastolicBp() && this.validateSystolicBp() && this.validateWeight();
        }

        private void checkButtonStatus()
        {
            this.createButton.IsEnabled = this.validateInput();
        }

        private bool validateWeight()
        {
            return this.weightTextBox.Text != string.Empty;
        }

        private bool validateSystolicBp()
        {
            return this.systolicBpTextBox.Text != string.Empty;
        }

        private bool validateDiastolicBp()
        {
            return this.diastolicBpTextBox.Text != string.Empty;
        }

        private bool validateBodyTemp()
        {
            var pattern = @"[0-9]+\.[0-9]{2}";
            var regex = new Regex(pattern);
            return regex.IsMatch(this.bodyTempTextBox.Text) && this.bodyTempTextBox.Text != string.Empty;
        }

        private bool validatePulse()
        {
            return this.pulseTextBox.Text != string.Empty;
        }

        private bool validateSymptoms()
        {
            this.symptomsRichEditBox.Document.GetText(0, out var reasons);
            reasons = reasons.Trim();
            return reasons != string.Empty;
        }

        private bool validateInitialDiagnosis()
        {
            this.initialDiagnosisRichEditBox.Document.GetText(0, out var reasons);
            reasons = reasons.Trim();
            return reasons != string.Empty;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.validateInput())
            {
                if (this.insertVisitInfo() && this.insertOrderedTests())
                {
                    this.Hide();
                    this.visitInformationController.CreatedVisitInfo = true;
                }
            }
        }

        private bool insertVisitInfo()
        {
            var patientId = this.visitInfoAppointment.PatientId;
            var date = this.visitInfoAppointment.ScheduledDate;
            this.symptomsRichEditBox.Document.GetText(0, out var symptoms);
            this.initialDiagnosisRichEditBox.Document.GetText(0, out var diagnosis);
            var visitInformation = new VisitInformation(patientId, date,
                this.systolicBpTextBox.Text, this.diastolicBpTextBox.Text, this.bodyTempTextBox.Text,
                this.pulseTextBox.Text, this.weightTextBox.Text, symptoms, diagnosis, null);
            return VisitInformationDAL.InsertVisitInfo(visitInformation);
        }

        private bool insertOrderedTests()
        {
            if (this.orderedTests == null || this.orderedTests.Count == 0)
            {
                return true;
            }
            foreach (var currentTest in this.orderedTests)
            {
                var testId = currentTest.ToString().Split(':')[0].Trim();
                var testResult = new TestResult(Int32.Parse(testId), this.visitInfoAppointment.PatientId, this.visitInfoAppointment.ScheduledDate, string.Empty, false);
                if (!TestResultDAL.InsertTestResult(testResult))
                {
                    return false;
                }
            }
            return true;
        }

        private void initialDiagnosisRichEditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateInitialDiagnosis())
            {
                this.initialDiagnosisErrorTextBlock.Text = "Must have initial diagnosis";
                this.initialDiagnosisErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.initialDiagnosisErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void symptomsRichEditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateSymptoms())
            {
                this.symptomsErrorTextBlock.Text = "Must have symptoms";
                this.symptomsErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.symptomsErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void pluseTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validatePulse())
            {
                this.pulseErrorTextBlock.Text = "Must have pulse";
                this.pulseErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.pulseErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void WeightTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (this.weightTextBox.Text != string.Empty)
            {
                var temp = this.weightTextBox.Text;
                temp = $"{Convert.ToDouble(temp):0.00}";
                this.weightTextBox.Text = temp;
            }

            if (!this.validateWeight())
            {
                this.weightErrorTextBlock.Text = "Invalid Weight. E.g. 170.54";
                this.weightErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.weightErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void bodyTempTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (this.bodyTempTextBox.Text != string.Empty)
            {
                var temp = this.bodyTempTextBox.Text;
                temp = $"{Convert.ToDouble(temp):0.00}";
                this.bodyTempTextBox.Text = temp;
            }
            if (!this.validateBodyTemp())
            {
                this.bodyTempErrorTextBlock.Text = "Invalid body temp. E.g. 98.60";
                this.bodyTempErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.bodyTempErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void diastolicBpTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateDiastolicBp())
            {
                this.diastolicBpErrorTextBlock.Text = "Must have diastolic BP";
                this.diastolicBpErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.diastolicBpErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void systolicBpTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateSystolicBp())
            {
                this.systolicBpErrorTextBlock.Text = "Must have systolic BP";
                this.systolicBpErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.systolicBpErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void SymptomsRichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            if (this.validateSymptoms())
            {
                this.symptomsErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.symptomsErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void InitialDiagnosisRichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            if (this.validateInitialDiagnosis())
            {
                this.initialDiagnosisErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.initialDiagnosisErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void SystolicBpTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            if (this.systolicBpErrorTextBlock != null)
            {
                if (args.NewText.Any(c => !char.IsDigit(c)))
                {
                    args.Cancel = true;
                    this.systolicBpErrorTextBlock.Text = "Only Digits allowed.";
                    this.systolicBpErrorTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    this.systolicBpErrorTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void DiastolicBpTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            if (this.diastolicBpErrorTextBlock != null)
            {
                if (args.NewText.Any(c => !char.IsDigit(c)))
                {
                    args.Cancel = true;
                    this.diastolicBpErrorTextBlock.Text = "Only Digits allowed.";
                    this.diastolicBpErrorTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    this.diastolicBpErrorTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BodyTempTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            var result = 0.0;
            if (!Double.TryParse(args.NewText, out result) && args.NewText != string.Empty)
            {
                args.Cancel = true;
                this.bodyTempErrorTextBlock.Text = "Invalid format. E.g 98.60";
                this.bodyTempErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.bodyTempErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void WeightTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            var result = 0.0;
            if (!Double.TryParse(args.NewText, out result) && args.NewText != string.Empty)
            {
                args.Cancel = true;
                this.weightErrorTextBlock.Text = "Invalid format. E.g 170.56";
                this.weightErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.weightErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void PluseTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            if (this.pulseErrorTextBlock != null)
            {
                if (args.NewText.Any(c => !char.IsDigit(c)))
                {
                    args.Cancel = true;
                    this.pulseErrorTextBlock.Text = "Only Digits allowed.";
                    this.pulseErrorTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    this.pulseErrorTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void orderTestsButton_Click(object sender, RoutedEventArgs e)
        {
            var patientId = this.visitInfoAppointment.PatientId;
            var date = this.visitInfoAppointment.ScheduledDate;
            this.symptomsRichEditBox.Document.GetText(0, out var symptoms);
            this.initialDiagnosisRichEditBox.Document.GetText(0, out var diagnosis);
            var visitInfo = new VisitInformation(patientId, date,
                this.systolicBpTextBox.Text, this.diastolicBpTextBox.Text, this.bodyTempTextBox.Text,
                this.pulseTextBox.Text, this.weightTextBox.Text, symptoms, diagnosis, null);
            this.Hide();
            OrderTestContentDialog orderTestContentDialog = new OrderTestContentDialog(visitInfo, this.visitInfoAppointment, this.visitInformationController);
            await orderTestContentDialog.ShowAsync();
        }
    }
}
