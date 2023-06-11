using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.UsersCommands.UpdatePassword
{
    public sealed class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        private readonly IUserService _userService;

        public UpdatePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            //Check if whether NewPassword and confirmNewPassword same
            if (!request.NewPassword.Equals(request.ConfirmNewPassword))
            {
                throw new PasswordChangeFailedException("Please enter same passwords to confirm whether they are same");
            }

            await _userService.UpdatePassword(request.UserId, request.ResetToken, request.NewPassword);
            return new();
        }
    }
}
