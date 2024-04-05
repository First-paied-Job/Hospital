namespace Hospital.Data.Models.Hospitals.People
{
    using System;
    using System.Collections.Generic;

    public class Doctor
    {
        public Doctor()
        {
            this.Patients = new HashSet<Patient>();
            this.Departments = new HashSet<Department>();
        }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string Adress { get; set; }

        public string Qualification { get; set; }

        public string Shift { get; set; }

        public DateTime DateForWork { get; set; }

        public string? BossDepartmentId { get; set; }

        public virtual Department BossDepartment { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
