using System.Runtime;

namespace ECommerceAPI.Application.Features.Queries.UserQueries.GetUsers
{
    public class GetUsersQueryResponse
    {
        public object Users { get; set; }
        public int TotalUsersCount { get; set; }
    }
}