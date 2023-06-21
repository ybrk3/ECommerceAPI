using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.Authorization
{
    public sealed class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        private readonly IMenuReadRepository _menuReadRepository;
        private readonly IMenuWriteRepository _menuWriteRepository;
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IEndpointWriteRepository _endpointWriteRepository;
        private readonly IApplicationService _applicationService;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthorizationEndpointService(IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IApplicationService applicationService, RoleManager<AppRole> roleManager)
        {
            _menuReadRepository = menuReadRepository;
            _menuWriteRepository = menuWriteRepository;
            _endpointReadRepository = endpointReadRepository;
            _endpointWriteRepository = endpointWriteRepository;
            _applicationService = applicationService;
            _roleManager = roleManager;
        }

        public async Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
        {
            #region Adding Menu & Endpoint
            //Check whether menu exists
            bool menuExistency = await _menuReadRepository.Any(m => m.Name == menu);
            Menu? _menu = await _menuReadRepository.GetSingleAsync(m => m.Name == menu); //to add menu to endpoint, if menu exists
            //If menu not exist, add it
            if (!menuExistency)
            {
                _menu = new() { Id = Guid.NewGuid(), Name = menu };
                await _menuWriteRepository.AddAsync(_menu);
                await _menuWriteRepository.SaveAsync();
            }

            //Check whether endpoint exits or not
            Endpoint? endpoint = await _endpointReadRepository.Table.Include(e => e.Menu).Include(e => e.Roles).FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);
            //If not exist
            if (endpoint == null)
            {
                var action = _applicationService.GetAuthorizeDefinitionEndpoint(type)
               .FirstOrDefault(m => m.MenuName == menu)
               ?.Actions.FirstOrDefault(e => e.ActionCode == code);

                endpoint = new()
                {
                    Id = Guid.NewGuid(),
                    ActionType = action.ActionType,
                    Code = action.ActionCode,
                    HttpType = action.HttpType,
                    Definition = action.Definition,
                    Menu = _menu,
                };

                await _endpointWriteRepository.AddAsync(endpoint);
                await _endpointWriteRepository.SaveAsync();
            }
            #endregion

            #region Adding roles to endpoints
            //Remove relation of endpoint with roles
            endpoint.Roles.Clear();

            //Get roles with same role name
            var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();

            //we created HashSet in ctor of Endpoint to use add function
            foreach (var role in appRoles)
                endpoint.Roles.Add(role);

            await _endpointWriteRepository.SaveAsync();

            #endregion
        }

        public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
        {
            Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.Menu)
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu.Name == menu);
            if (endpoint != null)
                return endpoint.Roles.Select(r => r.Name).ToList();
            return null;
        }
    }
}
