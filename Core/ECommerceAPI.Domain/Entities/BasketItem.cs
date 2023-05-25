using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public Guid BasketId { get; set; } //Foreign key with Basket
        public Basket Basket { get; set; }

        public Guid ProductId { get; set; } 
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
