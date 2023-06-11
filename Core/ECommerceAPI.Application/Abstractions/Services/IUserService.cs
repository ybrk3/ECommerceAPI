using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDTO> CreateAsync(CreateUserDTO model);
        //it will update refresh token
        Task UpdateRefreshToken(string refreshToken, AppUser user,DateTime accesTokenExpiryDate ,int onAddRefreshTokenTime);

        Task UpdatePassword(string userId, string resetToken, string newPassword);
    }
}
