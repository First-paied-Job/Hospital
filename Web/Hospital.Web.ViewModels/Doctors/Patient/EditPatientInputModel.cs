namespace Hospital.Web.ViewModels.Doctors.Patient
{
    using System.ComponentModel.DataAnnotations;

    public class EditPatientInputModel
    {
        [Required]
        public string PatientId { get; set; }

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
