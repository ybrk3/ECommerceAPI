using ECommerceAPI.Application.Repositories.File;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories.File
{
    public class FileEntityReadRepository : ReadRepository<FileEntity>, IFileEntityReadRepository
    {
        public FileEntityReadRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
