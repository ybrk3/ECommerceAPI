using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ECommerceAPI.Persistence.Services.Authentication
{
    public sealed class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserService _userService;
        public GoogleAuthService(UserManager<AppUser> userManager, IConfiguration configuration, ITokenHandler tokenHandler, IUserService userService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenHandler = tokenHandler;
            _userService = userService;
        }

        public async Task<GoogleLoginUserResponseDTO> GoogleLoginAsync(GoogleLoginUserDTO model)
        {
            //Setting when validating JSON Web signature
            ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLogin:Google-Client-Id"] },

            };

            //Validating requested IdToken with above settings
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);

            //AspNetUsers consist of users registered through application not from external source
            //AspNetUserLogins consist of users registered from external source such as google, facebook
            //External user info to be added to the "AspNetUserLogins" table, if not exist in that table. 
            UserLoginInfo userLoginInfo = new(model.Provider, payload.Subject, model.Provider);

            //With login information it tries to find user trying to be logged in
            //If returns null, that means user not exists in AspNetUserLogins and AspNetUsers tables
            AppUser? appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

            bool result = appUser != null;
            if(result) {
                //Check user by its mail address
                if (appUser == null) { appUser= await _userManager.FindByEmailAsync(model.Email); }


                if (appUser == null)
                {
                    //If user's mail address also not found, add it to the application as a new user
                    appUser = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        NameSurname = payload.Name,
                        Email = payload.Email,
                        UserName = payload.Email.Split('@')[0],
                    };
                    IdentityResult createResult = await _userManager.CreateAsync(appUser);
                    result = createResult.Succeeded;
                }
            }


            //It will save it to the AspNetUserLogins table
            if (result) await _userManager.AddLoginAsync(appUser, userLoginInfo);
            else throw new Exception("Invalid external authentication");

            //After user saved to the application, Authorization
            Token token = _tokenHandler.CreateAccessToken(5,appUser);
            return new()
            {
                Token = token,
            };

        }
    }
}
