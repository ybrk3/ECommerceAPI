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
    public sealed class OrderService : IOrderService
    {
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICompletedOrderWriteRepository _completedOrderWriteRespository;
        private readonly ICompletedOrderReadRepository _completedOrderReadRepository;
        private readonly IMailService _mailService;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRespository, ICompletedOrderReadRepository completedOrderReadRepository, IMailService mailService)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRespository = completedOrderWriteRespository;
            _completedOrderReadRepository = completedOrderReadRepository;
            _mailService = mailService;
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
           var orders =  _orderReadRepository.Table
                            ?.Include(o => o.Basket)
                                ?.ThenInclude(b => b.BasketItems)
                                .ThenInclude(bi => bi.Product);

            var orderWithOrderStatus = await (from order in orders
                                       join orderCompleted in _completedOrderReadRepository.Table
                                         on order.Id equals orderCompleted.OrderId into _co
                                       from co in _co.DefaultIfEmpty()
                                       select new 
                                       {
                                           Id = order.Id,
                                           CreatedDate = order.CreatedDate,
                                           OrderCode = order.OrderCode,
                                           Basket = order.Basket,
                                           Completed = co != null ? true : false,
                                           Address = order.Address,
                                           Description = order.Description
                                       }).FirstOrDefaultAsync(o=> o.Id == Guid.Parse(id));

            return new()
            {
                Id = orderWithOrderStatus?.Id.ToString(),
                Address = orderWithOrderStatus?.Address,
                Description = orderWithOrderStatus?.Description,
                BasketItems = orderWithOrderStatus?.Basket.BasketItems?.Select(bi=> new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity
                }),
                CreatedDate = orderWithOrderStatus?.CreatedDate,
                OrderCode = orderWithOrderStatus?.OrderCode,
                Completed= orderWithOrderStatus?.Completed,
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


            //Left join to get completedOrders
            //it will get null value if empty
            var dataWithCompletionInfo = from order in data
                                         join completedOrder in _completedOrderReadRepository.Table
                                         on order.Id equals completedOrder.OrderId into co
                                         from _co in co.DefaultIfEmpty()
                                         select new
                                         {
                                             Id=order.Id,
                                             CreatedDate = order.CreatedDate,
                                             OrderCode=order.OrderCode,
                                             Basket=order.Basket,
                                             Completed=_co !=null ? true : false,
                                         };
            

            return new()
            {
                TotalOrderCount = await query.CountAsync(),
                Orders = await dataWithCompletionInfo.Select(o => new
                {
                    Id=o.Id,
                    CreatedDate = o.CreatedDate,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    NameSurname = o.Basket.User.NameSurname,
                    Completed=o.Completed,
                }).ToListAsync()
            };

        }

        public async Task<(bool, OrderCompletedDto)> CompleteOrderAsync(string orderId)
        {
            Domain.Entities.Order? order = await _orderReadRepository.Table.Include(o => o.Basket).ThenInclude(b => b.User).FirstOrDefaultAsync(o=> o.Id==Guid.Parse(orderId));

            if (order is not null)
            {
               await _completedOrderWriteRespository.AddAsync(new()
                {
                    OrderId = Guid.Parse(orderId),
                });

                //Eğer sipariş tamamlandıysa maila atacak
              return(  await _completedOrderWriteRespository.SaveAsync() > 0, new OrderCompletedDto()
              {
                  OrderCode=order.OrderCode,
                  OrderDate=order.CreatedDate,
                  NameSurname=order.Basket.User.NameSurname,
                  mailAddress=order?.Basket?.User?.Email,
              });
            }
            return (false, null);
        }
    }
}
