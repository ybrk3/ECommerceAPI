using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.RoleQueries.GetRoles
{
    public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
    {
        private readonly IRoleService _roleService;
     

        public GetRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
           var roles = _roleService.GetRoles(request.Page,request.Size);
            
            return new()
            {
                Roles = roles.Roles,
                TotalRolesCount = roles.TotalRolesCount,
            };

        }
    }
}
