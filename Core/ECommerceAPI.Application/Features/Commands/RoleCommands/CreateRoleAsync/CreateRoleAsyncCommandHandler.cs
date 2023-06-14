using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.RoleCommands.CreateAsync
{
    public sealed class CreateRoleAsyncCommandHandler : IRequestHandler<CreateRoleAsyncCommandRequest, CreateRoleAsyncCommandResponse>
    {
        private readonly IRoleService _roleService;

        public CreateRoleAsyncCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<CreateRoleAsyncCommandResponse> Handle(CreateRoleAsyncCommandRequest request, CancellationToken cancellationToken)
        {
           bool result= await _roleService.CreateRole(request.Name);
            return new()
            {
                Succeeded = result,
            };
        }
    }
}
