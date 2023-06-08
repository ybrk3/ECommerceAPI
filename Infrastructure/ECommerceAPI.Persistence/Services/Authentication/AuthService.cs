using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.Authentication
{
    public sealed class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public AuthService(SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, UserManager<AppUser> userManager, IUserService userService)
        {
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<LoginUserResponseDTO> LoginAsync(LoginUserDTO model)
        {
            //At first find the user to login
            //1- By its name
            AppUser? user = await _userManager.FindByNameAsync(model.EmailOrUsername);
            

            //2- if userName not entered, check it by its email address
            if (user == null) { await _userManager.FindByEmailAsync(model.EmailOrUsername); }


            //If user stil not exists, throw exception
            if (user == null) throw new NotFoundUserException();

            //If user found

            //Check password
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false); //lockOutOnFailure is for lock the login for a given time (such as; try after 1 minutes)


            //Give authorization, if password matches
            if (signInResult.Succeeded)
            {
                //User authenticated, so authorized it 
                Token token = _tokenHandler.CreateAccessToken(5, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);
                return new()
                {
                    Token = token,
                };
                
            }
            //Throw exception if password not matches
            throw new AuthenticationErrorException();
        }

        public async Task<Token> LoginWithRefreshToken(string refreshToken)
        {
            //find the user by refresh token
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            //If user found and refresh token not expired
            if (user != null && user.RefreshTokenExpiryDate > DateTime.UtcNow)
            {
                //Create toke
                Token token = _tokenHandler.CreateAccessToken(5, user);
                //update user's refresh token and then user
                await _userService.UpdateRefreshToken(token.AccessToken, user, token.Expiration, 5);
                return token;
            }

            //if user not found, throw authentication error
            throw new AuthenticationErrorException();
        }
    }
}
