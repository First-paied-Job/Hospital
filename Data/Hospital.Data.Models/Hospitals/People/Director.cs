namespace Hospital.Data.Models.Hospitals.People
{
    public class Director
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Adress { get; set; }

        public string HospitalId { get; set; }

        public virtual Hospital Hospital { get; set; }
    }
}
