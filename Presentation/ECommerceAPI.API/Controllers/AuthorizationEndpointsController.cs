using ECommerceAPI.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint;
using ECommerceAPI.Application.Features.Queries.AuthorizationEndpoint;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("get-roles-to-endpoint")]
        public async Task<IActionResult> GetRolesToEndpoint( GetRolesToEndpointQueryRequest request)
        {
            GetRolesToEndpointQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleEndpoint([FromBody] AssingRoleEndpointCommandRequest request)
        {
            request.Type = typeof(Program);

            AssingRoleEndpointCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
