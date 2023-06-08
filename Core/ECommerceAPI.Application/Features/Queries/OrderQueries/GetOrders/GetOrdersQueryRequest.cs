using ECommerceAPI.Application.RequestParameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrders
{
    public class GetOrdersQueryRequest : Pagination, IRequest<GetOrdersQueryResponse>
    {
    }
}
