using MediatR;

namespace ECommerceAPI.Application.Features.Queries.RoleQueries.GetRoleById
{
    public class GetRoleByIdAsyncQueryRequest : IRequest<GetRoleByIdAsyncQueryResponse>
    {
        public string Id { get; set; }
    }
}