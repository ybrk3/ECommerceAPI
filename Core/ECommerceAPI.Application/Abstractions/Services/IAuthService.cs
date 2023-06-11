using ECommerceAPI.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IAuthService : IRefreshTokenService
    {
        Task<LoginUserResponseDTO> LoginAsync(LoginUserDTO model);
        Task PasswordResetAsync(string email); //Which creates token to reset password and it's sending email to user
        Task<bool> VerifyResetToken(string resetToken, string userId);
    }
}
