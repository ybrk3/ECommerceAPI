using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.BasketCommands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandHandler : IRequestHandler<RemoveItemFromBasketCommandRequest, RemoveItemFromBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public RemoveItemFromBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<RemoveItemFromBasketCommandResponse> Handle(RemoveItemFromBasketCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.RemoveItemFromBasket(request.BasketItemId);

            return new();
        }
    }
}
