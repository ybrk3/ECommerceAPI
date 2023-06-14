using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes="Admin")]
    public class ApplicationServicesController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }


        [HttpGet]
        [AuthorizeDefinition(Menu ="Application Service",ActionType =ActionType.Reading, Definition = "Get Authorize Definition Endpoint")]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            var datas = _applicationService.GetAuthorizeDefinitionEndpoint(typeof(Program));
            return Ok(datas);
        }
    }
}
