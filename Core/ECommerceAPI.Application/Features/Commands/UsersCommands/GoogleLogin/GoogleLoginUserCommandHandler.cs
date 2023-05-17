using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ECommerceAPI.Application.Features.Commands.UsersCommands.GoogleLogin
{
    public class GoogleLoginUserCommandHandler : IRequestHandler<GoogleLoginUserCommandRequest, GoogleLoginUserCommandResponse>
    {
        readonly UserManager<AppUser> _userManager;
        readonly IConfiguration _configuration;
        readonly ITokenHandler _tokenHandler;

        public GoogleLoginUserCommandHandler(UserManager<AppUser> userManager, IConfiguration configuration, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginUserCommandResponse> Handle(GoogleLoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            //Setting when validating JSON Web signature
            ValidationSettings? settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLogin:Google-Client-Id"] }
            };

            //Validating requested IdToken with above settings
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            //AspNetUsers consist of users registered through application not from external source
            //AspNetUserLogins consist of users registered from external source such as google, facebook
            //External user info to be added to the "AspNetUserLogins" table, if not exist in that table. 
            UserLoginInfo userLoginInfo = new(request.Provider, payload.Subject, request.Provider);

            //With login information it tries to find user trying to be logged in
            //If returns null, that means user not exists in AspNetUserLogins and AspNetUsers tables
            AppUser? user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                //Check whether it's mail address is exist in db
                user = await _userManager.FindByEmailAsync(payload.Email);

                //if user still not found, we save it to the system
                if (user == null)
                {

                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email.Split("@")[0],
                        NameSurname = payload.Name,
                    };
                    IdentityResult createResult = await _userManager.CreateAsync(user); //It will save it to the AspNetUsers table
                    result = createResult.Succeeded;
                }
            }

            if (result) await _userManager.AddLoginAsync(user, userLoginInfo); //It will save it to the AspNetUserLogins table
            else throw new Exception("Invalid external authentication");

            //Authenticate the user with token
            Token token = _tokenHandler.CreateAccessToken(5);
            return new GoogleLoginUserCommandResponse()
            {
                Token = token,
            };
        }
    }
}
