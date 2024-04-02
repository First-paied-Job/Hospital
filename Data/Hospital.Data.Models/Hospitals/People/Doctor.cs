namespace Hospital.Data.Models.Hospitals.People
{
    using System.Collections.Generic;

    public class Doctor
    {
        public Doctor()
        {
            this.Patients = new HashSet<Patient>();
        }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string Adress { get; set; }

        public string Qualification { get; set; }

        public string DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
