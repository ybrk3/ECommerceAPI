using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.RequestParameters
{
    //It is for pagination in UI
    public abstract class Pagination
    {
        //It is used in GetAllProductsRequest, GetRoles, GetOrders, GetUsers
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
