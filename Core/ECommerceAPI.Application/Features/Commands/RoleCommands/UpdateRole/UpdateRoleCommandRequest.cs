using MediatR;

namespace ECommerceAPI.Application.Features.Commands.RoleCommands.UpdateRole
{
    public class UpdateRoleCommandRequest : IRequest<UpdateRoleCommandResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}