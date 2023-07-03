using ECommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.UserQueries.GetUsers
{
    public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQueryRequest, GetUsersQueryResponse>
    {
        private readonly IUserService _userService;

        public GetUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUsersQueryResponse> Handle(GetUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsers(request.Page, request.Size);
            return new GetUsersQueryResponse()
            {
                Users = users,
                TotalUsersCount = _userService.TotalUsersCount,
            };
        }
    }
}
