using ECommerceAPI.Application.Features.Commands.UsersCommands.FacebookLogin;
using ECommerceAPI.Application.Features.Commands.UsersCommands.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.UsersCommands.LoginUser;
using ECommerceAPI.Application.Features.Commands.UsersCommands.RefreshTokenLogin;
using ECommerceAPI.Application.Features.Commands.UsersCommands.ResetPassword;
using ECommerceAPI.Application.Features.Commands.UsersCommands.VerifyResetToken;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest request)
        {
            LoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest request)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(request);
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

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandRequest request)
        {
            ResetPasswordCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyResetTokenCommandRequest request)
        {
            VerifyResetTokenCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
