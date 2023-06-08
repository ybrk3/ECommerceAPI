using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.BasketQueries.GetBucketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
    {
        private readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            //Gets basket through username
            List<BasketItem> basketItems = await _basketService.GetBasketItemsAsync();

            return basketItems.Select(_ => new GetBasketItemsQueryResponse
            {
                BasketItemId = _.Id.ToString(),
                Name = _.Product.Name,
                Price = _.Product.Price,
                Quantity = _.Quantity,
                ProductId = _.ProductId,
                imagePaths = _.Product.Images.Where(img => img.Showcase).Select(i => i.Path).ToList(),
            }).ToList();

            throw new NotImplementedException();
        }
    }
}
