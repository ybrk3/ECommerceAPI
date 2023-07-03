using MediatR;

namespace ECommerceAPI.Application.Features.Queries.UserQueries.GetUserRoles
{
    public class GetUserRolesQueryRequest : IRequest<GetUserRolesQueryResponse>
    {
        public string userId { get; set; }
    }
}