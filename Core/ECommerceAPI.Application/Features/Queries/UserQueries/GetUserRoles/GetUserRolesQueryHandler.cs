using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.UserQueries.GetUserRoles
{
    public sealed class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQueryRequest, GetUserRolesQueryResponse>
    {
        private readonly IUserService _userService;

        public GetUserRolesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserRolesQueryResponse> Handle(GetUserRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var userRoles = await _userService.GetUserRoles(request.userId);
            return new()
            {
                UserRoles = userRoles,
            };
        }
    }
}
