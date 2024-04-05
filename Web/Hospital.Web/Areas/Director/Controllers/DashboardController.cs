namespace Hospital.Web.Areas.Director.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Directors.Schelude;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : DirectorController
    {
        private readonly IDirectorService directorService;

        public DashboardController(IDirectorService directorService)
        {
            this.directorService = directorService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var viewModel = await this.directorService.GetDirectorInfo(userId);
            return this.View(viewModel);
        }

        public async Task<IActionResult> PatientsInDepartment(string departmentId)
        {
            var viewModel = await this.directorService.GetPatientsInDepartment(departmentId);
            return this.View(viewModel);
        }

        public async Task<IActionResult> PatientsForDoctor(string doctorId)
        {
            var viewModel = await this.directorService.GetPatientsForDoctor(doctorId);
            return this.View(viewModel);
        }

        public async Task<IActionResult> SetScheludeForDoctors(string departmentId)
        {
            var viewModel = await this.directorService.SetScheludeForDoctors(departmentId);
            return this.View(viewModel);
        }

        public IActionResult ChangeScheludeForDoctor(string doctorId)
        {
            this.ViewBag.doctorId = doctorId;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeScheludeForDoctor(ScheludeInputModel input)
        {
            try
            {
                await this.directorService.ChangeScheludeForDoctor(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("schelude", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("ChangeScheludeForDoctor", input);
            }

            return this.Redirect($"/Director/Dashboard");
        }
    }
}
