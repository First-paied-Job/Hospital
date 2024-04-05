namespace Hospital.Web.ViewModels.Administration.Dashboard.Statistics
{
    using System.Collections.Generic;

    public class DoctorsViewModel
    {
        public string Name { get; set; }

        public List<PatientDTO> Patients { get; set; }
    }
}
