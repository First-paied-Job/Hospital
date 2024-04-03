namespace Hospital.Web.ViewModels.Administration.Dashboard.Hospital
{
    using System.Collections.Generic;

    public class HospitalViewModel
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string HospitalId { get; set; }

        public List<DepartmentDTO> Departments { get; set; }
    }
}
