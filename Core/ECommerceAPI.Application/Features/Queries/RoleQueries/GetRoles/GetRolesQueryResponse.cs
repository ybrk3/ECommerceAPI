using System.Runtime;

namespace ECommerceAPI.Application.Features.Queries.RoleQueries.GetRoles
{
    public class GetRolesQueryResponse
    {
        public object Roles { get; set; }
        public int TotalRolesCount { get; set; }
    }
}