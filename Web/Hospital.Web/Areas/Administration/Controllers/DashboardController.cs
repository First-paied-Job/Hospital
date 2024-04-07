namespace Hospital.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Administration.Dashboard.Department;
    using Hospital.Web.ViewModels.Administration.Dashboard.Director;
    using Hospital.Web.ViewModels.Administration.Dashboard.Doctor;
    using Hospital.Web.ViewModels.Administration.Dashboard.Hospital;
    using Hospital.Web.ViewModels.Administration.Dashboard.Room;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IAdministrationService administrationService;

        public DashboardController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        #region User

        public async Task<IActionResult> AssignRoles()
        {
            // Get all users
            var viewModel = await this.administrationService.GetAllUsers();
            return this.View(viewModel);
        }

        public async Task<IActionResult> AddUserRole(string roleId, string userId)
        {
            // Add the role to the user
            await this.administrationService.AddRoleToUser(roleId, userId);
            return this.Redirect("/Administration/Dashboard/AssignRoles");
        }

        public async Task<IActionResult> RemoveUserRole(string roleId, string userId)
        {
            // Remove the role from user
            await this.administrationService.RemoveRoleFromUser(roleId, userId);
            return this.Redirect("/Administration/Dashboard/AssignRoles");
        }

        #endregion

        #region Doctor

        public IActionResult DoctorControl()
        {
            return this.View();
        }

        public IActionResult AddDoctor()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(AddDoctorInput input)
        {
            try
            {
                // Add doctor to db
                await this.administrationService.AddDoctorAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("doctor", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                // Visualise errors
                return this.View("AddDoctor");
            }

            // Redirect to doctor panel
            return this.Redirect("/Administration/Dashboard/DoctorList");
        }

        public async Task<IActionResult> RemoveDoctor(string doctorId)
        {
            // Remove doctor 
            await this.administrationService.RemoveDoctorAsync(doctorId);

            return this.Redirect("/Administration/Dashboard/DoctorList");
        }

        public async Task<IActionResult> DoctorList()
        {
            // Get all doctors
            var viewModel = await this.administrationService.GetDoctorsAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> EditDoctor(string doctorId)
        {
            // Get doctor view model by id
            var viewModel = await this.administrationService.GetDoctorEditAsync(doctorId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctorPost(EditDoctorInputModel input)
        {
            // Update doctor
            await this.administrationService.EditDoctorAsync(input);

            return this.Redirect("/Administration/Dashboard/DoctorList");
        }

        #endregion

        #region Director

        public IActionResult DirectorControl()
        {
            return this.View("./Director/DirectorControl");
        }

        public IActionResult AddDirector()
        {
            return this.View("./Director/AddDirector");
        }

        [HttpPost]
        public async Task<IActionResult> AddDirector(AddDirectorInput input)
        {
            try
            {
                // Add director to db
                await this.administrationService.AddDirectorAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("Director", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("./Director/AddDirector");
            }

            return this.Redirect("/Administration/Dashboard/DirectorList");
        }

        public async Task<IActionResult> RemoveDirector(string directorId)
        {
            // Remove director from db
            await this.administrationService.RemoveDirectorAsync(directorId);

            return this.Redirect("/Administration/Dashboard/DirectorList");
        }

        public async Task<IActionResult> DirectorList()
        {
            // TODO: Directors who had their hospital removed should be outlisted.
            // Get directors
            var viewModel = await this.administrationService.GetDirectorsAsync();
            return this.View("./Director/DirectorList", viewModel);
        }

        public async Task<IActionResult> EditDirector(string directorId)
        {
            // Get director edit view model
            var viewModel = await this.administrationService.GetDirectorEditAsync(directorId);

            return this.View("./Director/EditDirector", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDirectorPost(EditDirectorInputModel input)
        {
            // Edit director
            await this.administrationService.EditDirectorAsync(input);

            return this.Redirect("/Administration/Dashboard/DirectorList");
        }

        #endregion

        #region Hospital

        public IActionResult HospitalDashboard()
        {
            return this.View();
        }

        public IActionResult AddHospital()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHospital(HospitalInputModel input)
        {
            try
            {
                // Add hosital to db
                await this.administrationService.AddHospitalAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("hospital", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddHospital");
            }

            return this.Redirect("/Administration/Dashboard/HospitalDashboard");
        }

        public async Task<IActionResult> RemoveHospital(string hospitalId)
        {
            // Remove hospital by id
            await this.administrationService.RemoveHospitalAsync(hospitalId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> HospitalList()
        {
            // Get all hospitals
            var viewModel = await this.administrationService.GetHospitalsAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> EditHospital(string hospitalId)
        {
            // Get view model for edit hospital
            var viewModel = await this.administrationService.GetHospitalEditAsync(hospitalId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditHospitalPost(EditHospitalInputModel input)
        {
            // Update hospital info
            await this.administrationService.EditHospitalAsync(input);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        #endregion

        #region Department

        public IActionResult AddDepartment(string hospitalId)
        {
            this.ViewBag.hospitalId = hospitalId;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentInputModel input)
        {
            try
            {
                // Add department to hospital
                await this.administrationService.AddDepartmentToHospitalAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noDepartment", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddDepartment");
            }

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> RemoveDepartment(string departmentId)
        {
            // Remove department
            await this.administrationService.RemoveDepartmentAsync(departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> DepartmentList(string hospitalId)
        {
            // Get all departments
            var viewModel = await this.administrationService.GetDepartmentsInHospitalAsync(hospitalId);
            return this.View(viewModel);
        }

        public async Task<IActionResult> EditDepartment(string departmentId)
        {
            // Get edit view model
            var viewModel = await this.administrationService.GetDepartmentEdit(departmentId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartmentPost(EditDepartmentInputModel input)
        {
            // Edin department in db
            await this.administrationService.EditDepartmentAsync(input);

            return this.Redirect($"/Administration/Dashboard/DepartmentList?hospitalId={input.HospitalId}");
        }

        public IActionResult AddDoctorToDepartment(string departmentId)
        {
            this.ViewBag.DepartmentId = departmentId;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctorToDepartmentPost(AddDoctorToDepartmentInput input)
        {
            try
            {
                // Add doctor to department
                await this.administrationService.AddDoctorToDepartment(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noDepartment", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddDoctorToDepartment", input);
            }

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> RemoveDoctorFromDepartment(string doctorId, string departmentId)
        {
            // Remove doctor from department
            await this.administrationService.RemoveDoctorFromDepartment(doctorId, departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> MakeDoctorBossOfDepartment(string doctorId, string departmentId)
        {
            // Make doctor the boss of the department
            await this.administrationService.MakeDoctorBossOfDepartment(doctorId, departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> RemoveDoctorBossOfDepartment(string doctorId, string departmentId)
        {
            // Removes the doctor from being a boss of the department
            await this.administrationService.RemoveDoctorBossOfDepartment(doctorId, departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        #endregion

        #region Room

        public IActionResult AddRoomToDepartment(string departmentId)
        {
            this.ViewBag.DepartmentId = departmentId;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRoomToDepartmentPost(AddRoomToDepartmentInput input)
        {
            try
            {
                // Add room to department
                await this.administrationService.AddRoomToDepartment(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noDepartment", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddRoomToDepartment", input);
            }

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> RemoveRoomFromDepartment(string roomId, string departmentId)
        {
            // Remove room from department
            await this.administrationService.RemoveRoomFromDepartment(roomId, departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        #endregion

        #region Statistics

        public async Task<IActionResult> PatientsStatistics()
        {
            // Get patient statistic view
            var viewModel = await this.administrationService.GetPatientsStatisticsAsync();
            return this.View("./Statistics/PatientsStatistics", viewModel);
        }

        public async Task<IActionResult> DoctorsStatistics()
        {
            // Get doctor statistic view
            var viewModel = await this.administrationService.GetDoctorsStatisticsAsync();
            return this.View("./Statistics/DoctorsStatistics", viewModel);
        }

        #endregion
    }
}
