using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.RoleCommands.CreateAsync;
using ECommerceAPI.Application.Features.Commands.RoleCommands.DeleteRoleAsync;
using ECommerceAPI.Application.Features.Commands.RoleCommands.UpdateRole;
using ECommerceAPI.Application.Features.Queries.RoleQueries.GetRoleById;
using ECommerceAPI.Application.Features.Queries.RoleQueries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Communications.CallRecords.MicrosoftGraphCallRecordsGetDirectRoutingCallsWithFromDateTimeWithToDateTime;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest request)
        {
            GetRolesQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles By Id", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoles([FromRoute] GetRoleByIdAsyncQueryRequest request)
        {
            GetRoleByIdAsyncQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost()]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleAsyncCommandRequest request)
        {
            CreateRoleAsyncCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> UpdateRole([FromBody, FromRoute] UpdateRoleCommandRequest request)
        {
            UpdateRoleCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> DeleteRole([FromRoute] DeleteRoleAsyncCommandRequest request)
        {
            DeleteRoleAsyncCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
