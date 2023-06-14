using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.Role
{
    public sealed class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new AppRole { Name = name });
            return result.Succeeded && result.Errors.Any();
        }

        public async Task<bool> DeleteRole(string name)
        {
            IdentityResult result = await _roleManager.DeleteAsync(new AppRole { Name = name });
            return result.Succeeded && result.Errors.Any();
        }

        public async Task<(string id, string name)> GetRoleByIdAsync(string id)
        {
            string role = await _roleManager.GetRoleIdAsync(new AppRole { Id = id });
            return (id, role);
        }

        public IDictionary<string, string?> GetRoles()
        {
            return _roleManager.Roles.ToDictionary(role => role.Id, role => role.Name);

        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            IdentityResult result = await _roleManager.UpdateAsync(new AppRole { Id = id, Name = name });
            return result.Succeeded && result.Errors.Any();
        }
    }
}
