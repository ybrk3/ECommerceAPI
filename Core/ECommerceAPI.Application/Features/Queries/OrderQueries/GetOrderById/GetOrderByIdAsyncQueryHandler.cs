using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrderById
{
    public sealed class GetOrderByIdAsyncQueryHandler : IRequestHandler<GetOrderByIdAsyncQueryRequest, GetOrderByIdAsyncQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdAsyncQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdAsyncQueryResponse> Handle(GetOrderByIdAsyncQueryRequest request, CancellationToken cancellationToken)
        {

            SingleOrderDto data = await _orderService.GetOrderByIdAsync(request.Id);
            return new()
            {
                Id = data.Id,
                Address = data.Address,
                Description = data.Description,
                BasketItems = data.BasketItems,
                OrderCode = data.OrderCode,
                CreatedDate = data.CreatedDate,
                Completed = data.Completed,
            };
        }
    }
}
