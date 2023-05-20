using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IRefreshTokenService
    {
        //Method will check user's refresh token which comes from UI and find user
        //then it will check refresh token expiry date
        //If aboves are ok, it will create a token and update user with token and refresh token data
        //then returns token to send back to UI

        Task<Application.DTOs.Token> LoginWithRefreshToken(string refreshToken); 
    }
}
