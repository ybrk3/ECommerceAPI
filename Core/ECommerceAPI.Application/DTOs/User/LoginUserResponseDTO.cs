using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.User
{
    public class LoginUserResponseDTO
    {
        public Token? Token { get; set; }

        //To return JWT-Authorization
    }
}
