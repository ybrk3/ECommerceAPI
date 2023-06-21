using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Role;
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
            IdentityResult result = await _roleManager.CreateAsync(new AppRole { Id = Guid.NewGuid().ToString(), Name = name });
            return result.Succeeded && result.Errors.Any();
        }

        public async Task<bool> DeleteRole(string id)
        {
            IdentityResult result = await _roleManager.DeleteAsync(new AppRole { Id = id });
            return result.Succeeded && result.Errors.Any();
        }

        public async Task<(string id, string name)> GetRoleByIdAsync(string id)
        {
            string role = await _roleManager.GetRoleIdAsync(new AppRole { Id = id });
            return (id, role);
        }

        public ListOrderDto GetRoles(int pageNo, int pageSize)
        {
            IQueryable<AppRole> query = _roleManager.Roles;
            int totalRolesCount = query.Count();
            object roles;


            //if pageNo or pageSize is not minus 1, it will return roles in consideration of pagination info
            if (pageNo != -1 || pageSize != -1)
                roles = query.Skip(pageNo * pageSize).Take(pageSize).Select(r => new
                {
                    r.Id,
                    r.Name
                });
            else
                roles = query.Select(r => new
                {
                    r.Name,
                    r.Id
                });

            return new()
            {
                Roles = roles,
                TotalRolesCount = totalRolesCount,
            };
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            IdentityResult result = await _roleManager.UpdateAsync(new AppRole { Id = id, Name = name });
            return result.Succeeded && result.Errors.Any();
        }
    }
}
