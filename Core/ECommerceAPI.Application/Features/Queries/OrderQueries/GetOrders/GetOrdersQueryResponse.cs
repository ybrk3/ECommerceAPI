using ECommerceAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrders
{
    public class GetOrdersQueryResponse 
    {
        public int TotalOrderCount { get; set; }
        public object Orders { get; set; }
    }
}
