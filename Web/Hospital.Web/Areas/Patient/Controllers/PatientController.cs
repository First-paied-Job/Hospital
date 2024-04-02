namespace Hospital.Web.Areas.Patient.Controllers
{
    using Hospital.Common;
    using Hospital.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.PatientRoleName)]
    [Area("Patient")]
    public class PatientController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
