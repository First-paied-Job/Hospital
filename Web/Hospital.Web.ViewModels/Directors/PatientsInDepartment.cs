namespace Hospital.Web.ViewModels.Directors
{
    using System.Collections.Generic;

    public class PatientsInDepartment
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<PatientInDepartmentDto> Patients { get; set; }
    }
}
