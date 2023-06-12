using ECommerceAPI.Application.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Configuration
{
    public class Menu
    {
        public string MenuName { get; set; } //represents the name of the controller
        public List<Action> Actions { get; set; } = new(); //includes info of end-point which is defines with AuthorizeDefinitionAttribute. We create instance because when using in the GetAuthorizeDefinitionEndpoint via creating Menu List element, it cannot be null
    }
}

