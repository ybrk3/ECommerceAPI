using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Features.Commands.OrderCommands.CompleteOrder;
using ECommerceAPI.Application.Features.Commands.OrderCommands.CreateOrder;
using ECommerceAPI.Application.Features.Commands.OrderCommands.DeleteOrder;
using ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrderById;
using ECommerceAPI.Application.Features.Queries.OrderQueries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")] //without this, jwt will not be authorized
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AuthorizeDefinition(Menu =AuthorizeDefinitionConstants.Orders, ActionType =ActionType.Writing, Definition ="Create Order")]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
        {
            CreateOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Orders")]
        public async Task<IActionResult> GetOrders([FromQuery] GetOrdersQueryRequest request)
        {
            GetOrdersQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Order By Id")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] GetOrderByIdAsyncQueryRequest request)
        {
            GetOrderByIdAsyncQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Deleting, Definition = "Delete Order")]
        public async Task<IActionResult> DeleteOrder([FromRoute] DeleteOrderCommandRequest request)
        {
            DeleteOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("complete-order/{orderId}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Updating, Definition = "Complete Order")]
        public async Task<IActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest request)
        {
            CompleteOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
