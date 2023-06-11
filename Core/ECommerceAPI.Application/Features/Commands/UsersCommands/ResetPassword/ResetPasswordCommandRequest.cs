using MediatR;

namespace ECommerceAPI.Application.Features.Commands.UsersCommands.ResetPassword
{
    public class ResetPasswordCommandRequest : IRequest<ResetPasswordCommandResponse>
    {
        public string Email { get; set; }
    }
}