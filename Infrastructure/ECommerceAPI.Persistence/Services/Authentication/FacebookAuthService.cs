using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.Facebook;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ECommerceAPI.Persistence.Services.Authentication
{
    public sealed class FacebookAuthService : IFacebookAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public FacebookAuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, System.Net.Http.IHttpClientFactory httpClientFactory, IConfiguration configuration, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<FacebookLoginUserResponseDTO> FacebookLoginAsyn(FacebookLoginUserDTO model)
        {
            //request token from facebook
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLogin:Facebook:App-Id"]}&client_secret={_configuration["ExternalLogin:Facebook:App-Secret"]}&grant_type=client_credentials");
            //deserialize the token response of facebook to check user
            FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

            //check user from facebook
            string? userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AuthToken}&access_token={facebookAccessTokenResponse?.AccessToken}");
            //get validation status and user_id which handle from above http client request
            FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);


            //if user is valid, get the information needed
            if (validation?.Data.IsValid != null)
            {
                //make a request from facebook for user info and get neccessary info to check or save the user
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={model.AuthToken}");
                FacebookUserInfo? facebookUserInfo = JsonSerializer.Deserialize<FacebookUserInfo>(userInfoResponse);

                //AspNetUsers consist of users registered through application not from external source
                //AspNetUserLogins consist of users registered from external source such as google, fb
                //External user info to be added to"AspNetUserLogins" table, if not exist in that table. 
                UserLoginInfo userLoginInfo = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");

                //With login information it tries to find user trying to be logged in
                //If returns null, that means user not exists in AspNetUserLogins and AspNetUsers tables
                AppUser? user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);


                bool result = user != null;

                //if user couldnt found, check whether its mail address is exist in db
                if (user == null) { await _userManager.FindByEmailAsync(facebookUserInfo.Email); }


                //if user still not found, we save it to the db
                if (user == null)
                {
                    //we created user
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = facebookUserInfo.Email,
                        UserName = facebookUserInfo.Email.Split('@')[0],
                        NameSurname = facebookUserInfo.Name,
                    };

                    //save created-user to AspNetUsers table
                    IdentityResult createResult = await _userManager.CreateAsync(user);
                    result = createResult.Succeeded;
                }

                if (result) await _userManager.CreateAsync(user); //it'll save it to the AspNetUserLogin table
                else throw new Exception("Invalid external authentication");

                //Authenticate the user with token
                Token token = _tokenHandler.CreateAccessToken(5, user);
                //send token to API
                return new() { Token = token };
            }


            throw new AuthenticationErrorException();

        }
    }
}
