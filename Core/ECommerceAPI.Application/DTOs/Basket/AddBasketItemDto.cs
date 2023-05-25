using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Basket
{
    public class AddBasketItemDto
    {
        //to which basket, items to be added will get by username of logged in user as adding items, not from client
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
