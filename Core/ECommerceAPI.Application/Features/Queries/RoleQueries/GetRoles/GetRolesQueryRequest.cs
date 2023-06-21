using ECommerceAPI.Application.RequestParameters;
using MediatR;

namespace ECommerceAPI.Application.Features.Queries.RoleQueries.GetRoles
{
    public class GetRolesQueryRequest : Pagination, IRequest<GetRolesQueryResponse>
    {
        /*
         * public int Page { get; set; }
         * public int Size { get; set; }
         */
    }
}