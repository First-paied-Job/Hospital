namespace Hospital.Web.ViewModels.Patients
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public string PatientId { get; set; }

        public int Payment { get; set; }

        public DoctorDTO Doctor { get; set; }

        public DepartmentDTO Department { get; set; }

        public RoomDTO Room { get; set; }

        public List<IllnessDTO> Illnesses { get; set; }
    }
}
