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
        Task<bool> DeleteRole(string name);
        Task<bool> UpdateRole(string id,string name);
        IDictionary<string, string?> GetRoles();
        Task<(string id,string name)> GetRoleByIdAsync(string id);
    }
}
