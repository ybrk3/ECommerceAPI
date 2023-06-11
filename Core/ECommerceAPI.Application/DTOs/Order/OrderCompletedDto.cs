using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Order
{
    public class OrderCompletedDto
    {
        public string? mailAddress { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string NameSurname { get; set; }
    }
}
