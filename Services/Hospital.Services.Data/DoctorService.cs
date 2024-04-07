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
            // Get doctor by id including children
            var doctor = await this.db.Doctors
                .Include(d => d.Departments)
                .Include(d => d.BossDepartment)
                .FirstOrDefaultAsync(d => d.Id == userId);

            if (doctor == null)
            {
                return new IndexViewModel()
                {
                    BossOfDepartment = null,
                    Departments = new List<DoctorDepartmentDTO>(),
                };
            }

            // Convert to Doctor Department DTO
            var departments = doctor.Departments
                .Select(d => new DoctorDepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                })
                .ToList();

            DoctorDepartmentDTO bossDepartment = null;

            // Set if boss of any departments
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
            // Get department by id including children
            var department = await this.db.Departments
                .Include(d => d.Rooms)
                .ThenInclude(r => r.Patients)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            // Convert tooms to RoomInDepartment
            var rooms = department.Rooms.Select(r => new RoomInDepartment
            {
                RoomId = r.RoomId,
                RoomName = r.Name,
                RoomType = r.RoomType.ToString(),
                Patients = r.Patients.Where(p => p.DoctorId == doctorId)
                .Select(p => new PatientDTO // Convert patient to PatientDTO
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
            // Get user by email
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Email == input.PatientEmail);

            if (user == null)
            {
                throw new ArgumentException("There is not a user with the given email in our system!");
            }

            // Get room by id
            var room = await this.db.Rooms.FindAsync(input.RoomId);

            // Get doctor by id
            var doctor = await this.db.Doctors.FindAsync(input.DoctorId);

            if (room == null)
            {
                throw new ArgumentException("This room does not exist!");
            }
            // Check if patient is in room
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

                // Set patient info
                patientCheck.FullName = input.FullName;
                patientCheck.Adress = input.Address;
                patientCheck.DaysStayCount = input.DayStayCount;
                patientCheck.PhoneNumber = input.PhoneNumber;

                // Add patient to room
                room.Patients.Add(patientCheck);
                patientCheck.Room = room;
                patientCheck.RoomId = room.RoomId;

                // Add patient to doctor
                doctor.Patients.Add(patientCheck);
                patientCheck.Doctor = doctor;
                patientCheck.DoctorId = doctor.Id;
                await this.db.SaveChangesAsync();
            }
            else
            {
                // Create patient
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

                // Add pateint to room and doctor
                room.Patients.Add(patient);
                doctor.Patients.Add(patient);
                user.Patient = patient;

                // Add patient to db
                await this.db.Patients.AddAsync(patient);
                await this.db.SaveChangesAsync();
            }
        }

        public async Task RemovePatientFromRoomAsync(string patientId, string roomId)
        {
            // Get patient by id
            var patient = await this.db.Patients.FindAsync(patientId);

            if (patient.RoomId != roomId)
            {
                throw new ArgumentException("This patient is not in this room!");
            }

            // Get room by id
            var room = await this.db.Rooms.Include(r => r.Patients).FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (room == null)
            {
                throw new ArgumentException("This room does not exist!");
            }

            // Remove patient from room and db
            room.Patients.Remove(patient);
            this.db.Patients.Remove(patient);
            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Patient

        public async Task<PatientInfoViewModel> GetPatientInfo(string patientId)
        {
            // Get patient by id
            var patient = await this.db.Patients.FindAsync(patientId);

            // Get patient illnesses
            var ips = await this.db.IllnessPatient.Where(ip => ip.PatientId == patientId).ToListAsync();

            var illnesses = new List<IllnessDTO>();

            foreach (var ip in ips)
            {
                // Get illnesses by id
                var illness = await this.db.Illnesses.FindAsync(ip.IllnessId);

                illnesses.Add(new IllnessDTO // Convert to IllnessDTO
                {
                    Id = illness.IllnessId,
                    Name = illness.Name,
                    CureMethod = illness.CureMethod,
                });
            }

            // Convert patient to Patient Info View Model
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
            // Get patient by id
            var patient = await this.db.Patients.FindAsync(input.PatientId);

            if (patient == null)
            {
                throw new ArgumentException("This patient does not exist!");
            }

            var illness = new Illness()
            {
                Name = input.IllnessName,
                CureMethod = input.CureMethod,
            };

            // Add illness to db
            await this.db.Illnesses.AddAsync(illness);
            await this.db.IllnessPatient.AddAsync(new IllnessPatient // Relation many to many
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
            // Get illness by id
            var illness = await this.db.Illnesses.FindAsync(illnessId);

            // Get realtion between patient and illness
            var ip = await this.db.IllnessPatient
                .FirstOrDefaultAsync(ip => ip.PatientId == patientId & ip.IllnessId == illnessId);

            if (ip == null)
            {
                throw new ArgumentException("This patient does not have this illness!");
            }

            // Remove relation between illness and patient
            this.db.IllnessPatient.Remove(ip);
            this.db.Illnesses.Remove(illness);
            await this.db.SaveChangesAsync();
        }

        public async Task EditPatientAsync(EditPatientInputModel input)
        {
            // Get patient by id
            var patient = await this.db.Patients.FindAsync(input.PatientId);

            if (patient == null)
            {
                throw new ArgumentException("Patient does not exist!");
            }

            // Update patient info
            patient.FullName = input.FullName;
            patient.Adress = input.Address;
            patient.PhoneNumber = input.PhoneNumber;
            patient.DaysStayCount = input.DayStayCount;
            await this.db.SaveChangesAsync();
        }

        public async Task<EditPatientViewModel> GetEditPatientAsync(string patientId)
        {
            // Get patient by id
            var patient = await this.db.Patients.FindAsync(patientId);

            if (patient == null)
            {
                throw new ArgumentException("Patient does not exist!");
            }

            // Convert to Edit Patient View Model
            return new EditPatientViewModel
            {
                PatientId = patientId,
                FullName = patient.FullName,
                Address = patient.Adress,
                DayStayCount = patient.DaysStayCount,
                PhoneNumber = patient.PhoneNumber,
            };
        }

        public async Task EditIllnessAsync(EditIllnessInput input)
        {
            var illness = await this.db.Illnesses.FindAsync(input.Id);

            if (illness == null)
            {
                throw new ArgumentException("Illness does not exist!");
            }

            illness.Name = input.Name;
            illness.CureMethod = input.CureMethod;
            await this.db.SaveChangesAsync();
        }

        public async Task<EditIllnessViewModel> GetEditIllnessAsync(string illnessId)
        {
            var illness = await this.db.Illnesses.FindAsync(illnessId);

            if (illness == null)
            {
                throw new ArgumentException("Illness does not exist");
            }

            return new EditIllnessViewModel
            {
                Id = illnessId,
                CureMethod = illness.CureMethod,
                Name = illness.Name,
            };
        }

        #endregion
    }
}
