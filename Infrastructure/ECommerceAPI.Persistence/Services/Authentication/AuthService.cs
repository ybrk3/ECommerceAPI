using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        readonly UserManager<AppUser> _userManager;

        public AuthService(SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userManager = userManager;
        }

        public async Task<LoginUserResponseDTO> LoginAsync(LoginUserDTO model)
        {
            //At first find the user to login
            //1- By its name
            AppUser? user = await _userManager.FindByNameAsync(model.EmailOrUsername);

            //2- if userName not entered, by its email address
            if (user == null) user = await _userManager.FindByEmailAsync(model.EmailOrUsername);

            //If user stil not exists, throw exception
            if (user == null) throw new NotFoundUserException();


            //If user found

            //Check password
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false); //lockOutOnFailure is for lock the login for a given time (such as; try after 1 minutes)


            //Give authorization, if password matches
            if(signInResult.Succeeded)
            {
                //User authenticated, so authorized it
                Token token = _tokenHandler.CreateAccessToken(5);
                return new()
                {
                    Token = token,
                };
            }

            //Throw exception if password not matches
            throw new AuthenticationErrorException();
            
        }
    }
}
