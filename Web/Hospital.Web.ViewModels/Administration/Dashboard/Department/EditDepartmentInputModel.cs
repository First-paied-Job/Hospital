namespace Hospital.Web.ViewModels.Administration.Dashboard.Department
{
    using System.ComponentModel.DataAnnotations;

    public class EditDepartmentInputModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string DepartmentId { get; set; }

        [Required]
        public string HospitalId { get; set; }
    }
}
