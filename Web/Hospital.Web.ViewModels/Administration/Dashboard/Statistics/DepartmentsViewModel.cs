using System.Collections.Generic;

namespace Hospital.Web.ViewModels.Administration.Dashboard.Statistics
{
    public class DepartmentsViewModel
    {
        public string Name { get; set; }

        public List<DoctorInDepartmentDTO> Doctors { get; set; }
    }
}
