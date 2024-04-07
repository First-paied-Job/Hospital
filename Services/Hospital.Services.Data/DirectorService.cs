namespace Hospital.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Hospital.Data;
    using Hospital.Data.Models.Hospitals.People;
    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Directors;
    using Hospital.Web.ViewModels.Directors.Schelude;
    using Microsoft.EntityFrameworkCore;

    public class DirectorService : IDirectorService
    {
        private readonly ApplicationDbContext db;

        public DirectorService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task ChangeScheludeForDoctor(ScheludeInputModel input)
        {
            // Get doctor by id
            var doctor = await this.db.Doctors.FirstOrDefaultAsync(d => d.Id == input.Id);

            // Update shift info
            doctor.Shift = input.Shift;
            doctor.DateForWork = input.Date;

            await this.db.SaveChangesAsync();
        }

        public async Task<IndexViewModel> GetDirectorInfo(string userId)
        {
            var viewModel = new IndexViewModel();

            // Get director by id
            var director = await this.db.Directors.FindAsync(userId);

            if (director == null)
            {
                throw new ArgumentException("You are not a director!");
            }

            viewModel.DirectorId = director.Id;

            // Get hospital by id
            var hospital = await this.db.Hospitals.FindAsync(director.HospitalId);

            if (hospital == null)
            {
                return viewModel;
            }

            // Convert hospital to HospitalDTO
            viewModel.Hospital = new HospitalDTO
            {
                HospitalId = hospital.HospitalId,
                Name = hospital.Name,
                Location = hospital.Location,
                Departments = new List<DepartmentDTO>(),
            };

            // Get departments including children by hospital id
            var departments = await this.db.Departments
                .Include(d => d.Doctors)
                .Include(d => d.Rooms)
                .ThenInclude(r => r.Patients)
                .Where(d => d.HospitalId == hospital.HospitalId)
                .ToListAsync();

            foreach (var department in departments)
            {
                var departmentDto = new DepartmentDTO // Covert to DepartmentDTO
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                };

                // Convert rooms to List of RoomDTO
                departmentDto.Rooms = department.Rooms.Select(r => new RoomDTO
                {
                    RoomId = r.RoomId,
                    RoomName = r.Name,
                    RoomType = r.RoomType.ToString(),
                    Patients = r.Patients.Select(p => new PatientDTO
                    {
                        PatientId = p.Id,
                        FullName = p.FullName,
                        DaysStayCount = p.DaysStayCount,
                    }).ToList(),
                }).ToList();

                // Convert doctors to List of DoctorDTO
                departmentDto.Doctors = department.Doctors.Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    Name = d.FullName,
                    Qualifications = d.Qualification,
                }).ToList();

                viewModel.Hospital.Departments.Add(departmentDto);
            }

            return viewModel;
        }

        public async Task<PatientForDoctor> GetPatientsForDoctor(string doctorId)
        {
            // Get doctor by id including children
            var doctor = await this.db.Doctors.Include(d => d.Patients).FirstOrDefaultAsync(d => d.Id == doctorId);

            var viewModel = new PatientForDoctor // Convert doctor to Patient For Doctor
            {
                Name = doctor.FullName,
                Address = doctor.Adress,
                Qualifications = doctor.Qualification,
                Patients = doctor.Patients.Select(p => new PatientForDoctorDTO // Convert patients to Patient For Doctor DTO
                {
                    FullName = p.FullName,
                    Address = p.Adress,
                    DaysStayCount = p.DaysStayCount,
                    Id = p.Id,
                    Illnesses = this.db.IllnessPatient
                        .Include(ip => ip.Illness)
                        .Where(ip => ip.PatientId == p.Id)
                        .Select(ip => new IllnessDTO // Convert Illness to IllnessDTO
                        {
                            Id = ip.Illness.IllnessId,
                            Name = ip.Illness.Name,
                        })
                        .ToList(),
                }).ToList(),
            };

            return viewModel;
        }

        public async Task<PatientsInDepartment> GetPatientsInDepartment(string departmentId)
        {
            // Get department by id including children
            var department = await this.db.Departments
                .Include(d => d.Rooms)
                .ThenInclude(r => r.Patients)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            var viewModel = new PatientsInDepartment
            {
                Name = department.Name,
                Patients = new List<PatientInDepartmentDto>(),
            };

            // Get patients in every room
            foreach (var room in department.Rooms)
            {
                var patients = room.Patients
                    .Select(p => new PatientInDepartmentDto // Convert patient to Patient In Department Dto
                    {
                        FullName = p.FullName,
                        Address = p.Adress,
                        DaysStayCount = p.DaysStayCount,
                        Id = p.Id,
                        Payment = p.DaysStayCount * 10,
                        Illnesses = this.db.IllnessPatient
                            .Include(ip => ip.Illness)
                            .Where(ip => ip.PatientId == p.Id)
                            .Select(ip => new IllnessDTO // Convert illness to IllnessDTO
                            {
                                Id = ip.Illness.IllnessId,
                                Name = ip.Illness.Name,
                            })
                            .ToList(),
                    }).ToList();

                viewModel.Patients.AddRange(patients);
            }

            return viewModel;
        }

        public async Task<Schelude> SetScheludeForDoctors(string departmentId)
        {
            // Get department by id including children
            var department = await this.db.Departments
                .Include(d => d.Doctors)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            return new Schelude // Create schelude view model
            {
                Doctors = department.Doctors.Select(d => new DoctorScheludeDTO // Convert doctors to Doctor Schelude DTO
                {
                    DoctorId = d.Id,
                    DoctorName = d.FullName,
                    Date = d.DateForWork.Year < 2000 ? "NO DATE" : d.DateForWork.ToString("d MMM yyyy"),
                    Shift = d.Shift == null ? "NO SHIFT" : d.Shift,
                }).ToList(),
            };
        }
    }
}
