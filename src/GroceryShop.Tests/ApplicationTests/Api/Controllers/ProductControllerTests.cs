using Xunit;
using Moq;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GroceryShop.Api.Controllers;
using GroceryShop.Core.DTOs;
using GroceryShop.Application.CQRS.Queries;

namespace GroceryShop.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOk_WithProductList()
        {
            // Arrange
            var expectedProducts = new List<ProductDto>
            {
                new ProductDto { Id = new System.Guid(), Name = "Apple", Price = 2.5M },
                new ProductDto { Id = new System.Guid(), Name = "Banana", Price = 1.5M }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(expectedProducts, returnedProducts);
        }

        [Fact]
        public async Task GetProducts_CallsMediatorWithCorrectQuery()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductDto>());

            // Act
            await _controller.GetProducts();

            // Assert
            _mediatorMock.Verify(
                m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}
