namespace Hospital.Web.ViewModels.Administration.Dashboard.Department
{
    using System.ComponentModel.DataAnnotations;

    public class AddDoctorToDepartmentInput
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string DepartmentId { get; set; }
    }
}
