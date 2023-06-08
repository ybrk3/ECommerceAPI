using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderReadRepository _orderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task CreateOrder(CreateOrderDto order)
        {
            string orderCode = (new Random().NextDouble() * 10000000).ToString().Split('.')[1];
            await _orderWriteRepository.AddAsync(new()
            {
                Address = order.Address,
                Id = Guid.Parse(order.BasketId),
                Description = order.Description,
                OrderCode = orderCode,
            });
            await _orderWriteRepository.SaveAsync();
        }

        public async Task<SingleOrderDto> GetOrderByIdAsync(string id)
        {
            Domain.Entities.Order? order = await _orderReadRepository.Table.Include(o => o.Basket).ThenInclude(b => b.BasketItems).ThenInclude(bi=>bi.Product).SingleOrDefaultAsync(o => o.Id == Guid.Parse(id));

            return new()
            {
                Id = order?.Id.ToString(),
                Address = order?.Address,
                Description = order?.Description,
                BasketItems = order?.Basket.BasketItems.Select(bi=> new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity
                }),
                CreatedDate = order?.CreatedDate,
                OrderCode = order?.OrderCode,
            };
        }

        public async Task<ListOrdersDto> ListOrdersAsync(int page, int size)
        {

            var query = _orderReadRepository.Table?.Include(o => o.Basket)
                 ?.ThenInclude(b => b.User)
                 ?.Include(o => o.Basket)
                    ?.ThenInclude(b => b.BasketItems)
                    ?.ThenInclude(bi => bi.Product);
            var data = query?.Skip(page * size).Take(size);
            /*.Take((page * size)..size);*/


            return new()
            {
                TotalOrderCount = await query.CountAsync(),
                Orders = await data.Select(o => new
                {
                    Id=o.Id,
                    CreatedDate = o.CreatedDate,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    NameSurname = o.Basket.User.NameSurname,
                }).ToListAsync()
            };

        }
    }
}
