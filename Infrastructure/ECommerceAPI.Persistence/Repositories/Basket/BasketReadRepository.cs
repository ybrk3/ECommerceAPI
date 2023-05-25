using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.Repositories.Basket;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistence.Contexts;
using ECommerceAPI.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories.Basket
{
    public sealed class BasketReadRepository : ReadRepository<Domain.Entities.Basket>, Application.Repositories.Basket.IBasketReadRepository
    {
        public BasketReadRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
