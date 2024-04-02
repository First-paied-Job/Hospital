namespace Hospital.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : DoctorController
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
    }
}
