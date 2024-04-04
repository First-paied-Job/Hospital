namespace Hospital.Web.ViewModels.Doctors.Room
{
    using System.Collections.Generic;

    public class RoomInDepartment
    {
        public string RoomId { get; set; }

        public string RoomName { get; set; }

        public string RoomType { get; set; }

        public List<PatientDTO> Patients { get; set; }
    }
}
