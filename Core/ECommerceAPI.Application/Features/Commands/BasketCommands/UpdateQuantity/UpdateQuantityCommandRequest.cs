using ECommerceAPI.Application.DTOs.Basket;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.BasketCommands.UpdateQuantity
{
    public class UpdateQuantityCommandRequest : UpdateBasketItemDto, IRequest<UpdateQuantityCommandResponse>
    {
    }
}
