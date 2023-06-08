using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }
        public string Address { get; set; }


        // public ICollection<Product> Products { get; set; } = Basket içerisindeki BasketItems ilgili product'ları taşıdığı için gerek yok
        // public Customer Customer { get; set; } = users assessed as customer

        public Basket Basket { get; set; }
        public string OrderCode { get; set; }
    }
}
