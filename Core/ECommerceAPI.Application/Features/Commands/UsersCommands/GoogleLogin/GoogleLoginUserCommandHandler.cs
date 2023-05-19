using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.User;
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
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleLoginUserCommandHandler(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        public async Task<GoogleLoginUserCommandResponse> Handle(GoogleLoginUserCommandRequest request, CancellationToken cancellationToken)
        {
          GoogleLoginUserResponseDTO googleLoginUserResponse= await _googleAuthService.GoogleLoginAsync(new()
            {
                Email = request.Email,
                Name = request.Name,
                PhotoUrl = request.PhotoUrl,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Id = request.Id,
                Provider = request.Provider,
                IdToken = request.IdToken,

            });
            return new()
            {
                Token = googleLoginUserResponse.Token,
            };
        }
    }
}
/* public string Id { get; set; }
        public string Email { get; set; }
        public string IdToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Provider { get; set; }*/