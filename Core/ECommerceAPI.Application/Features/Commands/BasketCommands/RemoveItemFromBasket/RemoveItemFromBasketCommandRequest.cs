using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.BasketCommands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandRequest : IRequest<RemoveItemFromBasketCommandResponse>
    {
        public string BasketItemId { get; set; }
    }
}
