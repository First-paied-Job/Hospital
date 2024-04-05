namespace Hospital.Web.ViewModels.Directors
{
    using System.Collections.Generic;

    public class DepartmentDTO
    {
        public string DepartmentId { get; set; }

        public string Name { get; set; }

        public List<RoomDTO> Rooms { get; set; }

        public List<DoctorDTO> Doctors { get; set; }
    }
}
