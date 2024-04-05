namespace Hospital.Web.ViewModels.Directors
{
    using System.Collections.Generic;

    public class HospitalDTO
    {
        public string HospitalId { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public List<DepartmentDTO> Departments { get; set; }
    }
}
