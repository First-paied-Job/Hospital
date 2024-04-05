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
            var viewModel = await this.administrationService.GetAllUsers();
            return this.View(viewModel);
        }

        public async Task<IActionResult> AddUserRole(string roleId, string userId)
        {
            await this.administrationService.AddRoleToUser(roleId, userId);
            return this.Redirect("/Administration/Dashboard/AssignRoles");
        }

        public async Task<IActionResult> RemoveUserRole(string roleId, string userId)
        {
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
                await this.administrationService.AddDoctorAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("doctor", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddDoctor");
            }

            return this.Redirect("/Administration/Dashboard/DoctorList");
        }

        public async Task<IActionResult> RemoveDoctor(string doctorId)
        {
            await this.administrationService.RemoveDoctorAsync(doctorId);

            return this.Redirect("/Administration/Dashboard/DoctorList");
        }

        public async Task<IActionResult> DoctorList()
        {
            var viewModel = await this.administrationService.GetDoctorsAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> EditDoctor(string doctorId)
        {
            var viewModel = await this.administrationService.GetDoctorEditAsync(doctorId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctorPost(EditDoctorInputModel input)
        {
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
            await this.administrationService.RemoveDirectorAsync(directorId);

            return this.Redirect("/Administration/Dashboard/DirectorList");
        }

        public async Task<IActionResult> DirectorList()
        {
            // Directors who had their hospital removed should be outlisted.
            var viewModel = await this.administrationService.GetDirectorsAsync();
            return this.View("./Director/DirectorList", viewModel);
        }

        public async Task<IActionResult> EditDirector(string directorId)
        {
            var viewModel = await this.administrationService.GetDirectorEditAsync(directorId);

            return this.View("./Director/EditDirector", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDirectorPost(EditDirectorInputModel input)
        {
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
            await this.administrationService.RemoveHospitalAsync(hospitalId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> HospitalList()
        {
            var viewModel = await this.administrationService.GetHospitalsAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> EditHospital(string hospitalId)
        {
            var viewModel = await this.administrationService.GetHospitalEditAsync(hospitalId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditHospitalPost(EditHospitalInputModel input)
        {
            await this.administrationService.EditHospitalAsync(input);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        #endregion

        #region Department

        public IActionResult DepartmentDashboard()
        {
            return this.View();
        }

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
            await this.administrationService.RemoveDepartmentAsync(departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> DepartmentList(string hospitalId)
        {
            var viewModel = await this.administrationService.GetDepartmentsInHospitalAsync(hospitalId);
            return this.View(viewModel);
        }

        public async Task<IActionResult> EditDepartment(string departmentId)
        {
            var viewModel = await this.administrationService.GetDepartmentEdit(departmentId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartmentPost(EditDepartmentInputModel input)
        {
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
            await this.administrationService.RemoveDoctorFromDepartment(doctorId, departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> MakeDoctorBossOfDepartment(string doctorId, string departmentId)
        {
            await this.administrationService.MakeDoctorBossOfDepartment(doctorId, departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        public async Task<IActionResult> RemoveDoctorBossOfDepartment(string doctorId, string departmentId)
        {
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
            await this.administrationService.RemoveRoomFromDepartment(roomId, departmentId);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }

        #endregion

        #region Statistics

        public async Task<IActionResult> PatientsStatistics()
        {
            var viewModel = await this.administrationService.GetPatientsStatisticsAsync();
            return this.View("./Statistics/PatientsStatistics", viewModel);
        }

        public async Task<IActionResult> DoctorsStatistics()
        {
            var viewModel = await this.administrationService.GetDoctorsStatisticsAsync();
            return this.View("./Statistics/DoctorsStatistics", viewModel);
        }

        #endregion
    }
}
