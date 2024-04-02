namespace Hospital.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hospital.Data;
    using Hospital.Data.Models;
    using Hospital.Services.Data.Contracts;
    using Hospital.Web.ViewModels.Administration.Dashboard.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AdministrationService(
                ApplicationDbContext db,
                UserManager<ApplicationUser> userManager,
                RoleManager<ApplicationRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #region User
        public async Task AddRoleToUser(string roleId, string userId)
        {
            var user = this.db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("userId", "There is no user with the given userId!");
            }

            var role = this.db.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                throw new InvalidOperationException($"There is no role with the given roleId!");
            }

            var userRole = new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = user.Id,
            };

            user.Roles.Add(userRole);

            this.db.Users.Update(user);
            await this.db.UserRoles.AddAsync(userRole);
            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<UserRoleViewModel>> GetAllUsers()
        {
            var viewModel = new List<UserRoleViewModel>();

            var roles = await this.db.Roles.ToListAsync();
            var users = await this.db.Users.Include(u => u.Roles).ToListAsync();

            foreach (var user in users)
            {
                var userRolesNames = await this.userManager.GetRolesAsync(user);

                var userRoles = roles.Where(r => userRolesNames.Contains(r.Name) == true)
                    .Select(r => new RoleDTO()
                    {
                        RoleId = r.Id,
                        Name = r.Name,
                    })
                    .ToList();

                var availableRoles = roles
                    .Where(r => userRolesNames.Contains(r.Name) == false)
                    .Select(r => new RoleDTO()
                    {
                        RoleId = r.Id,
                        Name = r.Name,
                    })
                    .ToList();

                var userModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    Roles = userRoles,
                    AvailableRoles = availableRoles,
                };

                viewModel.Add(userModel);
            }

            return viewModel;
        }

        public async Task RemoveRoleFromUser(string roleId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId", "The given userId is invalid!");
            }

            var user = this.db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("userId", "There is no user with the given userId!");
            }

            var role = this.db.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                throw new InvalidOperationException($"There is no role with the given roleId!");
            }

            this.db.UserRoles.Remove(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = user.Id,
            });

            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }

        #endregion


    }
}
