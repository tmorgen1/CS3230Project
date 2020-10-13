using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using ClinicDatabaseSystem.DAL;
using ClinicDatabaseSystem.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClinicDatabaseSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            PatientDAL.InsertPatient("Doe", "John", DateTime.Now, "1234569999", new Address("some funny address", null, "30222", "newnan", "Georgia"), "30222");
            var patients = PatientDAL.GetPatients();
        }
    }
}
