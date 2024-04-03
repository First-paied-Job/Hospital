namespace Hospital.Web.ViewModels.Administration.Dashboard.Doctor
{
    using System.ComponentModel.DataAnnotations;

    public class AddDoctorInput
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [MinLength(2)]
        public string Adress { get; set; }

        [Required]
        public string Qualification { get; set; }
    }
}
