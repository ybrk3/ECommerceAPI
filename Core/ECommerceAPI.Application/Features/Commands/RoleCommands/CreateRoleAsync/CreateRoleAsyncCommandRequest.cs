using MediatR;

namespace ECommerceAPI.Application.Features.Commands.RoleCommands.CreateAsync
{
    public class CreateRoleAsyncCommandRequest : IRequest<CreateRoleAsyncCommandResponse>
    {
        public string Name { get; set; }
    }
}