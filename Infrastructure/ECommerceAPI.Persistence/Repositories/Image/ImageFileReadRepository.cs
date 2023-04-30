using ECommerceAPI.Application.Repositories.Image;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories.Image
{
    public class ImageFileReadRepository : ReadRepository<ImageFile>, IImageFileEntityReadRepository
    {
        public ImageFileReadRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
