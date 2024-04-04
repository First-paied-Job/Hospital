namespace Hospital.Web.ViewModels.Doctors
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public DoctorDepartmentDTO BossOfDepartment { get; set; }

        public List<DoctorDepartmentDTO> Departments { get; set; }
    }
}
