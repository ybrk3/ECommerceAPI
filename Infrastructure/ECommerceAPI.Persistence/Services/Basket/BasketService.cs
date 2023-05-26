using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Basket;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Basket;
using ECommerceAPI.Application.Repositories.BasketItem;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.Basket
{
    public sealed class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _contextAccessor; //it's injected to get user's username when request made. In order to access the object created after request made by user, add "AddHttpContextAccessor()" to API program.cs
        private readonly UserManager<AppUser> _userManager;
        //to get user's orders
        private readonly IOrderWriteRepository _orderWriteRepository;
        //to get BasketItems and/or set them
        private readonly IBasketItemWriteRepository _basketItemWriteRepository;
        private readonly IBasketItemReadRepository _basketItemReadRepository;


        private readonly IBasketReadRepository _basketReadRepository;


        public BasketService(IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IOrderWriteRepository orderWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _orderWriteRepository = orderWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        public async Task AddItemToBasket(AddBasketItemDto basketItem)
        {
            //get user and then its basket
            Domain.Entities.Basket _basket = await FindUserAndGetBasket(); //if it fails, it throws Exception
            if (_basket != null)
            {
                //Check if basketItem to be added whether exists or not
                BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == _basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));
                if (_basketItem != null) _basketItem.Quantity++; //if basketItem exits, increase its quantity
                else //otherwise create and add new BasketItem
                {
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = _basket.Id,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity,
                    });
                }
                await _basketItemWriteRepository.SaveAsync();
            }

        }
        public async Task UpdateQuantity(UpdateBasketItemDto basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if (_basketItem != null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task<List<BasketItem>> GetBucketItemsAsync()
        {
            //Find user and get its basket
            Domain.Entities.Basket basket = await FindUserAndGetBasket();

            //get "Basket" including BasketItems and Products where user's basket equals Baskets in db
            Domain.Entities.Basket? resultBasket = await _basketReadRepository.Table
                                                    .Include(b => b.BasketItems)
                                                    .ThenInclude(b => b.ProductId)
                                                    .FirstOrDefaultAsync(b => b.Id == basket.Id);

            //return basket's basketItems in a list
            return resultBasket?.BasketItems?.ToList();
        }

        public async Task RemoveItemFromBasket(string basketItemId)
        {
            //get basketItem by id
            BasketItem itemToRemove = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (itemToRemove != null)
            {
                //then remove it from db
                _basketItemWriteRepository.Remove(itemToRemove);
                await _basketItemWriteRepository.SaveAsync();
            }
        }




        //Method to find user and get its baskets and/or set its basket
        private async Task<Domain.Entities.Basket> FindUserAndGetBasket()
        {
            //get username from HttpContext which created when user makes request
            string? username = _contextAccessor?.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
                //Get user who made request with their Baskets
                AppUser? user = await _userManager.Users
                     .Include(u => u.Baskets)
                     .FirstOrDefaultAsync(u => u.UserName == username);

                //Get user's Baskets with its' orders, if there is a Basket not ordered yet, return null and group them into BasketOrders
                var _basket = from baskets in user?.Baskets
                              join order in _orderWriteRepository.Table
                              on baskets.Id equals order.Basket.Id into BasketOrder
                              from order in BasketOrder.DefaultIfEmpty()
                              select new
                              {
                                  Basket = baskets,
                                  Order = order
                              };
                //Check if there is null order, if so, set return value as basket which's order is null
                Domain.Entities.Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null)) targetBasket = _basket?.FirstOrDefault(b => b.Order is null)?.Basket;
                else
                {
                    //if there's no null order, it means there isn't an existent basket, so create a new basket and add it to user's Basket table
                    targetBasket = new();
                    user?.Baskets?.Add(targetBasket);
                }
                await _orderWriteRepository.SaveAsync();
                return targetBasket;
            }
            throw new Exception("Unexpected Error! Please try again");
        }
    }
}

