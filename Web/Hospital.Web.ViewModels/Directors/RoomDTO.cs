namespace Hospital.Web.ViewModels.Directors
{
    using System.Collections.Generic;

    public class RoomDTO
    {
        public string RoomId { get; set; }

        public string RoomName { get; set; }

        public string RoomType { get; set; }

        public List<PatientDTO> Patients { get; set; }
    }
}
