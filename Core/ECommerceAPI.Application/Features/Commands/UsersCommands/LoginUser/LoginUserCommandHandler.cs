using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.UsersCommands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenHandler tokenHandler)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            //At first find the user to login
            //1- By its name
            AppUser? user = await _userManager.FindByNameAsync(request.EmailOrUsername);
            //2- if userName not entered, by its email address
            if (user == null) user = await _userManager.FindByEmailAsync(request.EmailOrUsername);

            if (user == null) throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false); //lockOutOnFailure is for lock the login for a given time (such as; try after 1 minutes)
            if (result.Succeeded)
            {
                //User found and authenticate so Authorization to be set here
                Token token = _tokenHandler.CreateAccessToken(5);
                return new LoginUserCommandResponse()
                {
                    Token = token,
                };
            }
            //If Authentication not successfully completed, it will return given default message in exceptionb class
            throw new AuthenticationErrorException();
        }
    }
}
