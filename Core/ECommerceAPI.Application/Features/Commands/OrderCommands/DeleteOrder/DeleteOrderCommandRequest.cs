using MediatR;

namespace ECommerceAPI.Application.Features.Commands.OrderCommands.DeleteOrder
{
    public class DeleteOrderCommandRequest :  IRequest<DeleteOrderCommandResponse>
    {
        public string Id { get; set; }
    }
}