using System.Collections.Generic;

namespace Hospital.Web.ViewModels.Directors
{
    public class PatientForDoctorDTO
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public int DaysStayCount { get; set; }

        public List<IllnessDTO> Illnesses { get; set; }
    }
}