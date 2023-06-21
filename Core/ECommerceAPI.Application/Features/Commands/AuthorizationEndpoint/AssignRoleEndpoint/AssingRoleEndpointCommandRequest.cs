using MediatR;
using MediatR.Pipeline;

namespace ECommerceAPI.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint
{
    public class AssingRoleEndpointCommandRequest : IRequest<AssingRoleEndpointCommandResponse>
    {
        public string[] Roles { get; set; }
        public string Menu { get; set; }
        public string Code { get; set; }

        //Type gets value from API not from UI, so we declared it as nullable
        public Type? Type { get; set; }
    }
}