using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories.Basket
{
    public interface IBasketReadRepository : IReadRepository<Domain.Entities.Basket>
    {
    }
}
