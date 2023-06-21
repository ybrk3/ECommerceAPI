using ECommerceAPI.Application.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<bool> CreateRole(string name);
        Task<bool> DeleteRole(string id);
        Task<bool> UpdateRole(string id,string name);
        ListOrderDto GetRoles(int pageNo, int pageSize);
        Task<(string id,string name)> GetRoleByIdAsync(string id);
    }
}
