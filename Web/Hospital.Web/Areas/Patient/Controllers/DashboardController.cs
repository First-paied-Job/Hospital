namespace Hospital.Web.Areas.Patient.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Hospital.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : PatientController
    {
        private readonly IPatientService patientService;

        public DashboardController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var viewModel = await this.patientService.GetInformationForPatient(userId);

            return this.View(viewModel);
        }
    }
}
