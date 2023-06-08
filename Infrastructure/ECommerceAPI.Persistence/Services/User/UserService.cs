using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.UsersCommands.CreateUser;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.User
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponseDTO> CreateAsync(CreateUserDTO model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                NameSurname = model.NameSurname,
            }, model.Password);

            CreateUserResponseDTO response = new() { Succeeded = result.Succeeded };

            if (response.Succeeded)
                response.Message = "Successfully registered";
            else
            {
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            }
            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accesTokenExpiryDate, int onAddRefreshTokenTime)
        {
            if (user != null)
            {
                //set user's refresh token values
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryDate = accesTokenExpiryDate.AddMinutes(onAddRefreshTokenTime);
                //then update it
                await _userManager.UpdateAsync(user);
            }
            throw new NotFoundUserException();
        }
    }
}
