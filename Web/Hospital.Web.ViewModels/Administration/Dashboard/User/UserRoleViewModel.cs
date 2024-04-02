using System.Collections.Generic;

namespace Hospital.Web.ViewModels.Administration.Dashboard.User
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public ICollection<RoleDTO> Roles { get; set; }

        public ICollection<RoleDTO> AvailableRoles { get; set; }
    }
}
