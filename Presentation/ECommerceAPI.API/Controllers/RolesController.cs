using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Features.Commands.RoleCommands.CreateAsync;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMediator _mediator;

        public RolesController(IRoleService roleService, IMediator mediator)
        {
            _roleService = roleService;
            _mediator = mediator;
        }
        [HttpPost("{name}")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleAsyncCommandRequest request)
        {

            return Ok();
        }
    }
}
