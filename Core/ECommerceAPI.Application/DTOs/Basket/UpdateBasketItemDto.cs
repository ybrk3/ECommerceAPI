using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Basket
{
    public class UpdateBasketItemDto
    {
        public string BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
