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
    public sealed partial class CreateVisitInfoContentDialog : ContentDialog
    {
        public CreateVisitInfoContentDialog()
        {
            this.InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void initialDiagnosisRichEditBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void symptomsRichEditBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void pluseTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void bodyTempTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void diastolicBpTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void systolicBpTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void SymptomsRichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            
        }

        private void InitialDiagnosisRichEditBox_OnTextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {

        }
    }
}
