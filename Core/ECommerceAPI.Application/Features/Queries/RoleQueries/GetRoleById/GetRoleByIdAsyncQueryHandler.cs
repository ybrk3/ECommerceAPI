using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.RoleQueries.GetRoleById
{
    public sealed class GetRoleByIdAsyncQueryHandler : IRequestHandler<GetRoleByIdAsyncQueryRequest, GetRoleByIdAsyncQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetRoleByIdAsyncQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRoleByIdAsyncQueryResponse> Handle(GetRoleByIdAsyncQueryRequest request, CancellationToken cancellationToken)
        {
          var data= await _roleService.GetRoleByIdAsync(request.Id);
            return new()
            {
                Id = data.id,
                Name = data.name,
            };
        }
    }
}
