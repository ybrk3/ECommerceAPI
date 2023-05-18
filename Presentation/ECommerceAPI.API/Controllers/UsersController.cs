using ECommerceAPI.Application.Features.Commands.UsersCommands.CreateUser;
using ECommerceAPI.Application.Features.Commands.UsersCommands.FacebookLogin;
using ECommerceAPI.Application.Features.Commands.UsersCommands.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.UsersCommands.LoginUser;
using MediatR;
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

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
        {
            CreateUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest request)
        {
            LoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("google-login")] //End-point
        public async Task<IActionResult> GoogleLogin(GoogleLoginUserCommandRequest request)
        {
            GoogleLoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("facebook-login")] //End-point
        public async Task<IActionResult> FacebookLogin(FacebookLoginUserCommandRequest request)
        {
            FacebookLoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
