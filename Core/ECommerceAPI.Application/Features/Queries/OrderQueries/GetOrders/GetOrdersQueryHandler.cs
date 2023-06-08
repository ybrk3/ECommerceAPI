using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrders
{
    public sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQueryRequest, GetOrdersQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrdersQueryResponse> Handle(GetOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderService.ListOrdersAsync(request.Page, request.Size);
            return new()
            {
                TotalOrderCount = data.TotalOrderCount,
                Orders = data.Orders,
            };
        }
    }
}
