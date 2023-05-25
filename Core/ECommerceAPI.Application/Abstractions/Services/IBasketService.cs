using ECommerceAPI.Application.DTOs.Basket;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBucketItemAsync();
        public Task AddItemToBasket(AddBasketItemDto basketItem);
        public Task UpdateQuantity(UpdateBasketItemDto basketItem);
        public Task RemoveItemFromBasket(string basketItem);
    }
}
