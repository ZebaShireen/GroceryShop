using FluentValidation;
using FluentValidation.Results;
using GroceryShop.Application.CQRS.Commands.CreateOrder;
using GroceryShop.Core.Domain.Entities;
using GroceryShop.Core.Interfaces;
using GroceryShop.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GroceryShop.Tests.CommandHandlers
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IValidator<CreateOrderCommand>> _validatorMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _validatorMock = new Mock<IValidator<CreateOrderCommand>>();
            _orderRepositoryMock = new Mock<IOrderRepository>();

            _handler = new CreateOrderCommandHandler(_validatorMock.Object, _orderRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsOrderId_WhenValidationSuccessful()
        {
            // Arrange
            var order = new CreateOrderCommand
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = "Test Address",
                OrderItems = new List<Item>
                {
                    new Item { ProductId = Guid.NewGuid(), Quantity = 2, Price = 5.0m }
                }
            };

            var expectedOrderId = Guid.NewGuid();

            _validatorMock
                .Setup(v => v.ValidateAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _orderRepositoryMock
                .Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(expectedOrderId);

            // Act
            var result = await _handler.Handle(order, CancellationToken.None);

            // Assert
            Assert.Equal(expectedOrderId, result);
            _orderRepositoryMock.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task Handle_MapsOrderItemsCorrectly()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new CreateOrderCommand
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = "Test Address",
                OrderItems = new List<Item>
                {
                    new Item { ProductId = productId, Quantity = 3, Price = 10m }
                }
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            Order capturedOrder = null;

            _orderRepositoryMock
                .Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .ReturnsAsync(Guid.NewGuid());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedOrder);
            Assert.Single(capturedOrder.Items);
            Assert.Equal(productId, capturedOrder.Items[0].ProductId);
            Assert.Equal(3, capturedOrder.Items[0].Quantity);
            Assert.Equal(10m, capturedOrder.Items[0].Price);
        }
    }
}
