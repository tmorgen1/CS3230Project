using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;
using ClinicDatabaseSystem.ViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClinicDatabaseSystem.View
{
    public sealed partial class ViewVisitInfoContentDialog : ContentDialog
    {

        private AppointmentNameInfo appointmentNameInfo;

        public ViewVisitInfoContentDialog(AppointmentNameInfo appointmentNameInfo)
        {
            this.InitializeComponent();
            this.appointmentNameInfo = appointmentNameInfo;
            this.loadVistInfo();
        }

        private void loadVistInfo()
        {
            this.patientIdTextBox.Text = this.appointmentNameInfo.Appointment.PatientId.ToString();
            this.dateTextBox.Text = this.appointmentNameInfo.Appointment.ScheduledDate.ToString();
            
            var visitInfo = VisitInformationDAL.GetVisitInfoFromAppointment(this.appointmentNameInfo.Appointment)[0];
            this.systolicBpTextBox.Text = visitInfo.SystolicBp;
            this.diastolicBpTextBox.Text = visitInfo.DiastolicBp;
            this.bodyTempTextBox.Text = visitInfo.BodyTemp;
            this.pulseTextBox.Text = visitInfo.Pulse;
            
            this.symptomsRichEditBox.Document.SetText(TextSetOptions.None, visitInfo.Symptoms);
            this.symptomsRichEditBox.IsReadOnly = true;
            
            this.initialDiagnosisRichEditBox.Document.SetText(TextSetOptions.None, visitInfo.InitialDiagnosis);
            this.initialDiagnosisRichEditBox.IsReadOnly = true;
            
            if (visitInfo.FinalDiagnosis != null)
            {
                this.finalDiagnosisRichEditBox.Document.SetText(TextSetOptions.None, visitInfo.FinalDiagnosis);
            }
            this.finalDiagnosisRichEditBox.IsReadOnly = true;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
