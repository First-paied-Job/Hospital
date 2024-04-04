namespace Hospital.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Hospital.Data;
    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Patients;
    using Microsoft.EntityFrameworkCore;

    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext db;

        public PatientService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IndexViewModel> GetInformationForPatient(string userId)
        {
            var p = await this.db.Patients.FindAsync(userId);

            if (p == null)
            {
                throw new ArgumentException("You are not a patient!");
            }

            var room = await this.db.Rooms.FindAsync(p.RoomId);

            var department = await this.db.Departments.FindAsync(room.DepartmentId);

            var doctor = await this.db.Doctors.FindAsync(p.DoctorId);

            var ips = await this.db.IllnessPatient.Where(ip => ip.PatientId == p.Id).ToListAsync();
            var illnesses = new List<IllnessDTO>();

            foreach (var ip in ips)
            {
                var illness = await this.db.Illnesses.FindAsync(ip.IllnessId);
                illnesses.Add(new IllnessDTO
                {
                    Id = illness.IllnessId,
                    Name = illness.Name,
                });
            }

            return new IndexViewModel
            {
                PatientId = p.Id,
                Department = new DepartmentDTO
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                },
                Doctor = new DoctorDTO
                {
                    DoctorId = doctor.Id,
                    Name = doctor.FullName,
                    Qulifications = doctor.Qualification
                },
                Room = new RoomDTO
                {
                    RoomId = room.RoomId,
                    RoomName = room.Name,
                    RoomType = room.RoomType.ToString(),
                },
                Illnesses = illnesses,
            };
        }
    }
}
