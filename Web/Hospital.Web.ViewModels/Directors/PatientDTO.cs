namespace Hospital.Web.ViewModels.Directors
{
    using System.Collections.Generic;

    public class PatientDTO
    {
        public string PatientId { get; set; }

        public string FullName { get; set; }

        public int DaysStayCount { get; set; }
    }
}
