namespace Hospital.Web.ViewModels.Administration.Dashboard.Department
{
    using System.Collections.Generic;

    public class DepartmentViewModel
    {
        public string HospitalId { get; set; }

        public string DepartmentId { get; set; }

        public string Name { get; set; }

        public List<DoctorDTO> Doctors { get; set; }

        public List<RoomDTO> Rooms { get; set; }
    }
}
