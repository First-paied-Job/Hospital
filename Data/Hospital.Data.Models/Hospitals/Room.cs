namespace Hospital.Data.Models.Hospitals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using global::Hospital.Data.Models.Hospitals.People;

    public class Room
    {
        public Room()
        {
            this.RoomId = Guid.NewGuid().ToString();
            this.Patients = new HashSet<Patient>();
        }

        public string RoomId { get; set; }

        public string Name { get; set; }

        public virtual int RoomTypeId { get; set; }

        [EnumDataType(typeof(RoomType))]
        public RoomType RoomType
        {
            get
            {
                return (RoomType)this.RoomTypeId;
            }

            set
            {
                this.RoomTypeId = (int)value;
            }
        }

        public string DepartmentId { get; set; }

        public Department Department { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
