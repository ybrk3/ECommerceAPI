using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.DTOs.Configuration;
using ECommerceAPI.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Configurations
{
    public sealed class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoint(Type type)
        {
            //Get the assembly of API to get it's controllers. Type of the attribute will get from parameter of this method and will be typeof(program)
            Assembly assembly = Assembly.GetAssembly(type);

            //Get "Controllers" in API which get as Assembly
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            //Create a Menu List to set our custom attributes and method/action/end-point which is defined with our custom attribute, properties 
            List<Menu> menus = new();

            //If controllers which is in type derived from ControllerBase, exist
            if (controllers != null)
            {
                //Get each controller and get it's methods (/actions/end-point) which are defined with our Attribute (AuthorizeDefinitionAttribute)
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));

                    //if methods exist, get the actions/methods/end-point which is defined with our Attribute (AuthorizeDefinitionAttribute)
                    if (actions != null)
                    {
                        foreach (var action in actions)
                        {
                            //get  attributes of actions/methods/end-point
                            var attributes = action.GetCustomAttributes(true);

                            //If attributes exist, get our custom attribute (AuthorizeDefinitionAttribute)
                            if (attributes != null)
                            {
                                //to set our custom attribute's info to menus list
                                Menu menu = null;

                                //Get our custom attribute
                                var authorizeDefinitionAttribute = attributes.FirstOrDefault(att => att.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                                //If menus list does not have menu with same above attributes name, create it. Else, get the menu element from menus list with same name of attributes menu
                                if (!menus.Any(m => m.MenuName == authorizeDefinitionAttribute.Menu))
                                {
                                    menu = new() { MenuName = authorizeDefinitionAttribute.Menu };
                                    menus.Add(menu);
                                }
                                else
                                    menu = menus.FirstOrDefault(m => m.MenuName == authorizeDefinitionAttribute.Menu);
                                //Create a Action instance with its properties got from our custom attribute
                                Application.DTOs.Configuration.Action _action = new()
                                {
                                    ActionType = Enum.GetName(typeof(ActionType), authorizeDefinitionAttribute.ActionType),
                                    Definition = authorizeDefinitionAttribute.Definition,
                                };
                                //get the httptype of the action/method/end-point via attributes and set it to our Action type variable
                                var httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;

                                //if our method has httpType attribute, get its method. Else set it as "Get"
                                if (httpAttribute != null)
                                    _action.HttpType = httpAttribute.HttpMethods.First();
                                else
                                    _action.HttpType = HttpMethods.Get;


                                //Create Unique Code for Action
                                _action.ActionCode= $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ", "")}";
                                //Add our action properties which is Action object to Action type Action property of our menu
                                menu.Actions.Add(_action);
                            }
                        };
                    }
                }
            }
            return menus;
        }
    }
}
