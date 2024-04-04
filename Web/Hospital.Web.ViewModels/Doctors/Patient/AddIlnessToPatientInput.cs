namespace Hospital.Web.ViewModels.Doctors.Patient
{
    using System.ComponentModel.DataAnnotations;

    public class AddIlnessToPatientInput
    {
        [Required]
        public string PatientId { get; set; }

        [Required]
        public string IllnessName { get; set; }
    }
}
