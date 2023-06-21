using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.RoleCommands.DeleteRoleAsync
{
    public sealed class DeleteRoleAsyncCommandHandler : IRequestHandler<DeleteRoleAsyncCommandRequest, DeleteRoleAsyncCommandResponse>
    {
        private readonly IRoleService _roleService;

        public DeleteRoleAsyncCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<DeleteRoleAsyncCommandResponse> Handle(DeleteRoleAsyncCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await _roleService.DeleteRole(request.Id);
            return new()
            {
                Succeeded = result,
            };
        }
    }
}
