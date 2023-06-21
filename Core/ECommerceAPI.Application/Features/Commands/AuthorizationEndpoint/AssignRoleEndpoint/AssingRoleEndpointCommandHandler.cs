using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint
{
    public sealed class AssingRoleEndpointCommandHandler : IRequestHandler<AssingRoleEndpointCommandRequest, AssingRoleEndpointCommandResponse>
    {
        private readonly IAuthorizationEndpointService _authorizationEndpointService;

        public AssingRoleEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpointService)
        {
            _authorizationEndpointService = authorizationEndpointService;
        }

        public async Task<AssingRoleEndpointCommandResponse> Handle(AssingRoleEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            await _authorizationEndpointService.AssignRoleEndpointAsync(request.Roles, request.Menu, request.Code, request.Type);
            return new() { };
        }
    }
}
