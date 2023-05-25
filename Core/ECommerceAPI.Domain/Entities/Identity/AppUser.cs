
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities.Identity
{
    //IdentityUser contains props for identity management
    //This class to be add to db
    public class AppUser : IdentityUser<string>
    {
        //NameSurname not included in IdentityUser so we add it here
        public string NameSurname { get; set; }

        //For Refresh Token
        //While creating user it'll be null so it's nullable
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }

        public ICollection<Basket>? Baskets { get; set; }
    }
}
