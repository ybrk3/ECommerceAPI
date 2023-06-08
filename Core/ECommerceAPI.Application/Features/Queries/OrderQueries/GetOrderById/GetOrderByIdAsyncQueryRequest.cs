using MediatR;

namespace ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrderById
{
    public class GetOrderByIdAsyncQueryRequest : IRequest<GetOrderByIdAsyncQueryResponse>
    {
        public string Id { get; set; }
    }
}