using ECommerceAPI.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<LoginUserResponseDTO> LoginAsync(LoginUserDTO model);
    }
}
