using Xunit;
using Moq;
using System.Threading.Tasks;
using MediatR;
using GroceryShop.Api.Controllers;
using GroceryShop.Application.CQRS.Commands.CreateOrder;

namespace GroceryShop.Tests.Controllers
{
    public class OrdersControllerValidationTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly OrdersController _controller;

        public OrdersControllerValidationTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new OrdersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateOrder_ThrowsValidationException_WhenInvalid()
        {
            // Arrange
            var command = new CreateOrderCommand();

            _mediatorMock.Setup(m => m.Send(command, default))
                .ThrowsAsync(new FluentValidation.ValidationException("Invalid"));

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                () => _controller.CreateOrder(command)
            );
        }
    }
}
