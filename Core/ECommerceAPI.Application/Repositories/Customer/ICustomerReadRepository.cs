using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    //this interface contains methods in IReadRepository in relation to Customer
    public interface ICustomerReadRepository : IReadRepository<Customer>
    {
    }
}
