namespace Hospital.Web.ViewModels.Doctors.Patient
{
    using System.Collections.Generic;

    public class PatientInfoViewModel
    {
        public string PatientId { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public int DayStayCount { get; set; }

        public List<IllnessDTO> Ilnesses { get; set; }
    }
}
