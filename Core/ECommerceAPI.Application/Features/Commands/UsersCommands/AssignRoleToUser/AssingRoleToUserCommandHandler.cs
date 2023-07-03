using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.UsersCommands.AssignRoleToUser
{
    public sealed class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommandRequest, AssignRoleToUserCommandResponse>
    {
        private readonly IUserService _userService;

        public AssignRoleToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.AssingRoleToUser(request.UserId, request.Roles);

            return new();
        }
    }
}
