namespace Hospital.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hospital.Web.ViewModels.Administration.Dashboard.User;

    public interface IAdministrationService
    {
        // User
        #region
        public Task<ICollection<UserRoleViewModel>> GetAllUsers();

        public Task AddRoleToUser(string roleId, string userId);

        public Task RemoveRoleFromUser(string roleId, string userId);

        #endregion
    }
}
