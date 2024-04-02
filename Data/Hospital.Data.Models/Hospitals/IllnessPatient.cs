using Hospital.Data.Models.Hospitals.People;

namespace Hospital.Data.Models.Hospitals
{
    public class IllnessPatient
    {
        public string PatientId { get; set; }

        public Patient Patient { get; set; }

        public string IllnessId { get; set; }

        public Illness Illness { get; set; }
    }
}
