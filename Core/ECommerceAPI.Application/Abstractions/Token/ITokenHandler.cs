using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Token
{
    //it consist of methods to be used while creating and refreshing tokens
    public interface ITokenHandler
    {
        //To create Token
        Application.DTOs.Token CreateAccessToken(int expiration);

        //To create resresh token
        string CreateRefreshToken();

    }
}


/*Note:
 * 
 * DTO: Either a model to be sent to view and also model carrying data between services (kinda CQRS response classes)
 * 
 * ViewModel: model to be sent to view
 */