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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class CreateTestResultContentDialog : ContentDialog
    {
        public CreateTestResultContentDialog()
        {
            this.InitializeComponent();
            this.loadInfo();
        }

        private void loadInfo()
        {
            //TODO: load all info from database that isnt the result.
        }

        private bool validateResults()
        {
            this.resultsRichEditBox.Document.GetText(0, out var reasons);
            reasons = reasons.Trim();
            return reasons != string.Empty;
        }

        private void checkButtonStatus()
        {
            this.confirmButton.IsEnabled = this.validateResults();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: add test result to database
        }

        private void ResultsRichEditBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.checkButtonStatus();
            if (!this.validateResults())
            {
                this.resultsErrorTextBlock.Text = "Must have result.";
                this.resultsErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.resultsErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void ResultsRichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            this.checkButtonStatus();
            sender.Document.GetText(0, out var value);
            if (value != string.Empty)
            {
                this.resultsErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }
    }
}
