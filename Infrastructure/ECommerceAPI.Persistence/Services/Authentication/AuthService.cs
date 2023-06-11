using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Helpers.CustomEncoders;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
        private readonly IMailService _mailService;

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
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 20);
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
                await _userService.UpdateRefreshToken(token.AccessToken, user, token.Expiration, 20);
                return token;
            }

            //if user not found, throw authentication error
            throw new AuthenticationErrorException();
        }

        public async Task PasswordResetAsync(string email)
        {
            //Get and check whether email exists in the system
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                //Create refresh token to reset password
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
               
                //however this token can include non-proper chars for link url, so we are encoding it
                //byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken); //convert it to byte
                //resetToken = WebEncoders.Base64UrlEncode(tokenBytes); //encoding it
                resetToken =resetToken.UrlEncode();

                //Send mail to user
                await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyResetToken(string resetToken, string userId)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                //convert resetToken to byte array and decode it
                //byte[] resetTokenByte = WebEncoders.Base64UrlDecode(resetToken);
                //convert byte resetToken to string
                //resetToken = Encoding.UTF8.GetString(resetTokenByte);
                resetToken=resetToken.UrlDecode();
                
                
                //verify decoded and converted resetToken with user's info
                await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            };

            return false;
        }
    }
}
