using GroceryShop.Api.Controllers;
using GroceryShop.Application.CQRS.Commands;
using GroceryShop.Application.CQRS.Commands.CreateOrder;
using GroceryShop.Application.CQRS.Queries;
using GroceryShop.Core.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GroceryShop.Tests.ApplicationTests.Api.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new OrdersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtAction_WithOrderDto()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new CreateOrderCommand();

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default))
                         .ReturnsAsync(orderId);

            // Act
            var result = await _controller.CreateOrder(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(OrdersController.GetOrder), createdResult.ActionName);
            Assert.Equal(orderId, createdResult.Value);
        }



        [Fact]
        public async Task PurchaseLoyalty_ReturnsOk_WithResult()
        {
            // Arrange
            var command = new PurchaseLoyaltyCommand(Guid.NewGuid());
            var response = true;

            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.PurchaseLoyalty(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task GetOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var response = new OrderDto
            {
                OrderId = orderId,
                TotalAmount = 25.00m
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetOrderQuery>(), default))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }


        [Fact]
        public async Task GetOrder_Throws_WhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetOrderQuery>(), default))
                .ReturnsAsync((OrderDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.GetOrder(orderId));
        }

    }
}
