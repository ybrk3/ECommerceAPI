using ECommerceAPI.Application.RequestParameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.GetAllProducts
{
    //Parameter to be get through request from client
    //We set it as Request Class through MediatR IRequest interface
    public class GetAllProductsQueryRequest : Pagination, IRequest<GetAllProductsQueryResponse>
    {
        /*public Pagination Pagination { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }*/
    }
}
