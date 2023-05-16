using ECommerceAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.UsersCommands.LoginUser
{
    public class LoginUserCommandResponse
    {
        public Token Token { get; set; }

        //To return JWT-Authorization
    }
}
