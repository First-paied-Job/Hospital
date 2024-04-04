namespace Hospital.Web.Areas.Doctor.Controllers
{
    using Hospital.Common;
    using Hospital.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.DoctorRoleName)]
    [Area("Doctor")]
    public class DoctorController : BaseController
    {
    }
}
