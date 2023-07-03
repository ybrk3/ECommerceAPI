using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.UsersCommands.CreateUser;
using ECommerceAPI.Application.Helpers.CustomEncoders;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services.User
{
    public sealed class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEndpointReadRepository _endpointReadRepository;

        public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
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
            if (user is not null)
            {
                //set user's refresh token values
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryDate = accesTokenExpiryDate.AddMinutes(onAddRefreshTokenTime);
                //then update it
                await _userManager.UpdateAsync(user);
            }
            throw new NotFoundUserException();
        }
        public async Task UpdatePassword(string userId, string resetToken, string newPassword)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

                if (result.Succeeded) { await _userManager.UpdateSecurityStampAsync(user); }
                else throw new PasswordChangeFailedException("Password reset unsuccessful");
            };
        }

        public async Task<List<UsersListDto>> GetUsers(int page, int size)
        {
            var users = await _userManager.Users
                     .Skip(page * size)
                     .Take(size)
                     .ToListAsync();

            return users.Select(user => new UsersListDto
            {
                Id = user.Id,
                NameSurname = user.NameSurname,
                Username = user.UserName,
                Email = user.Email,
                TwoFactorEnabled = user.TwoFactorEnabled,
            }).ToList();
        }

        public int TotalUsersCount => _userManager.Users.Count(); //It is for pagination

        public async Task AssingRoleToUser(string userId, string[] roles)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Count() != 0) await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);

            }
            throw new NotFoundUserException("Sorry. We encountered an unexpected error. Please try again");
        }

        public async Task<string[]> GetUserRoles(string userIdOrName)
        {
            AppUser? user = await _userManager.FindByIdAsync(userIdOrName);
            if (user is null) user = await _userManager.FindByNameAsync(userIdOrName);

            if (user is not null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }

            throw new NotFoundUserException("Sorry. We encountered an unexpected error. Please try again");
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            //get the user's roles via name info
            var userRoles = await GetUserRoles(name);
            //if user does not have roles, it means that the user has no permission
            if (!userRoles.Any())
                return false;

            //Getting endpoint info with roles by code of endpoint
            Endpoint? endpoint = await _endpointReadRepository.Table
                   .Include(e => e.Roles)
                   .FirstOrDefaultAsync(e => e.Code == code);
            if (endpoint is null)
                return false;
            //If endpoint is not null, function will continue by checking roles through below

            var endpointRoles = endpoint.Roles.Select(r => r.Name);
            //If one of userRoles equals the endpointRole of endpointRoles, method will return true
            return userRoles.Any(userRoles => endpointRoles.Contains(userRoles));

        }
    }
}
