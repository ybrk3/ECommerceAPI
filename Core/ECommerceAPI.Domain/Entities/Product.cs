using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
       // public ICollection<Order> Orders { get; set; } We can get order info from BasketItems

        //many-to-many relationship with images
        public ICollection<ImageFile> Images { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
