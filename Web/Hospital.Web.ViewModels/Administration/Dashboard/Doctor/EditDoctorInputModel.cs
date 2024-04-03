namespace Hospital.Web.ViewModels.Administration.Dashboard.Doctor
{
    using System.ComponentModel.DataAnnotations;

    public class EditDoctorInputModel
    {
        public string DoctorId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [MinLength(2)]
        public string Adress { get; set; }

        [Required]
        public string Qualification { get; set; }
    }
}
