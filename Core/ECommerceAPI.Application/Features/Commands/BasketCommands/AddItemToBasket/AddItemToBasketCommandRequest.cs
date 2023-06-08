using ECommerceAPI.Application.DTOs.Basket;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.BasketCommands.AddItemToBasket
{
    public class AddItemToBasketCommandRequest :AddBasketItemDto ,IRequest<AddItemToBasketCommandResponse>
    {
    }
}
