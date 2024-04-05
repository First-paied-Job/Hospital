namespace Hospital.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;
    using Hospital.Common;
    using Hospital.Data;
    using Hospital.Data.Models;
    using Hospital.Data.Models.Hospitals;
    using Hospital.Data.Models.Hospitals.People;
    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Administration.Dashboard.Department;
    using Hospital.Web.ViewModels.Administration.Dashboard.Director;
    using Hospital.Web.ViewModels.Administration.Dashboard.Doctor;
    using Hospital.Web.ViewModels.Administration.Dashboard.Hospital;
    using Hospital.Web.ViewModels.Administration.Dashboard.Room;
    using Hospital.Web.ViewModels.Administration.Dashboard.Statistics;
    using Hospital.Web.ViewModels.Administration.Dashboard.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AdministrationService(
                ApplicationDbContext db,
                UserManager<ApplicationUser> userManager,
                RoleManager<ApplicationRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #region User
        public async Task AddRoleToUser(string roleId, string userId)
        {
            var user = this.db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("userId", "There is no user with the given userId!");
            }

            var role = this.db.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                throw new InvalidOperationException($"There is no role with the given roleId!");
            }

            var userRole = new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = user.Id,
            };

            user.Roles.Add(userRole);

            this.db.Users.Update(user);
            await this.db.UserRoles.AddAsync(userRole);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<UserRoleViewModel>> GetAllUsers()
        {
            var viewModel = new List<UserRoleViewModel>();

            var roles = await this.db.Roles.ToListAsync();
            var users = await this.db.Users.Include(u => u.Roles).ToListAsync();

            foreach (var user in users)
            {
                var userRolesNames = await this.userManager.GetRolesAsync(user);

                var userRoles = roles.Where(r => userRolesNames.Contains(r.Name) == true)
                    .Select(r => new RoleDTO()
                    {
                        RoleId = r.Id,
                        Name = r.Name,
                    })
                    .ToList();

                var availableRoles = roles
                    .Where(r => userRolesNames.Contains(r.Name) == false)
                    .Select(r => new RoleDTO()
                    {
                        RoleId = r.Id,
                        Name = r.Name,
                    })
                    .ToList();

                var userModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    Roles = userRoles,
                    AvailableRoles = availableRoles,
                };

                viewModel.Add(userModel);
            }

            return viewModel;
        }

        public async Task RemoveRoleFromUser(string roleId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId", "The given userId is invalid!");
            }

            var user = this.db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("userId", "There is no user with the given userId!");
            }

            var role = this.db.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                throw new InvalidOperationException($"There is no role with the given roleId!");
            }

            this.db.UserRoles.Remove(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = user.Id,
            });

            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Doctor
        public async Task AddDoctorAsync(AddDoctorInput input)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Email == input.UserEmail);

            if (user == null)
            {
                throw new ArgumentException("There is not a user with the given email in our system!");
            }

            var check = await this.db.Doctors.FirstOrDefaultAsync(d => d.FullName == input.FullName);

            if (check != null)
            {
                throw new ArgumentException("There is already a doctor with this name!");
            }

            var doctor = new Doctor
            {
                Id = user.Id,
                FullName = input.FullName,
                Qualification = input.Qualification,
                Adress = input.Adress,
            };

            user.Doctor = doctor;

            await this.db.Doctors.AddAsync(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDoctorAsync(string doctorId)
        {
            var doctor = await this.db.Doctors.FindAsync(doctorId);

            if (doctor == null)
            {
                throw new ArgumentException("This person is not a doctor!");
            }

            this.db.Doctors.Remove(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<DoctorViewModel>> GetDoctorsAsync()
        {
            return await this.db.Doctors.Select(d => new DoctorViewModel
            {
                DoctorId = d.Id,
                Adress = d.Adress,
                Qualification = d.Qualification,
                FullName = d.FullName,
            }).ToListAsync();
        }

        public async Task<EditDoctorViewModel> GetDoctorEditAsync(string doctorId)
        {
            var doctor = await this.db.Doctors.FindAsync(doctorId);

            return new EditDoctorViewModel
            {
                DoctorId = doctorId,
                FullName = doctor.FullName,
                Adress = doctor.Adress,
                Qualification = doctor.Qualification,
            };
        }

        public async Task EditDoctorAsync(EditDoctorInputModel input)
        {
            var doctor = await this.db.Doctors.FindAsync(input.DoctorId);

            doctor.FullName = input.FullName;
            doctor.Adress= input.Adress;
            doctor.Qualification = input.Qualification;

            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Director

        public async Task AddDirectorAsync(AddDirectorInput input)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Email == input.UserEmail);

            if (user == null)
            {
                throw new ArgumentException("There is not a user with the given email in our system!");
            }

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.DirectorRoleName) == false)
            {
                throw new ArgumentException("The given person does not have the director role! Add the role first in the assign roles tab");
            }

            var check = await this.db.Directors.FirstOrDefaultAsync(d => d.FullName == input.FullName);

            if (check != null)
            {
                throw new ArgumentException("There is already a director with this name!");
            }

            var hospital = await this.db.Hospitals.Include(h => h.Director).FirstOrDefaultAsync(h => h.Name == input.HospitalName);

            if (hospital == null)
            {
                throw new ArgumentException("There is no hopsital with this name!");
            }

            if (hospital.Director != null)
            {
                throw new ArgumentException("This hospital already has a director!");
            }

            var director = new Director
            {
                Id = user.Id,
                FullName = input.FullName,
                Adress = input.Adress,
                Hospital = hospital,
                HospitalId = hospital.HospitalId,
            };

            user.Director = director;

            hospital.Director = director;
            hospital.DirectorId = director.Id;

            await this.db.Directors.AddAsync(director);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDirectorAsync(string directorId)
        {
            var director = await this.db.Directors.Include(d => d.Hospital).FirstOrDefaultAsync(d => d.Id == directorId);

            if (director == null)
            {
                throw new ArgumentException("This person is not a director!");
            }

            this.db.Directors.Remove(director);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<DirectorViewModel>> GetDirectorsAsync()
        {
            return await this.db.Directors.Include(d => d.Hospital).Select(d => new DirectorViewModel
            {
                DirectorId = d.Id,
                Adress = d.Adress,
                FullName = d.FullName,
                HospitalName = d.Hospital.Name,
                HospitalId = d.HospitalId,
            }).ToListAsync();
        }

        public async Task<EditDirectorViewModel> GetDirectorEditAsync(string directorId)
        {
            var director = await this.db.Directors.FindAsync(directorId);

            return new EditDirectorViewModel
            {
                DirectorId = director.Id,
                FullName = director.FullName,
                Adress = director.Adress,
            };
        }

        public async Task EditDirectorAsync(EditDirectorInputModel input)
        {
            var director = await this.db.Directors.FindAsync(input.DirectorId);

            director.FullName = input.FullName;
            director.Adress = input.Adress;

            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Hospital

        public async Task AddHospitalAsync(HospitalInputModel input)
        {
            var check = await this.db.Hospitals.FirstOrDefaultAsync(h => h.Name == input.Name);

            if (check != null)
            {
                throw new ArgumentException("There is already a Hospital with this name!");
            }

            var hospital = new Hospital
            {
                Name = input.Name,
                Location = input.Location,
            };

            await this.db.Hospitals.AddAsync(hospital);

            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<HospitalViewModel>> GetHospitalsAsync()
        {
            var hospitals = await this.db.Hospitals
                .Select(h => new HospitalViewModel
                {
                    HospitalId = h.HospitalId,
                    Name = h.Name,
                    Location = h.Location,
                    Departments = h.Departments.Select(c => new DepartmentDTO
                    {
                        DepartmentId = c.DepartmentId,
                        Name = c.Name,
                    }).ToList(),
                })
                .ToListAsync();

            return hospitals;
        }

        public async Task<EditHospitalViewModel> GetHospitalEditAsync(string hospitalId)
        {
            var hospitalDb = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            if (hospitalDb == null)
            {
                throw new ArgumentNullException("hospitalId", "No hospital found with this id!");
            }

            return new EditHospitalViewModel()
            {
                HospitalId = hospitalDb.HospitalId,
                Name = hospitalDb.Name,
                Location = hospitalDb.Location,
            };
        }

        public async Task EditHospitalAsync(EditHospitalInputModel input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "The given input is null!");
            }

            var hospital = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == input.HospitalId);

            if (hospital == null)
            {
                throw new InvalidOperationException("No hospital found with this id!");
            }

            hospital.Name = input.Name;
            hospital.Location = input.Location;

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveHospitalAsync(string hospitalId)
        {
            var hospital = await this.db.Hospitals
                .Include(h => h.Director)
                .Include(h => h.Departments).ThenInclude(c => c.Doctors)
                .Include(c => c.Departments).ThenInclude(c => c.Rooms)
                .ThenInclude(r => r.Patients)
                .FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            foreach (var d in hospital.Departments)
            {
                foreach (var r in d.Rooms)
                {
                    foreach (var p in r.Patients)
                    {
                        p.Doctor = null;
                        p.DoctorId = null;
                    }
                }
            }

            if (hospital == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            if (hospital.Director != null)
            {
                hospital.Director.HospitalId = null;
                hospital.Director.Hospital = null;
            }

            this.db.Hospitals.Remove(hospital);

            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Department

        public async Task AddDepartmentToHospitalAsync(DepartmentInputModel input)
        {
            var employer = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == input.HospitalEmployerId);
            if (employer == null)
            {
                throw new ArgumentException("No hospital with the given name exists!");
            }

            var check = await this.db.Departments.FirstOrDefaultAsync(c => c.Name == input.Name);
            if (check != null)
            {
                throw new ArgumentException("This department already exists!");
            }

            var department = new Department
            {
                Name = input.Name,
                HospitalId = employer.HospitalId,
                Hospital = employer,
            };

            await this.db.Departments.AddAsync(department);
            employer.Departments.Add(department);

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDepartmentAsync(string departmentId)
        {
            var department = await this.db.Departments
                .Include(c => c.Doctors)
                .Include(c => c.Rooms)
                .ThenInclude(r => r.Patients)
                .FirstOrDefaultAsync(h => h.DepartmentId == departmentId);

            if (department == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            var hospitalEmployer = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == department.HospitalId);

            hospitalEmployer.Departments.Remove(department);

            this.db.Departments.Remove(department);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<DepartmentViewModel>> GetDepartmentsInHospitalAsync(string hospitalId)
        {
            var hospital = await this.db.Hospitals
                .Include(h => h.Departments)
                .ThenInclude(d => d.Boss)
                .Include(h => h.Departments)
                .ThenInclude(c => c.Doctors)
                .Include(h => h.Departments)
                .ThenInclude(c => c.Rooms)
                .FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            var departments = hospital.Departments
                .Select(d => new DepartmentViewModel
                {
                    HospitalId = hospital.HospitalId,
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                    Boss = d.Boss == null ? null : new DoctorDTO
                    {
                        DepartmentId = d.DepartmentId,
                        DoctorId = d.Boss.Id,
                        Name = d.Boss.FullName,
                        Qulifications = d.Boss.Qualification,
                    },
                    Doctors = d.Doctors
                        .Where(p => p.Id != d.BossId)
                        .Select(p => new DoctorDTO
                        {
                            DepartmentId = d.DepartmentId,
                            DoctorId = p.Id,
                            Name = p.FullName,
                            Qulifications = p.Qualification,
                        }).ToList(),
                    Rooms = d.Rooms
                        .Select(r => new RoomDTO
                        {
                            RoomId = r.RoomId,
                            RoomName = r.Name,
                            RoomType = r.RoomType.ToString(),
                        })
                        .ToList(),
                }).ToList();
            return departments;
        }

        public async Task EditDepartmentAsync(EditDepartmentInputModel input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "The given input is null!");
            }

            var department = await this.db.Departments.FirstOrDefaultAsync(d => d.DepartmentId == input.DepartmentId);

            if (department == null)
            {
                throw new InvalidOperationException("No department found with this id!");
            }

            department.Name = input.Name;

            await this.db.SaveChangesAsync();
        }

        public async Task<EditDepartmentViewModel> GetDepartmentEdit(string id)
        {
            var departmentDb = await this.db.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (departmentDb == null)
            {
                throw new ArgumentNullException("departmentDb", "No department found with this id!");
            }

            return new EditDepartmentViewModel
            {
                DepartmentId = departmentDb.DepartmentId,
                Name = departmentDb.Name,
                HospitalId = departmentDb.HospitalId,
            };
        }

        public async Task AddDoctorToDepartment(AddDoctorToDepartmentInput input)
        {
            var user = await this.db.Users.Include(u => u.Doctor).FirstOrDefaultAsync(u => u.Email == input.Email);

            if (user == null)
            {
                throw new ArgumentException("The given person is not in our system!");
            }

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.DoctorRoleName) == false)
            {
                throw new ArgumentException("The given person does not have the doctor role!");
            }

            var doctor = await this.db.Doctors.FirstOrDefaultAsync(d => d.Id == user.Id);

            if (doctor == null)
            {
                throw new ArgumentException("The given person is not a doctor!");
            }

            var department = await this.db.Departments.FindAsync(input.DepartmentId);

            if (department == null)
            {
                throw new ArgumentException("The given department does not exist.");
            }

            department.Doctors.Add(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDoctorFromDepartment(string doctorId, string departmentId)
        {
            var doctor = await this.db.Doctors.Include(d => d.Departments).FirstOrDefaultAsync(d => d.Id == doctorId);
            var department = await this.db.Departments.Include(d => d.Doctors).FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (doctor == null || department == null)
            {
                throw new ArgumentException("Doctor or department not found");
            }

            department.Doctors.Remove(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task MakeDoctorBossOfDepartment(string doctorId, string departmentId)
        {
            var doctor = await this.db.Doctors.FindAsync(doctorId);

            if (doctor.BossDepartmentId != null)
            {
                throw new ArgumentException("This doctor is a boss of another department!");
            }

            var department = await this.db.Departments.FindAsync(departmentId);

            if (department.BossId != null)
            {
                throw new ArgumentException("This department has a boss already!");
            }

            department.Boss = doctor;
            department.BossId = doctor.Id;

            doctor.BossDepartment = department;
            doctor.BossDepartmentId = department.DepartmentId;

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDoctorBossOfDepartment(string doctorId, string departmentId)
        {
            var doctor = await this.db.Doctors.FindAsync(doctorId);

            if (doctor.BossDepartmentId != departmentId)
            {
                throw new ArgumentException("This doctor is not a boss of this department!");
            }

            var department = await this.db.Departments.FindAsync(departmentId);

            department.Boss = null;
            department.BossId = null;

            doctor.BossDepartment = null;
            doctor.BossDepartmentId = null;

            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Room

        public async Task AddRoomToDepartment(AddRoomToDepartmentInput input)
        {
            var department = await this.db.Departments.FindAsync(input.DepartmentId);

            if (department == null)
            {
                throw new ArgumentException("The given department does not exist.");
            }

            var room = new Room
            {
                Name = input.Name,
                Department = department,
                DepartmentId = department.DepartmentId,
                RoomTypeId = int.Parse(input.RoomType),
            };

            department.Rooms.Add(room);
            await this.db.Rooms.AddAsync(room);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveRoomFromDepartment(string roomId, string departmentId)
        {
            var room = await this.db.Rooms.FindAsync(roomId);
            var department = await this.db.Departments.Include(d => d.Rooms).FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (room == null || department == null)
            {
                throw new ArgumentException("Room or department not found");
            }

            department.Rooms.Remove(room);
            this.db.Rooms.Remove(room);
            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Statistics

        public async Task<ICollection<DoctorsViewModel>> GetPatientsStatisticsAsync()
        {
            return await this.db.Doctors
                .Include(d => d.Patients)
                .Select(d => new DoctorsViewModel
                {
                    Name = d.FullName,
                    Patients = d.Patients.Select(p => new global::Hospital.Web.ViewModels.Administration.Dashboard.Statistics.PatientDTO
                    {
                        Name = p.FullName,
                    }).ToList(),
                }).ToListAsync();
        }

        public async Task<ICollection<DepartmentsViewModel>> GetDoctorsStatisticsAsync()
        {
            return await this.db.Departments
                .Include(d => d.Doctors)
                .Select(d => new DepartmentsViewModel
                {
                    Name = d.Name,
                    Doctors = d.Doctors.Select(dc => new DoctorInDepartmentDTO
                    {
                        Name = dc.FullName,
                    }).ToList(),
                }).ToListAsync();
        }

        #endregion
    }
}
