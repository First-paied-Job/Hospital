namespace Hospital.Data.Models.Hospitals
{
    using System;
    using System.Collections.Generic;

    using global::Hospital.Data.Models.Hospitals.People;

    public class Hospital
    {
        public Hospital()
        {
            this.HospitalId = Guid.NewGuid().ToString();
            this.Departments = new HashSet<Department>();
        }

        public string HospitalId { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string DirectorId { get; set; }

        public virtual Director Director { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
