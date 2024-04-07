namespace Hospital.Web.ViewModels.Doctors.Patient
{
    using System.ComponentModel.DataAnnotations;

    public class EditIllnessInput
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CureMethod { get; set; }
    }
}
