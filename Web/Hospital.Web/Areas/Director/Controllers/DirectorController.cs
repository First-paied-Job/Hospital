namespace Hospital.Web.Areas.Director.Controllers
{
    using Hospital.Common;
    using Hospital.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.DirectorRoleName)]
    [Area("Director")]
    public class DirectorController : BaseController
    {
    }
}
