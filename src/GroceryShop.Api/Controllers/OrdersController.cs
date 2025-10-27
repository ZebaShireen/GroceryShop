using GroceryShop.Application.CQRS.Commands;
using GroceryShop.Application.CQRS.Commands.CreateOrder;
using GroceryShop.Application.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { orderId = result }, result);
        }

        [HttpPost("purchase-loyalty")]
        public async Task<IActionResult> PurchaseLoyalty([FromBody] PurchaseLoyaltyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var query = new GetOrderQuery(orderId);
            var result = await _mediator.Send(query);

            if (result == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            return Ok(result);
        }
    }
}