using ECommerceAPI.Application.RequestParameters;
using MediatR;

namespace ECommerceAPI.Application.Features.Queries.UserQueries.GetUsers
{
    public class GetUsersQueryRequest : Pagination, IRequest<GetUsersQueryResponse>
    {
    }
}