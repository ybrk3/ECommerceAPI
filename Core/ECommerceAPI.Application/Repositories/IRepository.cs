using ECommerceAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Repositories
{
    //Type in DbSet<T> should be reference type so we set a constraint accordingly
    public interface IRepository<T> where T : BaseEntity
    {
        //We use it to get the related table from DB through which we access ORM tool functions
        DbSet<T> Table { get; }
    }
}
