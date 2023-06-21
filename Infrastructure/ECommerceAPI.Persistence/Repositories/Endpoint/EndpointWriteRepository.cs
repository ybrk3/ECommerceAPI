using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories
{
    public class EndpointWriteRepository : WriteRepository<Endpoint> , IEndpointWriteRepository
    {
        public EndpointWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
