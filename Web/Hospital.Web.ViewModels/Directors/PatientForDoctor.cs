using System.Collections.Generic;

namespace Hospital.Web.ViewModels.Directors
{
    public class PatientForDoctor
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Qualifications { get; set; }

        public List<PatientForDoctorDTO> Patients { get; set; }
    }
}
