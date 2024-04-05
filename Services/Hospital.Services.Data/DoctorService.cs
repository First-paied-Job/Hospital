namespace Hospital.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Hospital.Common;
    using Hospital.Data;
    using Hospital.Data.Models;
    using Hospital.Data.Models.Hospitals;
    using Hospital.Data.Models.Hospitals.People;
    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Doctors;
    using Hospital.Web.ViewModels.Doctors.Patient;
    using Hospital.Web.ViewModels.Doctors.Room;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public DoctorService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IndexViewModel> GetDoctorsDepartmentsAsync(string userId)
        {
            var doctor = await this.db.Doctors
                .Include(d => d.Departments)
                .Include(d => d.BossDepartment)
                .FirstOrDefaultAsync(d => d.Id == userId);

            var departments = doctor.Departments
                .Select(d => new DoctorDepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                })
                .ToList();

            DoctorDepartmentDTO bossDepartment = null;

            if (doctor.BossDepartment != null)
            {
                bossDepartment = new DoctorDepartmentDTO
                {
                    DepartmentId = doctor.BossDepartmentId,
                    Name = doctor.BossDepartment.Name,
                };
            }

            return new IndexViewModel()
            {
                Departments = departments,
                BossOfDepartment = bossDepartment,
            };
        }

        #region Room
        public async Task<ICollection<RoomInDepartment>> GetRoomsInDepartment(string departmentId, string doctorId)
        {
            var department = await this.db.Departments
                .Include(d => d.Rooms)
                .ThenInclude(r => r.Patients)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            var rooms = department.Rooms.Select(r => new RoomInDepartment
            {
                RoomId = r.RoomId,
                RoomName = r.Name,
                RoomType = r.RoomType.ToString(),
                Patients = r.Patients.Where(p => p.DoctorId == doctorId)
                .Select(p => new PatientDTO
                {
                    FullName = p.FullName,
                    PatientId = p.Id,
                })
                .ToList(),
            }).ToList();

            return rooms;
        }

        public async Task AddPatientToRoomAsync(AddPatientToRoomInput input)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Email == input.PatientEmail);

            if (user == null)
            {
                throw new ArgumentException("There is not a user with the given email in our system!");
            }

            var room = await this.db.Rooms.FindAsync(input.RoomId);
            var doctor = await this.db.Doctors.FindAsync(input.DoctorId);

            if (room == null)
            {
                throw new ArgumentException("This room does not exist!");
            }

            var patientCheck = await this.db.Patients
                .Include(p => p.Room)
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(d => d.Id == user.Id);

            if (patientCheck != null)
            {
                if (patientCheck.Doctor != null)
                {
                    throw new ArgumentException("This patient is already taken!");
                }

                if (patientCheck.Room != null)
                {
                    throw new ArgumentException("This patient is in another room!");
                }

                patientCheck.FullName = input.FullName;
                patientCheck.Adress = input.Address;
                patientCheck.DaysStayCount = input.DayStayCount;
                patientCheck.PhoneNumber = input.PhoneNumber;

                room.Patients.Add(patientCheck);
                patientCheck.Room = room;
                patientCheck.RoomId = room.RoomId;

                doctor.Patients.Add(patientCheck);
                patientCheck.Doctor = doctor;
                patientCheck.DoctorId = doctor.Id;
                await this.db.SaveChangesAsync();
            }
            else
            {
                var patient = new Patient
                {
                    Id = user.Id,
                    FullName = input.FullName,
                    Adress = input.Address,
                    DaysStayCount = input.DayStayCount,
                    PhoneNumber = input.PhoneNumber,
                    Room = room,
                    RoomId = room.RoomId,
                    Doctor = doctor,
                    DoctorId = doctor.Id,
                };

                room.Patients.Add(patient);
                doctor.Patients.Add(patient);
                user.Patient = patient;

                await this.db.Patients.AddAsync(patient);
                await this.db.SaveChangesAsync();
            }
        }

        public async Task RemovePatientFromRoomAsync(string patientId, string roomId)
        {
            var patient = await this.db.Patients.FindAsync(patientId);

            if (patient.RoomId != roomId)
            {
                throw new ArgumentException("This patient is not in this room!");
            }

            var room = await this.db.Rooms.Include(r => r.Patients).FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (room == null)
            {
                throw new ArgumentException("This room does not exist!");
            }

            room.Patients.Remove(patient);
            this.db.Patients.Remove(patient);
            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Patient

        public async Task<PatientInfoViewModel> GetPatientInfo(string patientId)
        {
            var patient = await this.db.Patients.FindAsync(patientId);

            var ips = await this.db.IllnessPatient.Where(ip => ip.PatientId == patientId).ToListAsync();

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

            return new PatientInfoViewModel
            {
                PatientId = patientId,
                Address = patient.Adress,
                FullName = patient.FullName,
                DayStayCount = patient.DaysStayCount,
                Ilnesses = illnesses,
                PhoneNumber = patient.PhoneNumber,
            };
        }

        public async Task AddIllnesToPatientAsync(AddIlnessToPatientInput input)
        {
            var patient = await this.db.Patients.FindAsync(input.PatientId);

            if (patient == null)
            {
                throw new ArgumentException("This patient does not exist!");
            }

            var illness = new Illness()
            {
                Name = input.IllnessName,
            };

            await this.db.Illnesses.AddAsync(illness);
            await this.db.IllnessPatient.AddAsync(new IllnessPatient
            {
                IllnessId = illness.IllnessId,
                Illness = illness,
                Patient = patient,
                PatientId = patient.Id,
            });

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveIlnessFromPatient(string illnessId, string patientId)
        {
            // var patient = await this.db.Patients.FindAsync(patientId);
            var illness = await this.db.Illnesses.FindAsync(illnessId);

            var ip = await this.db.IllnessPatient
                .FirstOrDefaultAsync(ip => ip.PatientId == patientId & ip.IllnessId == illnessId);

            if (ip == null)
            {
                throw new ArgumentException("This patient does not have this illness!");
            }

            this.db.IllnessPatient.Remove(ip);
            this.db.Illnesses.Remove(illness);
            await this.db.SaveChangesAsync();
        }

        public async Task EditPatientAsync(EditPatientInputModel input)
        {
            var patient = await this.db.Patients.FindAsync(input.PatientId);

            if (patient == null)
            {
                throw new ArgumentException("Patient does not exist!");
            }

            patient.FullName = input.FullName;
            patient.Adress = input.Address;
            patient.PhoneNumber = input.PhoneNumber;
            patient.DaysStayCount = input.DayStayCount;
            await this.db.SaveChangesAsync();
        }

        public async Task<EditPatientViewModel> GetEditPatientAsync(string patientId)
        {
            var patient = await this.db.Patients.FindAsync(patientId);

            if (patient == null)
            {
                throw new ArgumentException("Patient does not exist!");
            }

            return new EditPatientViewModel
            {
                PatientId = patientId,
                FullName = patient.FullName,
                Address = patient.Adress,
                DayStayCount = patient.DaysStayCount,
                PhoneNumber = patient.PhoneNumber,
            };
        }

        #endregion
    }
}
