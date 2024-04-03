namespace Hospital.Web.ViewModels.Administration.Dashboard.Hospital
{
    using System.ComponentModel.DataAnnotations;

    public class HospitalInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
