using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ECommerceAPI.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //HttpContext yapılan http ile ilgili bilgileri veren instance döner
            // 1- TokenHandler içerisinde claimType'a karşılık verilen username'e erişilir            
            var name = context.HttpContext.User.Identity?.Name;
            // 2- Gelen bilgi kontrol edilir
            if (!string.IsNullOrEmpty(name) && name is not "brkyldrm03") //brkyldrm03 added to development
            {
                #region Getting attribute info of endpoint and creating "Code" (those info will be checked from db)
                // 3- ActionDescriptor üzerinden action bilgileri (Örn: name) yakalanır ve ControllerActionDescriptor olarak referans edilir
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                // 4- Endpoint'e gelen istek doğrultusunda ilgili enpointin attribute bilgilerini elde ederiz. Bu bilgilere erişebilmek için türü değiştirilir diğer türlü attribute olarak döner ve bu attribute'un property'lerine erişemeyiz.
                var attribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                // 5- Yapılan isteğin HttpType'ına ihtiyaçımız var bu yüzden ilgili endpoint'in HttpMethodAttribute'una erişilir.
                var httpAttribute = descriptor?.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;
                // 6- Code'un unique standartı olduğu için bunu oluşturuyoruz. Kontrolde Code üzerinden sağlanacak. Eğer endpoint işaretlenmediyse bu default olarak Get'dir
                var code = $"{(httpAttribute is null ? httpAttribute?.HttpMethods.First() : HttpMethods.Get)}.{attribute?.ActionType}.{attribute?.Definition.Replace(" ", "")}";
                #endregion
                #region Check whether the User's permission to use this endpoint

                bool hasRolePermission = await _userService.HasRolePermissionToEndpointAsync(name, code);

                if (!hasRolePermission) context.Result = new UnauthorizedResult(); //If not have, throw UnauthorizedResult
                else await next(); //if have next to other middleware

                #endregion
            }
            //Eğer name bilgisi gelmediyse, herhangi bir kısıt yok demektir. Bu yüzden next ile diğer Middleware'e devam edilir.
            else
                await next();
        }
    }
}
