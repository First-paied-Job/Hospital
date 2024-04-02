namespace Hospital.Data.Models.Hospitals
{
    using System;
    using System.Collections.Generic;

    using global::Hospital.Data.Models.Hospitals.People;

    public class Department
    {
        public Department()
        {
            this.DepartmentId = Guid.NewGuid().ToString();
            this.Doctors = new HashSet<Doctor>();
            this.Patients = new HashSet<Patient>();
            this.Rooms = new HashSet<Room>();
        }

        public string DepartmentId { get; set; }

        public string Name { get; set; }

        public string BossId { get; set; }

        public virtual Doctor Boss { get; set; }

        public string HospitalId { get; set; }

        public virtual Hospital Hospital { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
