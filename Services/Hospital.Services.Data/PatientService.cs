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
            var viewModel = new IndexViewModel();

            // Get patient by id
            var p = await this.db.Patients.FindAsync(userId);

            if (p == null)
            {
                throw new ArgumentException("You are not a patient!");
            }

            // Get room by id
            var room = await this.db.Rooms.FindAsync(p.RoomId);

            if (room == null)
            {
                return viewModel;
            }

            // Get department by id
            var department = await this.db.Departments.FindAsync(room.DepartmentId);

            // Get doctor by id
            var doctor = await this.db.Doctors.FindAsync(p.DoctorId);

            // Get relation between patient and illnesses
            var ips = await this.db.IllnessPatient.Where(ip => ip.PatientId == p.Id).ToListAsync();
            var illnesses = new List<IllnessDTO>();

            foreach (var ip in ips)
            {
                // Get illness by id
                var illness = await this.db.Illnesses.FindAsync(ip.IllnessId);
                illnesses.Add(new IllnessDTO // Convert illness to IllnessDTO
                {
                    Id = illness.IllnessId,
                    Name = illness.Name,
                    CureMethod = illness.CureMethod,
                });
            }

            // Convert patient to Index View Model
            return new IndexViewModel
            {
                PatientId = p.Id,
                Payment = p.DaysStayCount * 10,
                Department = new DepartmentDTO // Convert department to DepartmentDTO
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                },
                Doctor = new DoctorDTO // Convert doctor to DoctorDTO
                {
                    DoctorId = doctor.Id,
                    Name = doctor.FullName,
                    Qulifications = doctor.Qualification
                },
                Room = new RoomDTO // Convert room to RoomDTO
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
