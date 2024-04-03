namespace Hospital.Web.ViewModels.Administration.Dashboard.Doctor
{
    using System.ComponentModel.DataAnnotations;

    public class DoctorViewModel
    {
        public string DoctorId { get; set; }

        public string FullName { get; set; }

        public string Adress { get; set; }

        public string Qualification { get; set; }
    }
}
