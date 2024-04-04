namespace Hospital.Web.ViewModels.Doctors.Room
{
    using System.ComponentModel.DataAnnotations;

    public class AddPatientToRoomInput
    {
        [Required]
        public string DoctorId { get; set; }

        [Required]
        public string RoomId { get; set; }

        [Required]
        [EmailAddress]
        public string PatientEmail { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public int DayStayCount { get; set; }
    }
}
