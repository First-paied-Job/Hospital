namespace Hospital.Web.ViewModels.Directors
{
    using System.Collections.Generic;

    public class PatientInDepartmentDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public int Payment { get; set; }

        public int DaysStayCount { get; set; }

        public List<IllnessDTO> Illnesses { get; set; }
    }
}
