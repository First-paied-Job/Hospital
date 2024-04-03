namespace Hospital.Web.ViewModels.Administration.Dashboard.Director
{
    using System.ComponentModel.DataAnnotations;

    public class EditDirectorInputModel
    {
        [Required]
        public string DirectorId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [MinLength(2)]
        public string Adress { get; set; }
    }
}
