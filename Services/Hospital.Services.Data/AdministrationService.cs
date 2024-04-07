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
            // Get user by id
            var user = this.db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("userId", "There is no user with the given userId!");
            }

            // Get role by id
            var role = this.db.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                throw new InvalidOperationException($"There is no role with the given roleId!");
            }

            // Add relation for user and role
            var userRole = new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = user.Id,
            };

            user.Roles.Add(userRole);

            // Update user
            this.db.Users.Update(user);
            await this.db.UserRoles.AddAsync(userRole);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<UserRoleViewModel>> GetAllUsers()
        {
            var viewModel = new List<UserRoleViewModel>();

            // Get all roles
            var roles = await this.db.Roles.ToListAsync();

            // Get all users including their roles
            var users = await this.db.Users.Include(u => u.Roles).ToListAsync();

            foreach (var user in users)
            {
                var userRolesNames = await this.userManager.GetRolesAsync(user);

                // Get all roles the user has
                var userRoles = roles.Where(r => userRolesNames.Contains(r.Name) == true)
                    .Select(r => new RoleDTO()
                    {
                        RoleId = r.Id,
                        Name = r.Name,
                    })
                    .ToList();

                // Get all roles the user does not have
                var availableRoles = roles
                    .Where(r => userRolesNames.Contains(r.Name) == false)
                    .Select(r => new RoleDTO()
                    {
                        RoleId = r.Id,
                        Name = r.Name,
                    })
                    .ToList();

                // Create view model
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

            // Get user by id
            var user = this.db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("userId", "There is no user with the given userId!");
            }

            // Get role by id
            var role = this.db.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                throw new InvalidOperationException($"There is no role with the given roleId!");
            }

            // Remove relation between user and role
            this.db.UserRoles.Remove(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = user.Id,
            });

            // Update user
            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Doctor
        public async Task AddDoctorAsync(AddDoctorInput input)
        {
            // Get user by email
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Email == input.UserEmail);

            if (user == null)
            {
                throw new ArgumentException("There is not a user with the given email in our system!");
            }

            // Check if doctor
            var check = await this.db.Doctors.FirstOrDefaultAsync(d => d.FullName == input.FullName);

            if (check != null)
            {
                throw new ArgumentException("There is already a doctor with this name!");
            }

            // Create doctor
            var doctor = new Doctor
            {
                Id = user.Id,
                FullName = input.FullName,
                Qualification = input.Qualification,
                Adress = input.Adress,
            };

            user.Doctor = doctor;

            // Add doctor to db
            await this.db.Doctors.AddAsync(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDoctorAsync(string doctorId)
        {
            // Get doctor by id
            var doctor = await this.db.Doctors
                .Include(d => d.Patients)
                .Include(d => d.Departments)
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            foreach (var patient in doctor.Patients)
            {
                patient.Doctor = null;
                patient.DoctorId = null;
                patient.Room = null;
                patient.RoomId = null;
            }

            doctor.Patients = null;

            if (doctor.BossDepartmentId != null)
            {
                var department = await this.db.Departments.FindAsync(doctor.BossDepartmentId);
                department.Boss = null;
                department.BossId = null;
                doctor.BossDepartment = null;
                doctor.BossDepartmentId = null;
            }

            if (doctor == null)
            {
                throw new ArgumentException("This person is not a doctor!");
            }

            // Remove from db
            this.db.Doctors.Remove(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<DoctorViewModel>> GetDoctorsAsync()
        {
            // Convert Doctor View Model
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
            // Get doctor by id
            var doctor = await this.db.Doctors.FindAsync(doctorId);

            // Convert Edit Doctor View Model
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
            // Get doctor by id
            var doctor = await this.db.Doctors.FindAsync(input.DoctorId);

            // Update doctor info
            doctor.FullName = input.FullName;
            doctor.Adress = input.Adress;
            doctor.Qualification = input.Qualification;

            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Director

        public async Task AddDirectorAsync(AddDirectorInput input)
        {
            // Get user by id
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Email == input.UserEmail);

            if (user == null)
            {
                throw new ArgumentException("There is not a user with the given email in our system!");
            }

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.DirectorRoleName) == false)
            {
                throw new ArgumentException("The given person does not have the director role! Add the role first in the assign roles tab");
            }

            // Check if user is director
            var check = await this.db.Directors.FirstOrDefaultAsync(d => d.FullName == input.FullName);

            if (check != null)
            {
                throw new ArgumentException("There is already a director with this name!");
            }

            // Get hospital by name
            var hospital = await this.db.Hospitals.Include(h => h.Director).FirstOrDefaultAsync(h => h.Name == input.HospitalName);

            if (hospital == null)
            {
                throw new ArgumentException("There is no hopsital with this name!");
            }

            if (hospital.Director != null)
            {
                throw new ArgumentException("This hospital already has a director!");
            }

            // Crate director
            var director = new Director
            {
                Id = user.Id,
                FullName = input.FullName,
                Adress = input.Adress,
                Hospital = hospital,
                HospitalId = hospital.HospitalId,
            };

            user.Director = director;

            // Add director to hospital
            hospital.Director = director;
            hospital.DirectorId = director.Id;

            // Add director to directors table
            await this.db.Directors.AddAsync(director);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDirectorAsync(string directorId)
        {
            // Get director by id
            var director = await this.db.Directors.Include(d => d.Hospital).FirstOrDefaultAsync(d => d.Id == directorId);

            if (director == null)
            {
                throw new ArgumentException("This person is not a director!");
            }

            // Remove director
            this.db.Directors.Remove(director);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<DirectorViewModel>> GetDirectorsAsync()
        {
            // Get all directors converted to Director View Model
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
            // Get directo by id
            var director = await this.db.Directors.FindAsync(directorId);

            // Convert to Edit Director View Model
            return new EditDirectorViewModel
            {
                DirectorId = director.Id,
                FullName = director.FullName,
                Adress = director.Adress,
            };
        }

        public async Task EditDirectorAsync(EditDirectorInputModel input)
        {
            // Get director by id
            var director = await this.db.Directors.FindAsync(input.DirectorId);

            // Update info
            director.FullName = input.FullName;
            director.Adress = input.Adress;

            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Hospital

        public async Task AddHospitalAsync(HospitalInputModel input)
        {
            // Check if hospital exists
            var check = await this.db.Hospitals.FirstOrDefaultAsync(h => h.Name == input.Name);

            if (check != null)
            {
                throw new ArgumentException("There is already a Hospital with this name!");
            }

            // Create hospital
            var hospital = new Hospital
            {
                Name = input.Name,
                Location = input.Location,
            };

            // Add hospital to db
            await this.db.Hospitals.AddAsync(hospital);

            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<HospitalViewModel>> GetHospitalsAsync()
        {
            // Get hospitals and convert to Hospital View Model
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
            // Get hospital by id
            var hospitalDb = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            if (hospitalDb == null)
            {
                throw new ArgumentNullException("hospitalId", "No hospital found with this id!");
            }

            // Convert to Edit Hospital View Model
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
            // Get hospital by id including children
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
                        // Remove doctor from every patient in hospital
                        p.Doctor = null;
                        p.DoctorId = null;
                    }
                }
            }

            if (hospital == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            // Remove director from hospital
            if (hospital.Director != null)
            {
                hospital.Director.HospitalId = null;
                hospital.Director.Hospital = null;
            }

            // Remove hospital
            this.db.Hospitals.Remove(hospital);

            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Department

        public async Task AddDepartmentToHospitalAsync(DepartmentInputModel input)
        {
            // Get hospital by id
            var employer = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == input.HospitalEmployerId);
            if (employer == null)
            {
                throw new ArgumentException("No hospital with the given name exists!");
            }

            // Check if department exists
            var check = await this.db.Departments.FirstOrDefaultAsync(c => c.Name == input.Name);
            if (check != null)
            {
                throw new ArgumentException("This department already exists!");
            }

            // Create department
            var department = new Department
            {
                Name = input.Name,
                HospitalId = employer.HospitalId,
                Hospital = employer,
            };

            // Add department to db and to hospital
            await this.db.Departments.AddAsync(department);
            employer.Departments.Add(department);

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDepartmentAsync(string departmentId)
        {
            // Get department including children by id
            var department = await this.db.Departments
                .Include(c => c.Doctors)
                .Include(c => c.Rooms)
                .ThenInclude(r => r.Patients)
                .FirstOrDefaultAsync(h => h.DepartmentId == departmentId);

            if (department == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            // Get hospital by id
            var hospitalEmployer = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == department.HospitalId);

            // Remove department from hospital and db
            hospitalEmployer.Departments.Remove(department);
            this.db.Departments.Remove(department);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<DepartmentViewModel>> GetDepartmentsInHospitalAsync(string hospitalId)
        {
            // Get hospital by id including children
            var hospital = await this.db.Hospitals
                .Include(h => h.Departments)
                .ThenInclude(d => d.Boss)
                .Include(h => h.Departments)
                .ThenInclude(c => c.Doctors)
                .Include(h => h.Departments)
                .ThenInclude(c => c.Rooms)
                .FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            // Get departments in hospital converted to Department View Model
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
                    Doctors = d.Doctors // Convert doctor to DoctorDTO
                        .Where(p => p.Id != d.BossId)
                        .Select(p => new DoctorDTO
                        {
                            DepartmentId = d.DepartmentId,
                            DoctorId = p.Id,
                            Name = p.FullName,
                            Qulifications = p.Qualification,
                        }).ToList(),
                    Rooms = d.Rooms
                        .Select(r => new RoomDTO // Convert room to RoomDTO
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

            // Get department by id
            var department = await this.db.Departments.FirstOrDefaultAsync(d => d.DepartmentId == input.DepartmentId);

            if (department == null)
            {
                throw new InvalidOperationException("No department found with this id!");
            }

            // Update department info
            department.Name = input.Name;

            await this.db.SaveChangesAsync();
        }

        public async Task<EditDepartmentViewModel> GetDepartmentEdit(string id)
        {
            // Get department by id
            var departmentDb = await this.db.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (departmentDb == null)
            {
                throw new ArgumentNullException("departmentDb", "No department found with this id!");
            }

            // Return view model
            return new EditDepartmentViewModel
            {
                DepartmentId = departmentDb.DepartmentId,
                Name = departmentDb.Name,
                HospitalId = departmentDb.HospitalId,
            };
        }

        public async Task AddDoctorToDepartment(AddDoctorToDepartmentInput input)
        {
            // Get user by email
            var user = await this.db.Users.Include(u => u.Doctor).FirstOrDefaultAsync(u => u.Email == input.Email);

            if (user == null)
            {
                throw new ArgumentException("The given person is not in our system!");
            }

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.DoctorRoleName) == false)
            {
                throw new ArgumentException("The given person does not have the doctor role!");
            }

            // Get doctor by id
            var doctor = await this.db.Doctors.FirstOrDefaultAsync(d => d.Id == user.Id);

            if (doctor == null)
            {
                throw new ArgumentException("The given person is not a doctor!");
            }

            // Get department by id
            var department = await this.db.Departments.FindAsync(input.DepartmentId);

            if (department == null)
            {
                throw new ArgumentException("The given department does not exist.");
            }

            // Add doctor to department
            department.Doctors.Add(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDoctorFromDepartment(string doctorId, string departmentId)
        {
            // Get doctor inluding children by id
            var doctor = await this.db.Doctors.Include(d => d.Departments).FirstOrDefaultAsync(d => d.Id == doctorId);

            // Get department including children by id
            var department = await this.db.Departments.Include(d => d.Doctors).FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (doctor == null || department == null)
            {
                throw new ArgumentException("Doctor or department not found");
            }

            // Remove doctor from department
            department.Doctors.Remove(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task MakeDoctorBossOfDepartment(string doctorId, string departmentId)
        {
            // Get doctor by id
            var doctor = await this.db.Doctors.FindAsync(doctorId);

            if (doctor.BossDepartmentId != null)
            {
                throw new ArgumentException("This doctor is a boss of another department!");
            }

            // Get department by id
            var department = await this.db.Departments.FindAsync(departmentId);

            if (department.BossId != null)
            {
                throw new ArgumentException("This department has a boss already!");
            }

            // Set the doctor as boss of the department
            department.Boss = doctor;
            department.BossId = doctor.Id;

            doctor.BossDepartment = department;
            doctor.BossDepartmentId = department.DepartmentId;

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDoctorBossOfDepartment(string doctorId, string departmentId)
        {
            // Get doctor by id
            var doctor = await this.db.Doctors.FindAsync(doctorId);

            if (doctor.BossDepartmentId != departmentId)
            {
                throw new ArgumentException("This doctor is not a boss of this department!");
            }

            // Get department by id
            var department = await this.db.Departments.FindAsync(departmentId);

            // Removes the doctor from being a boss of the department
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
            // Get department by id
            var department = await this.db.Departments.FindAsync(input.DepartmentId);

            if (department == null)
            {
                throw new ArgumentException("The given department does not exist.");
            }

            // Create room
            var room = new Room
            {
                Name = input.Name,
                Department = department,
                DepartmentId = department.DepartmentId,
                RoomTypeId = int.Parse(input.RoomType),
            };

            // Add room to department and to db
            department.Rooms.Add(room);
            await this.db.Rooms.AddAsync(room);
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveRoomFromDepartment(string roomId, string departmentId)
        {
            // Get room by id
            var room = await this.db.Rooms.FindAsync(roomId);

            // Get department by id including children
            var department = await this.db.Departments
                .Include(d => d.Rooms)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (room == null || department == null)
            {
                throw new ArgumentException("Room or department not found");
            }

            // Remove room from department and db
            department.Rooms.Remove(room);
            this.db.Rooms.Remove(room);
            await this.db.SaveChangesAsync();
        }

        #endregion

        #region Statistics

        public async Task<ICollection<DoctorsViewModel>> GetPatientsStatisticsAsync()
        {
            // Convert doctors to Doctors View Model
            return await this.db.Doctors
                .Include(d => d.Patients)
                .Select(d => new DoctorsViewModel
                {
                    Name = d.FullName,
                    Patients = d.Patients.Select(p => new global::Hospital.Web.ViewModels.Administration.Dashboard.Statistics.PatientDTO
                    {
                        Name = p.FullName,
                    }).ToList(), // Convert patient to PatientDTO
                }).ToListAsync();
        }

        public async Task<ICollection<DepartmentsViewModel>> GetDoctorsStatisticsAsync()
        {
            // Convert departments to Departments View Model
            return await this.db.Departments
                .Include(d => d.Doctors)
                .Select(d => new DepartmentsViewModel
                {
                    Name = d.Name,
                    Doctors = d.Doctors.Select(dc => new DoctorInDepartmentDTO
                    {
                        Name = dc.FullName,
                    }).ToList(), // Convert doctors to DoctorInDepartmentDTO
                }).ToListAsync();
        }

        #endregion
    }
}
