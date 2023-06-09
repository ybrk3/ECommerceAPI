﻿using MediatR;

namespace ECommerceAPI.Application.Features.Commands.RoleCommands.DeleteRoleAsync
{
    public class DeleteRoleAsyncCommandRequest : IRequest<DeleteRoleAsyncCommandResponse>
    {
        public string Id { get; set; }
    }
}