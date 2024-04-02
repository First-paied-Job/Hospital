namespace Hospital.Data.Models.Hospitals.People
{
    using System.Collections.Generic;

    public class Patient
    {
        public Patient()
        {
            this.IllnessPatient = new HashSet<IllnessPatient>();
        }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string Adress { get; set; }

        public string PhoneNumber { get; set; }

        public int DaysStayCount { get; set; }

        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual ICollection<IllnessPatient> IllnessPatient { get; set; }

        public string RoomId { get; set; }

        public Room Room { get; set; }
    }
}
