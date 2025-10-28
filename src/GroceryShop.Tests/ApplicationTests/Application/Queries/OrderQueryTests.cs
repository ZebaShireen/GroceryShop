using System;
using System.Threading;
using System.Threading.Tasks;
using GroceryShop.Application.CQRS.Handlers;
using GroceryShop.Application.CQRS.Queries;
using GroceryShop.Core.Domain.Entities;
using GroceryShop.Core.Interfaces;
using Moq;
using Xunit;

namespace GroceryShop.Tests.QueryHandlers
{
    public class GetOrderQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly GetOrderQueryHandler _handler;

        public GetOrderQueryHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _handler = new GetOrderQueryHandler(_orderRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsOrderDto_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderEntity = new Order
            {
                Id = orderId,
                TotalAmount = 50.0m,
                ShippingAddress = "123 Test Street"
            };

            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(orderEntity);

            var query = new GetOrderQuery(orderId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
            Assert.Equal(50.0m, result.TotalAmount);
            Assert.Equal("123 Test Street", result.ShippingAddress);

            _orderRepositoryMock.Verify(r => r.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync((Order)null);

            var query = new GetOrderQuery(orderId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _orderRepositoryMock.Verify(r => r.GetOrderByIdAsync(orderId), Times.Once);
        }
    }
}
