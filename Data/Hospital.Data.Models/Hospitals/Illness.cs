namespace Hospital.Data.Models.Hospitals
{
    using System;

    public class Illness
    {
        public Illness()
        {
            this.IllnessId = Guid.NewGuid().ToString();
        }

        public string IllnessId { get; set; }

        public string Name { get; set; }

        public string CureMethod { get; set; }
    }
}
