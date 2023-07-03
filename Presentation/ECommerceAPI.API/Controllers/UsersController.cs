using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.UsersCommands.AssignRoleToUser;
using ECommerceAPI.Application.Features.Commands.UsersCommands.CreateUser;
using ECommerceAPI.Application.Features.Commands.UsersCommands.FacebookLogin;
using ECommerceAPI.Application.Features.Commands.UsersCommands.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.UsersCommands.LoginUser;
using ECommerceAPI.Application.Features.Commands.UsersCommands.UpdatePassword;
using ECommerceAPI.Application.Features.Queries.UserQueries.GetUserRoles;
using ECommerceAPI.Application.Features.Queries.UserQueries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Menu = AuthorizeDefinitionConstants.Users, Definition = "Get Users")]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQueryRequest request)
        {
            GetUsersQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
        {
            CreateUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest request)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Menu = AuthorizeDefinitionConstants.Users, Definition = "Assign Role To User")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommandRequest request)
        {
            AssignRoleToUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-user-roles/{userId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Menu = AuthorizeDefinitionConstants.Users, Definition = "Get User Roles")]
        public async Task<IActionResult> GetUserRoles([FromRoute] GetUserRolesQueryRequest request)
        {
            GetUserRolesQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
