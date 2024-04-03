namespace Hospital.Web.ViewModels.Administration.Dashboard.Director
{
    using System.ComponentModel.DataAnnotations;

    public class AddDirectorInput
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
        public string HospitalName { get; set; }
    }
}
