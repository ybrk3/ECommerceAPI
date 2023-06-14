
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities.Identity
{
    //IdentityRole<> contains props for identity management
    //This class to be add to db and to be used for role_based_authentication structure
    public class AppRole : IdentityRole<string>
    {

    }
}
